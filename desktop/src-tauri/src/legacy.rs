use std::fs;
use std::path::Path;

use aes::Aes128;
use base64::{engine::general_purpose::STANDARD, Engine as _};
use cbc::Decryptor;
use cipher::{block_padding::Pkcs7, BlockDecryptMut, KeyIvInit};
use md5::{Digest, Md5};

use crate::settings::Settings;

type Aes128CbcDec = Decryptor<Aes128>;

pub fn import_folder(folder: &Path) -> Result<Settings, String> {
    let secret = fs::read_to_string(folder.join("secret.cfg")).map_err(|_| "secret.cfg پیدا نشد.")?;
    let phone = fs::read_to_string(folder.join("phone.cfg")).map_err(|_| "phone.cfg پیدا نشد.")?;
    let text = decrypt(strip_bom(secret.trim()), strip_bom(phone.trim()))?;
    let mut settings = Settings::default();
    settings.phone_ip = value(&text, "PHONEIP");
    settings.sip_account = value(&text, "SIPACCOUNT");
    settings.web_username = value(&text, "WEBUSERNAME")
        .trim_start_matches("USERNAME=")
        .to_string();
    settings.web_password = value(&text, "WEBPASSWORD")
        .trim_start_matches("PASSWORD=")
        .to_string();
    settings.use_ssl = value(&text, "usessl").eq_ignore_ascii_case("true");
    if settings.phone_ip.is_empty() {
        return Err("در فایل قبلی آی‌پی تلفن پیدا نشد.".into());
    }
    Ok(settings)
}

fn value(text: &str, key: &str) -> String {
    let prefix = format!("{key}=");
    text.lines()
        .find_map(|line| line.trim().strip_prefix(&prefix))
        .unwrap_or("")
        .trim()
        .to_string()
}

fn strip_bom(text: &str) -> &str {
    text.trim_start_matches('\u{feff}')
}

fn decrypt(secret: &str, encoded: &str) -> Result<String, String> {
    let cleaned: String = encoded.chars().filter(|ch| !ch.is_whitespace()).collect();
    let blob = STANDARD
        .decode(cleaned)
        .map_err(|_| "phone.cfg قابل خواندن نیست.".to_string())?;
    if blob.len() <= 16 {
        return Err("phone.cfg ناقص است.".into());
    }
    let (iv, data) = blob.split_at(16);
    let key = Md5::digest(secret.as_bytes());
    let cipher = Aes128CbcDec::new_from_slices(key.as_slice(), iv)
        .map_err(|_| "رمزگشایی تنظیمات قبلی ناموفق بود.".to_string())?;
    let mut buffer = data.to_vec();
    let plain = cipher
        .decrypt_padded_mut::<Pkcs7>(&mut buffer)
        .map_err(|_| "رمزگشایی تنظیمات قبلی ناموفق بود.".to_string())?;
    String::from_utf8(plain.to_vec()).map_err(|_| "متن تنظیمات قبلی معتبر نیست.".to_string())
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn reads_legacy_lines() {
        let text = "MINVERSION=3.0\nPHONEIP=10.0.0.8\nWEBUSERNAME=admin\n";
        assert_eq!(value(text, "PHONEIP"), "10.0.0.8");
    }

    #[test]
    fn imports_existing_dialer_config_when_present() {
        let folder = std::path::Path::new(env!("CARGO_MANIFEST_DIR")).join("../../yealink_dialer/bin/Debug/data");
        if !folder.join("phone.cfg").exists() {
            return;
        }
        let settings = import_folder(&folder).expect("legacy config");
        assert!(!settings.phone_ip.is_empty());
        assert!(!settings.web_username.is_empty());
    }
}
