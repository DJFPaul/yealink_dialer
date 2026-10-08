use crate::settings::Settings;
use serde::Serialize;
use serde_json::Value;

#[derive(Clone, Debug, Serialize, PartialEq, Eq)]
#[serde(rename_all = "camelCase")]
pub struct Contact {
    pub name: String,
    pub number: String,
}

pub fn fetch(settings: &Settings) -> Result<Vec<Contact>, String> {
    let url = settings.contacts_url.trim();
    if url.is_empty() {
        return Ok(Vec::new());
    }
    let client = reqwest::blocking::Client::builder()
        .timeout(std::time::Duration::from_secs(8))
        .build()
        .map_err(|err| err.to_string())?;
    let response = client
        .get(url)
        .header("accept", "application/xml, text/xml, application/json, */*")
        .send()
        .map_err(|err| format!("مخاطبین خوانده نشد: {err}"))?;
    if !response.status().is_success() {
        return Err(format!("مخاطبین خوانده نشد: {}", response.status()));
    }
    let text = response.text().map_err(|err| err.to_string())?;
    parse_contacts(&text)
}

pub fn parse_contacts(text: &str) -> Result<Vec<Contact>, String> {
    let trimmed = text.trim().trim_start_matches('\u{feff}');
    if trimmed.starts_with('<') {
        return parse_phonebook_xml(trimmed);
    }
    let value: Value = serde_json::from_str(trimmed).map_err(|_| "پاسخ مخاطبین JSON یا XML نیست.".to_string())?;
    let list = match &value {
        Value::Array(items) => items,
        Value::Object(map) => ["contacts", "data", "items", "result", "rows"]
            .iter()
            .find_map(|key| map.get(*key).and_then(Value::as_array))
            .ok_or("فهرست مخاطبین در پاسخ پیدا نشد.")?,
        _ => return Err("فهرست مخاطبین در پاسخ پیدا نشد.".into()),
    };
    let mut contacts = Vec::new();
    for item in list {
        let Some(number) = first_text(item, &["number", "phone", "extension", "tel", "mobile", "phoneNumber"]) else {
            continue;
        };
        if number.is_empty() {
            continue;
        }
        let name = first_text(item, &["name", "displayName", "fullName", "contactName", "title"]).unwrap_or_default();
        contacts.push(Contact { name, number });
    }
    Ok(contacts)
}

fn parse_phonebook_xml(text: &str) -> Result<Vec<Contact>, String> {
    use quick_xml::events::Event;
    use quick_xml::Reader;

    let mut reader = Reader::from_str(text);
    reader.config_mut().trim_text(true);
    let mut buf = Vec::new();
    let mut contacts = Vec::new();
    let mut in_entry = false;
    let mut tag = String::new();
    let mut label = String::new();
    let mut name = String::new();
    let mut phones: Vec<(String, String)> = Vec::new();

    loop {
        match reader.read_event_into(&mut buf) {
            Ok(Event::Start(event)) => {
                tag = local_name(event.name().as_ref());
                if tag.eq_ignore_ascii_case("DirectoryEntry") {
                    in_entry = true;
                    name.clear();
                    phones.clear();
                    label.clear();
                } else if tag.eq_ignore_ascii_case("Telephone") {
                    label = attribute(&event, "label");
                }
            }
            Ok(Event::Text(event)) => {
                if in_entry {
                    let value = event.unescape().map_err(|err| err.to_string())?.trim().to_string();
                    if tag.eq_ignore_ascii_case("Name") {
                        name = value;
                    } else if tag.eq_ignore_ascii_case("Telephone") && !value.is_empty() {
                        phones.push((label.clone(), value));
                    }
                }
            }
            Ok(Event::End(event)) => {
                if local_name(event.name().as_ref()).eq_ignore_ascii_case("DirectoryEntry") {
                    contacts.extend(entry_contacts(&name, &phones));
                    in_entry = false;
                    phones.clear();
                }
                tag.clear();
            }
            Ok(Event::Eof) => break,
            Err(err) => return Err(format!("دفترچه تلفن خوانده نشد: {err}")),
            _ => {}
        }
        buf.clear();
    }
    if contacts.is_empty() {
        return Err("در دفترچه تلفن مخاطبی پیدا نشد.".into());
    }
    Ok(contacts)
}

fn entry_contacts(name: &str, phones: &[(String, String)]) -> Vec<Contact> {
    let base = name.trim();
    phones
        .iter()
        .filter(|(_, number)| !number.trim().is_empty())
        .map(|(label, number)| {
            let kind = phone_kind(label);
            let display = if phones.len() > 1 && !kind.is_empty() && !base.is_empty() {
                format!("{base} ({kind})")
            } else if base.is_empty() {
                kind
            } else {
                base.to_string()
            };
            Contact {
                name: display,
                number: number.trim().to_string(),
            }
        })
        .collect()
}

fn phone_kind(label: &str) -> String {
    match label.trim().to_ascii_lowercase().as_str() {
        "mobile" | "cell" | "cellphone" => "موبایل".into(),
        "office" | "work" | "extension" | "ext" => "داخلی".into(),
        "home" => "منزل".into(),
        "fax" => "فکس".into(),
        "" => String::new(),
        other => other.to_string(),
    }
}

fn local_name(raw: &[u8]) -> String {
    let name = String::from_utf8_lossy(raw);
    name.rsplit(':').next().unwrap_or(name.as_ref()).to_string()
}

fn attribute(event: &quick_xml::events::BytesStart<'_>, key: &str) -> String {
    event
        .try_get_attribute(key)
        .ok()
        .flatten()
        .and_then(|attr| attr.unescape_value().ok())
        .map(|value| value.trim().to_string())
        .unwrap_or_default()
}

fn first_text(item: &Value, keys: &[&str]) -> Option<String> {
    let object = item.as_object()?;
    for key in keys {
        if let Some(value) = object.get(*key) {
            if let Some(text) = value.as_str() {
                let text = text.trim();
                if !text.is_empty() {
                    return Some(text.to_string());
                }
            } else if let Some(number) = value.as_i64() {
                return Some(number.to_string());
            } else if let Some(number) = value.as_u64() {
                return Some(number.to_string());
            }
        }
    }
    None
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn reads_wrapped_contacts() {
        let contacts = parse_contacts(
            r#"{"data":[{"displayName":"ندا","extension":"101"},{"name":"","phone":""}]}"#,
        )
        .unwrap();
        assert_eq!(
            contacts,
            vec![Contact {
                name: "ندا".into(),
                number: "101".into()
            }]
        );
    }

    #[test]
    fn reads_yealink_phonebook() {
        let contacts = parse_contacts(
            r#"<?xml version="1.0" encoding="UTF-8"?>
<YealinkIPPhoneDirectory>
  <DirectoryEntry>
    <Name>امیر علی ایرانی</Name>
    <Telephone label="Mobile">09367498293</Telephone>
  </DirectoryEntry>
  <DirectoryEntry>
    <Name>سیاوش</Name>
    <Telephone label="Office">502</Telephone>
    <Telephone label="Mobile">09125318049</Telephone>
  </DirectoryEntry>
</YealinkIPPhoneDirectory>"#,
        )
        .unwrap();
        assert_eq!(
            contacts,
            vec![
                Contact { name: "امیر علی ایرانی".into(), number: "09367498293".into() },
                Contact { name: "سیاوش (داخلی)".into(), number: "502".into() },
                Contact { name: "سیاوش (موبایل)".into(), number: "09125318049".into() },
            ]
        );
    }
}
