mod contacts;
mod history;
mod launch;
mod legacy;
mod notify;
mod phone;
mod profiles;
mod settings;

use std::fs;
use std::path::PathBuf;
use std::sync::Mutex;

use tauri::menu::{Menu, MenuItem};
use tauri::tray::{MouseButton, MouseButtonState, TrayIconBuilder, TrayIconEvent};
use tauri::{Emitter, Manager, WindowEvent};

use contacts::Contact;
use history::HistoryEntry;
use launch::Launch;
use settings::Settings;

struct AppState {
    settings_path: PathBuf,
    history_path: PathBuf,
    settings: Mutex<Settings>,
    pending_launch: Mutex<Launch>,
    callback_port: Mutex<u16>,
}

#[tauri::command]
fn get_settings(state: tauri::State<AppState>) -> Settings {
    lock(&state.settings).clone()
}

#[tauri::command]
fn save_settings(app: tauri::AppHandle, state: tauri::State<AppState>, settings: Settings) -> Result<Settings, String> {
    let settings = settings.normalized()?;
    settings::save(&state.settings_path, &settings)?;
    *lock(&state.settings) = settings.clone();
    apply_autostart(&app, settings.autostart)?;
    Ok(settings)
}

#[tauri::command]
fn list_models() -> Vec<ModelInfo> {
    profiles::all()
        .iter()
        .map(|profile| ModelInfo {
            id: profile.id.into(),
            label: profile.label.into(),
            note: profile.note.into(),
        })
        .collect()
}

#[tauri::command]
fn take_launch(state: tauri::State<AppState>) -> Launch {
    std::mem::take(&mut *lock(&state.pending_launch))
}

#[tauri::command]
async fn dial(state: tauri::State<'_, AppState>, number: String) -> Result<(), String> {
    let settings = lock(&state.settings).clone();
    tauri::async_runtime::spawn_blocking(move || phone::dial(&settings, &number))
        .await
        .map_err(|err| err.to_string())?
}

#[tauri::command]
async fn phone_action(state: tauri::State<'_, AppState>, action: String, number: Option<String>) -> Result<(), String> {
    let settings = lock(&state.settings).clone();
    tauri::async_runtime::spawn_blocking(move || phone::action(&settings, &action, number.as_deref()))
        .await
        .map_err(|err| err.to_string())?
}

#[tauri::command]
async fn fetch_contacts(state: tauri::State<'_, AppState>) -> Result<Vec<Contact>, String> {
    let settings = lock(&state.settings).clone();
    tauri::async_runtime::spawn_blocking(move || contacts::fetch(&settings))
        .await
        .map_err(|err| err.to_string())?
}

#[tauri::command]
fn callback_info(state: tauri::State<AppState>) -> CallbackInfo {
    callback_from(&state)
}

#[tauri::command]
async fn sync_action_urls(state: tauri::State<'_, AppState>) -> Result<String, String> {
    let settings = lock(&state.settings).clone();
    let info = callback_from(&state);
    if !info.listening || info.incoming_url.is_empty() {
        return Err("شنونده تماس روی این کامپیوتر روشن نیست.".into());
    }
    let cfg = notify::action_cfg(&info.address, info.port);
    let note = tauri::async_runtime::spawn_blocking(move || phone::upload_action_cfg(&settings, &cfg))
        .await
        .map_err(|err| err.to_string())??;
    Ok(format!("{note}. آدرس Incoming call: {}", info.incoming_url))
}

#[tauri::command]
fn list_history(state: tauri::State<AppState>) -> Vec<HistoryEntry> {
    history::load(&state.history_path)
}

#[tauri::command]
fn remember_call(
    state: tauri::State<AppState>,
    direction: String,
    number: String,
    name: String,
    result: String,
) -> Result<(), String> {
    history::remember(&state.history_path, &direction, &number, &name, &result).map(|_| ())
}

#[tauri::command]
fn finish_last_call(state: tauri::State<AppState>, result: String) -> Result<bool, String> {
    history::finish(&state.history_path, &result).map(|(updated, _)| updated)
}

#[tauri::command]
fn import_legacy(folder: String) -> Result<Settings, String> {
    legacy::import_folder(std::path::Path::new(&folder))
}

#[derive(serde::Serialize)]
#[serde(rename_all = "camelCase")]
struct ModelInfo {
    id: String,
    label: String,
    note: String,
}

#[derive(Clone, serde::Serialize)]
#[serde(rename_all = "camelCase")]
struct CallbackInfo {
    port: u16,
    address: String,
    incoming_url: String,
    listening: bool,
}

fn callback_from(state: &AppState) -> CallbackInfo {
    let port = *lock(&state.callback_port);
    let phone_ip = lock(&state.settings).phone_ip.clone();
    let address = notify::local_ip_toward(&phone_ip).unwrap_or_default();
    let incoming_url = if port == 0 || address.is_empty() {
        String::new()
    } else {
        notify::event_url(&address, port, "incoming")
    };
    CallbackInfo {
        port,
        address,
        incoming_url,
        listening: port != 0,
    }
}

#[cfg_attr(mobile, tauri::mobile_entry_point)]
pub fn run() {
    tauri::Builder::default()
        .plugin(tauri_plugin_single_instance::init(|app, args, _cwd| {
            let launch = launch::parse_args(args);
            show_main(&app);
            if launch.should_surface() {
                let _ = app.emit("launch", &launch);
            }
        }))
        .plugin(tauri_plugin_deep_link::init())
        .plugin(tauri_plugin_autostart::init(
            tauri_plugin_autostart::MacosLauncher::LaunchAgent,
            Some(vec!["--autostart"]),
        ))
        .plugin(tauri_plugin_opener::init())
        .invoke_handler(tauri::generate_handler![
            get_settings,
            save_settings,
            list_models,
            take_launch,
            dial,
            phone_action,
            fetch_contacts,
            callback_info,
            sync_action_urls,
            list_history,
            remember_call,
            finish_last_call,
            import_legacy
        ])
        .setup(|app| {
            let dir = app.path().app_config_dir()?;
            fs::create_dir_all(&dir)?;
            let settings_path = dir.join("settings.json");
            let history_path = dir.join("history.json");
            let saved = settings::load(&settings_path);
            let launch = launch::parse_args(std::env::args());
            let hide = launch.autostart && saved.setup_complete && !launch.should_surface();
            app.manage(AppState {
                settings_path,
                history_path,
                settings: Mutex::new(saved),
                pending_launch: Mutex::new(launch),
                callback_port: Mutex::new(0),
            });
            let events = app.handle().clone();
            match notify::start(move |peer, launch| {
                let phone_ip = lock(&events.state::<AppState>().settings).phone_ip.clone();
                if !notify::accepts(&phone_ip, peer.ip()) {
                    eprintln!("phone event ignored from {}", peer.ip());
                    return;
                }
                eprintln!(
                    "phone event incoming={} event={} number={}",
                    launch.incoming, launch.event_name, launch.number
                );
                show_main(&events);
                let _ = events.emit("launch", &launch);
            }) {
                Ok(port) => *lock(&app.state::<AppState>().callback_port) = port,
                Err(err) => eprintln!("callback listener: {err}"),
            }
            push_phone_events(app.handle().clone());
            install_tray(app.handle())?;
            install_close_to_tray(app.handle());
            if !hide {
                show_main(app.handle());
            }
            if let Err(err) = register_links(app.handle()) {
                eprintln!("protocol registration: {err}");
            }
            Ok(())
        })
        .run(tauri::generate_context!())
        .expect("error while running tauri application");
}

fn push_phone_events(app: tauri::AppHandle) {
    tauri::async_runtime::spawn(async move {
        let state = app.state::<AppState>();
        let settings = lock(&state.settings).clone();
        let info = callback_from(&state);
        if !settings.setup_complete || info.incoming_url.is_empty() {
            eprintln!("action urls skipped port={} address={}", info.port, info.address);
            return;
        }
        let cfg = notify::action_cfg(&info.address, info.port);
        match tauri::async_runtime::spawn_blocking(move || phone::upload_action_cfg(&settings, &cfg)).await {
            Ok(Ok(note)) => eprintln!("action urls: {note}"),
            Ok(Err(err)) => eprintln!("action urls: {err}"),
            Err(err) => eprintln!("action urls: {err}"),
        }
    });
}

fn apply_autostart(app: &tauri::AppHandle, enabled: bool) -> Result<(), String> {
    use tauri_plugin_autostart::ManagerExt;
    let launcher = app.autolaunch();
    if enabled {
        launcher.enable().map_err(|err| format!("اجرای خودکار ثبت نشد: {err}"))
    } else {
        launcher.disable().map_err(|err| format!("اجرای خودکار برداشته نشد: {err}"))
    }
}

fn register_links(app: &tauri::AppHandle) -> Result<(), String> {
    use tauri_plugin_deep_link::DeepLinkExt;
    app.deep_link().register_all().map_err(|err| err.to_string())
}

fn install_tray(app: &tauri::AppHandle) -> tauri::Result<()> {
    let open = MenuItem::with_id(app, "open", "باز کردن", true, None::<&str>)?;
    let quit = MenuItem::with_id(app, "quit", "خروج", true, None::<&str>)?;
    let menu = Menu::with_items(app, &[&open, &quit])?;
    let mut builder = TrayIconBuilder::new()
        .menu(&menu)
        .tooltip("شماره‌گیر یالینک")
        .show_menu_on_left_click(false)
        .on_menu_event(|app, event| match event.id.as_ref() {
            "open" => show_main(app),
            "quit" => app.exit(0),
            _ => {}
        })
        .on_tray_icon_event(|tray, event| {
            if let TrayIconEvent::Click {
                button: MouseButton::Left,
                button_state: MouseButtonState::Up,
                ..
            } = event
            {
                show_main(tray.app_handle());
            }
        });
    if let Some(icon) = app.default_window_icon() {
        builder = builder.icon(icon.clone());
    }
    builder.build(app)?;
    Ok(())
}

fn install_close_to_tray(app: &tauri::AppHandle) {
    let Some(window) = app.get_webview_window("main") else {
        return;
    };
    let hidden = window.clone();
    window.on_window_event(move |event| {
        if let WindowEvent::CloseRequested { api, .. } = event {
            api.prevent_close();
            let _ = hidden.hide();
        }
    });
}

fn show_main(app: &tauri::AppHandle) {
    let Some(window) = app.get_webview_window("main") else {
        return;
    };
    let _ = window.show();
    let _ = window.unminimize();
    let _ = window.set_focus();
}

fn lock<T>(mutex: &Mutex<T>) -> std::sync::MutexGuard<'_, T> {
    mutex.lock().unwrap_or_else(|err| err.into_inner())
}
