use std::fs;
use std::path::Path;

#[derive(Clone, Debug, serde::Serialize, serde::Deserialize)]
#[serde(rename_all = "camelCase", default)]
pub struct Settings {
    pub phone_ip: String,
    pub sip_account: String,
    pub web_username: String,
    pub web_password: String,
    pub use_ssl: bool,
    pub phone_model: String,
    pub contacts_url: String,
    pub action_cfg_url: String,
    pub autostart: bool,
    pub setup_complete: bool,
}

impl Default for Settings {
    fn default() -> Self {
        Self {
            phone_ip: String::new(),
            sip_account: String::new(),
            web_username: "admin".into(),
            web_password: String::new(),
            use_ssl: false,
            phone_model: "t31p".into(),
            contacts_url: String::new(),
            action_cfg_url: "http://127.0.0.1:8080/api/yealink-action.cfg".into(),
            autostart: true,
            setup_complete: false,
        }
    }
}

impl Settings {
    pub fn normalized(mut self) -> Result<Self, String> {
        self.phone_ip = self.phone_ip.trim().to_string();
        self.sip_account = self.sip_account.trim().to_string();
        self.web_username = self.web_username.trim().to_string();
        self.contacts_url = self.contacts_url.trim().to_string();
        self.action_cfg_url = self.action_cfg_url.trim().to_string();
        self.phone_model = self.phone_model.trim().to_string();
        if self.phone_model.is_empty() {
            self.phone_model = "t31p".into();
        }
        if crate::profiles::get(&self.phone_model).id != self.phone_model {
            return Err("مدل تلفن شناخته نشد.".into());
        }
        if self.setup_complete && self.phone_ip.is_empty() {
            return Err("آی‌پی تلفن لازم است.".into());
        }
        if self.setup_complete && self.web_username.is_empty() {
            return Err("نام کاربری وب تلفن لازم است.".into());
        }
        if self.phone_ip.chars().any(|ch| ch.is_whitespace() || ch == '/' || ch == '\\') {
            return Err("آی‌پی تلفن معتبر نیست.".into());
        }
        Ok(self)
    }
}

pub fn load(path: &Path) -> Settings {
    let Ok(text) = fs::read_to_string(path) else {
        return Settings::default();
    };
    serde_json::from_str(&text).unwrap_or_default()
}

pub fn save(path: &Path, settings: &Settings) -> Result<(), String> {
    if let Some(parent) = path.parent() {
        fs::create_dir_all(parent).map_err(|err| err.to_string())?;
    }
    let text = serde_json::to_string_pretty(settings).map_err(|err| err.to_string())?;
    fs::write(path, text).map_err(|err| err.to_string())
}
