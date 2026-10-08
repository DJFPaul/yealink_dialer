use tauri::{AppHandle, Emitter};
use tauri_plugin_updater::UpdaterExt;
use url::Url;

use crate::settings::Settings;

#[derive(Clone, serde::Serialize)]
#[serde(rename_all = "camelCase")]
pub struct UpdateNotice {
    pub version: String,
    pub notes: String,
}

#[derive(Clone, serde::Serialize)]
struct UpdateProgress {
    received: usize,
    total: Option<u64>,
}

pub fn endpoint(settings: &Settings) -> Result<Url, String> {
    let raw = settings.contacts_url.trim();
    if raw.is_empty() {
        return Err("آدرس دفترچه تلفن تنظیم نشده است.".into());
    }
    let mut endpoint = Url::parse(raw).map_err(|_| "آدرس دفترچه تلفن معتبر نیست.".to_string())?;
    if endpoint.scheme() != "https" {
        return Err("به‌روزرسانی از همان سرور دفترچه خوانده می‌شود و آن آدرس باید https باشد.".into());
    }
    endpoint.set_path("/updates/latest.json");
    endpoint.set_query(None);
    endpoint.set_fragment(None);
    Ok(endpoint)
}

pub async fn check(app: &AppHandle, settings: &Settings) -> Result<Option<UpdateNotice>, String> {
    let Some(update) = find(app, settings).await? else {
        return Ok(None);
    };
    Ok(Some(UpdateNotice {
        version: update.version,
        notes: update.body.unwrap_or_default(),
    }))
}

pub async fn install(app: &AppHandle, settings: &Settings) -> Result<(), String> {
    let Some(update) = find(app, settings).await? else {
        return Err("نسخه جدیدی منتشر نشده است.".into());
    };
    let mut received = 0usize;
    let progress = app.clone();
    update
        .download_and_install(
            move |chunk, total| {
                received += chunk;
                let _ = progress.emit("update-progress", UpdateProgress { received, total });
            },
            || {},
        )
        .await
        .map_err(|err| format!("نصب به‌روزرسانی انجام نشد: {err}"))?;
    Ok(())
}

async fn find(app: &AppHandle, settings: &Settings) -> Result<Option<tauri_plugin_updater::Update>, String> {
    let endpoint = endpoint(settings)?;
    let updater = app
        .updater_builder()
        .endpoints(vec![endpoint])
        .map_err(|err| err.to_string())?
        .build()
        .map_err(|err| err.to_string())?;
    updater
        .check()
        .await
        .map_err(|err| format!("بررسی به‌روزرسانی انجام نشد: {err}"))
}
