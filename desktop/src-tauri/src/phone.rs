use std::io::{Read, Write};
use std::net::{TcpStream, ToSocketAddrs};
use std::time::Duration;

use crate::profiles;
use crate::settings::Settings;

const KEY_GAP: Duration = Duration::from_millis(180);

pub fn dial(settings: &Settings, number: &str) -> Result<(), String> {
    let number = digits(number);
    if number.is_empty() {
        return Err("شماره‌ای برای تماس نیست.".into());
    }
    let sip = settings.sip_account.trim();
    let query = if sip.is_empty() {
        format!("number={number}")
    } else {
        format!("number={number}&outgoing_uri={sip}")
    };
    send_query(settings, &query)
}

pub fn action(settings: &Settings, name: &str, extra: Option<&str>) -> Result<(), String> {
    let profile = profiles::get(&settings.phone_model);
    let keys: Vec<String> = match name {
        "answer" => vec!["ANSWER".into()],
        "reject" => owned(profile.reject),
        "hangup" => owned(profile.hangup),
        "hold" => vec![profile.hold.into()],
        "unhold" => vec![profile.resume.into()],
        "mute" => vec!["MUTE".into()],
        "transfer" => {
            let target = digits(extra.unwrap_or(""));
            if target.is_empty() {
                return Err("شماره مقصد انتقال لازم است.".into());
            }
            vec![format!("BTrans={target}")]
        }
        "conference" => vec![profile.conference.into()],
        "speaker" => vec!["SPEAKER".into()],
        "headset" => vec!["HEADSET".into()],
        "dnd_on" => vec!["DNDOn".into()],
        "dnd_off" => vec!["DNDOff".into()],
        "redial" => vec!["RD".into()],
        _ => return Err("عملیات تلفن شناخته نشد.".into()),
    };
    send_keys(settings, &keys)
}

pub fn upload_action_cfg(settings: &Settings, cfg: &str) -> Result<String, String> {
    if settings.phone_ip.trim().is_empty() {
        return Err("آی‌پی تلفن لازم است.".into());
    }
    if cfg.trim().is_empty() {
        return Err("فایل رویداد تلفن خالی بود.".into());
    }
    let (host, port) = endpoint(settings)?;
    let user = settings.web_username.as_str();
    let password = settings.web_password.as_str();
    if let Ok(cookie) = login_cookie(&host, port, user, password) {
        let upload = upload_cfg(&host, port, user, password, &cookie, cfg)?;
        return Ok(redact(&format!("login=ok {upload}"), password));
    }
    match login_https(&host, user, password) {
        Ok(cookie) => {
            let upload = upload_https(&host, user, password, &cookie, cfg)?;
            Ok(redact(&format!("login=https {upload}"), password))
        }
        Err(err) => Err(format!(
            "ورود به وب تلفن نشد ({err}). در تلفن، فیلد Incoming call را با آدرسی که برنامه نشان می‌دهد عوض کنید."
        )),
    }
}

fn send_keys(settings: &Settings, keys: &[String]) -> Result<(), String> {
    for (index, key) in keys.iter().enumerate() {
        if index > 0 {
            std::thread::sleep(KEY_GAP);
        }
        send_query(settings, key)?;
    }
    Ok(())
}

fn send_query(settings: &Settings, query: &str) -> Result<(), String> {
    if query.chars().any(|ch| ch.is_control() || ch == ' ') {
        return Err("کلید تلفن معتبر نیست.".into());
    }
    let (host, port) = endpoint(settings)?;
    if settings.use_ssl {
        return send_https(&host, port, query, &settings.web_username, &settings.web_password);
    }
    let path = format!("/servlet?key={query}");
    let response = http_exchange(
        &host,
        port,
        "GET",
        &path,
        "",
        &[],
        &settings.web_username,
        &settings.web_password,
        "",
    )?;
    if response.timed_out || response.status == 0 || (200..300).contains(&response.status) || response.status == 401 {
        if response.status == 401 {
            return Err("نام کاربری یا رمز وب تلفن پذیرفته نشد.".into());
        }
        return Ok(());
    }
    Err(format!("تلفن پاسخ {} داد.", response.status))
}

fn send_https(host: &str, port: u16, query: &str, user: &str, password: &str) -> Result<(), String> {
    let client = reqwest::blocking::Client::builder()
        .timeout(Duration::from_secs(4))
        .danger_accept_invalid_certs(true)
        .build()
        .map_err(|err| err.to_string())?;
    let url = format!("https://{host}:{port}/servlet?key={query}");
    match client
        .get(url)
        .basic_auth(user, Some(password))
        .header("user-agent", "YealinkDialer")
        .send()
    {
        Ok(response) if response.status().as_u16() == 401 => Err("نام کاربری یا رمز وب تلفن پذیرفته نشد.".into()),
        Ok(_) => Ok(()),
        Err(err) if err.is_timeout() => Ok(()),
        Err(err) => Err(format!("اتصال به تلفن برقرار نشد: {err}")),
    }
}

fn login_https(host: &str, user: &str, password: &str) -> Result<String, String> {
    let client = reqwest::blocking::Client::builder()
        .timeout(Duration::from_secs(8))
        .connect_timeout(Duration::from_secs(4))
        .danger_accept_invalid_certs(true)
        .build()
        .map_err(|err| err.to_string())?;
    let url = format!("https://{host}/servlet?m=mod_listener&p=login&q=login");
    let body = format!("username={}&pwd={}", form_escape(user), form_escape(password));
    let response = client
        .post(url)
        .header("content-type", "application/x-www-form-urlencoded")
        .header("user-agent", "YealinkDialer")
        .body(body)
        .send()
        .map_err(|err| format!("https: {err}"))?;
    let cookie = response
        .headers()
        .get_all("set-cookie")
        .iter()
        .filter_map(|value| value.to_str().ok())
        .filter_map(|value| value.split(';').next())
        .map(str::trim)
        .filter(|value| !value.is_empty())
        .collect::<Vec<_>>()
        .join("; ");
    if cookie.is_empty() {
        let text = response.text().unwrap_or_default();
        return Err(brief_phone_error(&text));
    }
    Ok(cookie)
}

fn upload_https(host: &str, user: &str, password: &str, cookie: &str, cfg: &str) -> Result<String, String> {
    let client = reqwest::blocking::Client::builder()
        .timeout(Duration::from_secs(8))
        .danger_accept_invalid_certs(true)
        .build()
        .map_err(|err| err.to_string())?;
    let url = format!("https://{host}/servlet?m=mod_data&p=settings-upload");
    let response = client
        .post(url)
        .basic_auth(user, Some(password))
        .header("cookie", cookie)
        .header("content-type", "application/octet-stream")
        .header("user-agent", "YealinkDialer")
        .body(cfg.to_string())
        .send()
        .map_err(|err| format!("آپلود https ناموفق بود: {err}"))?;
    let status = response.status().as_u16();
    let text = response.text().unwrap_or_default();
    if (200..300).contains(&status) && !phone_rejected(&text) {
        return Ok(format!("آپلود شد ({status})"));
    }
    Err(format!("آپلود تنظیمات تلفن ناموفق بود ({status}) {}", brief_phone_error(&text)))
}

fn login_cookie(host: &str, port: u16, user: &str, password: &str) -> Result<String, String> {
    let body = format!("username={}&pwd={}", form_escape(user), form_escape(password));
    let response = http_exchange(
        host,
        port,
        "POST",
        "/servlet?m=mod_listener&p=login&q=login",
        "application/x-www-form-urlencoded",
        body.as_bytes(),
        "",
        "",
        "",
    )?;
    if response.cookie.is_empty() {
        return Err(format!("بدون کوکی {}", response.status));
    }
    Ok(response.cookie)
}

fn upload_cfg(host: &str, port: u16, user: &str, password: &str, cookie: &str, cfg: &str) -> Result<String, String> {
    let plain = http_exchange(
        host,
        port,
        "POST",
        "/servlet?m=mod_data&p=settings-upload",
        "application/octet-stream",
        cfg.as_bytes(),
        user,
        password,
        cookie,
    );
    if let Ok(response) = &plain {
        if (200..300).contains(&response.status) && !phone_rejected(&response.body) {
            return Ok(format!("آپلود شد ({})", response.status));
        }
    }
    let boundary = "----yealinkcfg";
    let body = format!(
        "--{boundary}\r\nContent-Disposition: form-data; name=\"file\"; filename=\"action.cfg\"\r\nContent-Type: text/plain\r\n\r\n{cfg}\r\n--{boundary}--\r\n"
    );
    let multi = http_exchange(
        host,
        port,
        "POST",
        "/servlet?m=mod_data&p=settings-upload",
        &format!("multipart/form-data; boundary={boundary}"),
        body.as_bytes(),
        user,
        password,
        cookie,
    );
    match multi {
        Ok(response) if (200..300).contains(&response.status) && !phone_rejected(&response.body) => {
            Ok(format!("آپلود شد ({})", response.status))
        }
        Ok(response) => Err(format!("آپلود تنظیمات تلفن ناموفق بود ({})", response.status)),
        Err(err) => Err(err),
    }
}

struct HttpResponse {
    status: u16,
    cookie: String,
    body: String,
    timed_out: bool,
}

fn phone_rejected(body: &str) -> bool {
    let lower = body.to_ascii_lowercase();
    lower.contains("authstatus") && lower.contains("error") || lower.contains("\"errorno\"")
}

fn brief_phone_error(body: &str) -> String {
    let compact = body.split_whitespace().collect::<Vec<_>>().join(" ");
    let compact = if compact.chars().count() > 80 {
        compact.chars().take(80).collect()
    } else {
        compact
    };
    if compact.is_empty() { "بدون توضیح".into() } else { compact }
}

fn http_exchange(
    host: &str,
    port: u16,
    method: &str,
    path: &str,
    content_type: &str,
    payload: &[u8],
    user: &str,
    password: &str,
    cookie: &str,
) -> Result<HttpResponse, String> {
    let address = format!("{host}:{port}");
    let mut socket = address
        .to_socket_addrs()
        .map_err(|_| "آدرس تلفن معتبر نیست.".to_string())?
        .next()
        .ok_or("آدرس تلفن معتبر نیست.".to_string())?;
    socket.set_port(port);
    let mut stream = TcpStream::connect_timeout(&socket, Duration::from_secs(4))
        .map_err(|err| format!("اتصال به تلفن برقرار نشد: {err}"))?;
    stream.set_read_timeout(Some(Duration::from_secs(4))).ok();
    stream.set_write_timeout(Some(Duration::from_secs(4))).ok();
    let mut request = format!("{method} {path} HTTP/1.0\r\nHost: {host}\r\nUser-Agent: YealinkDialer\r\n");
    if !user.is_empty() {
        request.push_str(&format!("Authorization: Basic {}\r\n", basic_token(user, password)));
    }
    if !cookie.is_empty() {
        request.push_str(&format!("Cookie: {cookie}\r\n"));
    }
    if !content_type.is_empty() {
        request.push_str(&format!("Content-Type: {content_type}\r\nContent-Length: {}\r\n", payload.len()));
    }
    request.push_str("Connection: close\r\n\r\n");
    stream
        .write_all(request.as_bytes())
        .and_then(|_| stream.write_all(payload))
        .map_err(|err| format!("ارسال به تلفن ناموفق بود: {err}"))?;
    let mut buffer = Vec::new();
    let mut timed_out = false;
    match stream.read_to_end(&mut buffer) {
        Ok(_) => {}
        Err(err) if err.kind() == std::io::ErrorKind::TimedOut || err.kind() == std::io::ErrorKind::WouldBlock => {
            timed_out = true;
        }
        Err(err) => return Err(format!("پاسخ تلفن خوانده نشد: {err}")),
    }
    let raw = String::from_utf8_lossy(&buffer);
    let (head, body) = raw.split_once("\r\n\r\n").unwrap_or((raw.as_ref(), ""));
    let status = head
        .lines()
        .next()
        .and_then(|line| line.split_whitespace().nth(1))
        .and_then(|code| code.parse().ok())
        .unwrap_or(0);
    Ok(HttpResponse {
        status,
        cookie: cookies(head),
        body: body.to_string(),
        timed_out,
    })
}

fn cookies(head: &str) -> String {
    let mut jar = Vec::new();
    for line in head.lines() {
        let Some((name, value)) = line.split_once(':') else {
            continue;
        };
        if name.eq_ignore_ascii_case("set-cookie") {
            let value = value.trim().split(';').next().unwrap_or("").trim();
            if !value.is_empty() {
                jar.push(value.to_string());
            }
        }
    }
    jar.join("; ")
}

fn endpoint(settings: &Settings) -> Result<(String, u16), String> {
    let raw = settings.phone_ip.trim();
    if raw.is_empty() {
        return Err("آی‌پی تلفن را در تنظیمات وارد کنید.".into());
    }
    let default_port = if settings.use_ssl { 443 } else { 80 };
    if let Some((host, port)) = raw.rsplit_once(':') {
        if !host.is_empty() && !host.contains(':') {
            if let Ok(port) = port.parse::<u16>() {
                return Ok((host.to_string(), port));
            }
        }
    }
    Ok((raw.to_string(), default_port))
}

fn digits(raw: &str) -> String {
    raw.chars()
        .filter(|ch| ch.is_ascii_digit() || matches!(ch, '+' | '*' | '#'))
        .collect()
}

fn owned(keys: &[&str]) -> Vec<String> {
    keys.iter().map(|key| (*key).to_string()).collect()
}

fn basic_token(user: &str, password: &str) -> String {
    use base64::{engine::general_purpose::STANDARD, Engine as _};
    let token = format!("{user}:{password}");
    let bytes: Vec<u8> = token.chars().map(|ch| if (ch as u32) <= 255 { ch as u8 } else { b'?' }).collect();
    STANDARD.encode(bytes)
}

fn form_escape(value: &str) -> String {
    let mut out = String::new();
    for byte in value.bytes() {
        match byte {
            b'A'..=b'Z' | b'a'..=b'z' | b'0'..=b'9' | b'-' | b'_' | b'.' | b'~' => out.push(byte as char),
            _ => out.push_str(&format!("%{byte:02X}")),
        }
    }
    out
}

fn redact(text: &str, secret: &str) -> String {
    if secret.is_empty() {
        text.to_string()
    } else {
        text.replace(secret, "***")
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn splits_host_and_port() {
        let settings = Settings {
            phone_ip: "10.1.1.5:8080".into(),
            ..Settings::default()
        };
        assert_eq!(endpoint(&settings).unwrap(), ("10.1.1.5".into(), 8080));
    }
}
