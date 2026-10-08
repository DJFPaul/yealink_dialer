#[derive(Clone, Debug, Default, PartialEq, Eq, serde::Serialize, serde::Deserialize)]
#[serde(rename_all = "camelCase")]
pub struct Launch {
    pub number: String,
    pub name: String,
    pub incoming: bool,
    pub event_name: String,
    pub autostart: bool,
}

impl Launch {
    pub fn should_surface(&self) -> bool {
        self.incoming || !self.event_name.is_empty() || !self.number.is_empty()
    }
}

pub fn parse_args<I>(args: I) -> Launch
where
    I: IntoIterator<Item = String>,
{
    let mut launch = Launch::default();
    let mut positionals = Vec::new();
    let mut args = args.into_iter().peekable();
    while let Some(arg) = args.next() {
        if is_executable(&arg) {
            continue;
        }
        match arg.as_str() {
            "--autostart" => launch.autostart = true,
            "--incoming" => launch.incoming = true,
            "--event" => {
                if let Some(name) = args.next() {
                    launch.event_name = name;
                }
            }
            _ => positionals.push(arg),
        }
    }
    if let Some(raw) = positionals.first() {
        launch.number = extract_number(raw);
    }
    launch.name = positionals
        .iter()
        .skip(1)
        .cloned()
        .collect::<Vec<_>>()
        .join(" ")
        .trim()
        .to_string();
    launch
}

fn is_executable(arg: &str) -> bool {
    let lower = arg.to_ascii_lowercase();
    lower.ends_with(".exe") || lower.contains("\\ydialer") || lower.contains("/ydialer")
}

pub fn extract_number(raw: &str) -> String {
    let trimmed = raw.trim().trim_matches('"').trim();
    let lower = trimmed.to_ascii_lowercase();
    let mut rest = trimmed;
    for prefix in ["callto://", "callto:", "tel:", "phone:", "sip:"] {
        if lower.starts_with(prefix) {
            rest = &trimmed[prefix.len()..];
            break;
        }
    }
    let rest = rest.trim_start_matches('/');
    let user = rest.split(['@', ';', '?']).next().unwrap_or(rest);
    user.chars()
        .filter(|ch| ch.is_ascii_digit() || matches!(ch, '+' | '*' | '#'))
        .collect()
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn parses_callto_link() {
        let launch = parse_args(["C:\\App\\YDialer.exe".into(), "callto:+982112345678".into()]);
        assert_eq!(launch.number, "+982112345678");
        assert!(!launch.incoming);
    }

    #[test]
    fn parses_incoming_event() {
        let launch = parse_args([
            "--incoming".into(),
            "--event".into(),
            "missed".into(),
            "09120000000".into(),
            "علی".into(),
            "رضایی".into(),
        ]);
        assert!(launch.incoming);
        assert_eq!(launch.event_name, "missed");
        assert_eq!(launch.number, "09120000000");
        assert_eq!(launch.name, "علی رضایی");
    }

    #[test]
    fn strips_sip_host() {
        assert_eq!(extract_number("sip:1001@192.168.1.1"), "1001");
    }
}
