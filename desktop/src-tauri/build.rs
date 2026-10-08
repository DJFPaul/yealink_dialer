fn main() {
    let attributes = tauri_build::Attributes::new().app_manifest(
        tauri_build::AppManifest::new().commands(&[
            "get_settings",
            "save_settings",
            "list_models",
            "take_launch",
            "dial",
            "phone_action",
            "fetch_contacts",
            "callback_info",
            "sync_action_urls",
            "list_history",
            "remember_call",
            "finish_last_call",
            "import_legacy",
            "app_version",
            "check_update",
            "install_update",
        ]),
    );
    tauri_build::try_build(attributes).expect("failed to run tauri-build");
}
