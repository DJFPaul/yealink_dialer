use std::fs;
use std::path::Path;

use chrono::Local;

#[derive(Clone, Debug, serde::Serialize, serde::Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct HistoryEntry {
    pub at: String,
    pub direction: String,
    pub number: String,
    pub name: String,
    pub result: String,
}

const OPEN_RESULTS: &[&str] = &["زنگ", "در حال تماس", "پاسخ داده", "برقرار"];

pub fn load(path: &Path) -> Vec<HistoryEntry> {
    let Ok(text) = fs::read_to_string(path) else {
        return Vec::new();
    };
    serde_json::from_str(&text).unwrap_or_default()
}

pub fn remember(
    path: &Path,
    direction: &str,
    number: &str,
    name: &str,
    result: &str,
) -> Result<Vec<HistoryEntry>, String> {
    let mut entries = load(path);
    entries.insert(
        0,
        HistoryEntry {
            at: Local::now().format("%Y/%m/%d %H:%M").to_string(),
            direction: clean(direction),
            number: clean(number),
            name: clean(name),
            result: clean(result),
        },
    );
    if entries.len() > 200 {
        entries.truncate(200);
    }
    write(path, &entries)?;
    Ok(entries)
}

pub fn finish(path: &Path, result: &str) -> Result<(bool, Vec<HistoryEntry>), String> {
    let mut entries = load(path);
    let Some(entry) = entries.first_mut() else {
        return Ok((false, entries));
    };
    if !OPEN_RESULTS.contains(&entry.result.as_str()) {
        return Ok((false, entries));
    }
    entry.result = clean(result);
    write(path, &entries)?;
    Ok((true, entries))
}

fn clean(value: &str) -> String {
    value.replace(['|', '\r', '\n'], " ").trim().to_string()
}

fn write(path: &Path, entries: &[HistoryEntry]) -> Result<(), String> {
    if let Some(parent) = path.parent() {
        fs::create_dir_all(parent).map_err(|err| err.to_string())?;
    }
    let text = serde_json::to_string_pretty(entries).map_err(|err| err.to_string())?;
    fs::write(path, text).map_err(|err| err.to_string())
}
