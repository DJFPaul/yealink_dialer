use std::io::{Read, Write};
use std::net::{IpAddr, SocketAddr, TcpListener, TcpStream, UdpSocket};
use std::sync::Arc;
use std::time::Duration;

use crate::launch::{extract_number, Launch};

const PORTS: &[u16] = &[18080, 18180, 18280, 28080];

pub fn start(on_event: impl Fn(SocketAddr, Launch) + Send + Sync + 'static) -> Result<u16, String> {
    let mut last = "پورت شنونده تماس باز نشد.".to_string();
    for port in PORTS {
        match TcpListener::bind(("0.0.0.0", *port)) {
            Ok(listener) => {
                allow_firewall(*port);
                let on_event = Arc::new(on_event);
                std::thread::Builder::new()
                    .name("yealink-callback".into())
                    .spawn(move || serve(listener, on_event))
                    .map_err(|err| err.to_string())?;
                return Ok(*port);
            }
            Err(err) => last = format!("پورت {port} باز نشد: {err}"),
        }
    }
    Err(last)
}

pub fn local_ip_toward(phone_ip: &str) -> Option<String> {
    let host = phone_ip.trim().split(':').next()?.trim();
    if host.is_empty() {
        return None;
    }
    let socket = UdpSocket::bind("0.0.0.0:0").ok()?;
    socket.connect((host, 9)).ok()?;
    match socket.local_addr().ok()?.ip() {
        IpAddr::V4(ip) if !ip.is_loopback() && !ip.is_unspecified() => Some(ip.to_string()),
        _ => None,
    }
}

pub fn accepts(phone_ip: &str, peer: IpAddr) -> bool {
    if peer.is_loopback() {
        return true;
    }
    let host = phone_ip.trim().split(':').next().unwrap_or("").trim();
    !host.is_empty() && peer.to_string() == host
}

pub fn event_url(ip: &str, port: u16, event: &str) -> String {
    format!("http://{ip}:{port}/yealink?event={event}&number=$remote&name=$display_remote")
}

pub fn action_cfg(ip: &str, port: u16) -> String {
    let line = |key: &str, event: &str| format!("{key} = {}", event_url(ip, port, event));
    [
        "#!version:1.0.0.1".to_string(),
        String::new(),
        line("action_url.incoming_call", "incoming"),
        line("action_url.outgoing_call", "outgoing"),
        line("action_url.call_established", "established"),
        line("action_url.call_terminated", "terminated"),
        line("action_url.missed_call", "missed"),
        line("action_url.reject_incoming_call", "rejected"),
        line("action_url.hold", "hold"),
        line("action_url.unhold", "unhold"),
        line("action_url.mute", "mute"),
        line("action_url.unmute", "unmute"),
        line("action_url.dnd_on", "dnd-on"),
        line("action_url.dnd_off", "dnd-off"),
        String::new(),
    ]
    .join("\r\n")
}

pub fn parse_http(data: &[u8]) -> Option<Launch> {
    let text = String::from_utf8_lossy(data);
    let line = text.lines().next()?.trim();
    let rest = line
        .strip_prefix("GET ")
        .or_else(|| line.strip_prefix("get "))?;
    let target = rest.rsplit_once(" HTTP/").map(|(target, _)| target).unwrap_or(rest).trim();
    parse_target(target)
}

fn serve(listener: TcpListener, on_event: Arc<dyn Fn(SocketAddr, Launch) + Send + Sync>) {
    for incoming in listener.incoming() {
        let Ok(stream) = incoming else {
            continue;
        };
        let on_event = Arc::clone(&on_event);
        std::thread::spawn(move || respond(stream, &on_event));
    }
}

fn respond(mut stream: TcpStream, on_event: &Arc<dyn Fn(SocketAddr, Launch) + Send + Sync>) {
    let _ = stream.set_read_timeout(Some(Duration::from_secs(2)));
    let peer = stream.peer_addr().ok();
    let mut data = Vec::new();
    let mut buf = [0u8; 1024];
    loop {
        match stream.read(&mut buf) {
            Ok(0) => break,
            Ok(n) => {
                data.extend_from_slice(&buf[..n]);
                if data.windows(4).any(|window| window == b"\r\n\r\n") || data.len() >= 8192 {
                    break;
                }
            }
            Err(_) => break,
        }
    }
    if let (Some(peer), Some(launch)) = (peer, parse_http(&data)) {
        on_event(peer, launch);
    }
    let body = b"OK";
    let head = format!(
        "HTTP/1.0 200 OK\r\nContent-Type: text/plain; charset=utf-8\r\nContent-Length: {}\r\nConnection: close\r\n\r\n",
        body.len()
    );
    let _ = stream.write_all(head.as_bytes());
    let _ = stream.write_all(body);
}

fn parse_target(target: &str) -> Option<Launch> {
    let (path, query) = target.split_once('?').unwrap_or((target, ""));
    let params = form_pairs(query);
    let event = params
        .iter()
        .find(|(key, _)| key == "event" || key == "action")
        .map(|(_, value)| value.as_str())
        .unwrap_or("");
    let number = first_value(&params, &["number", "remote", "caller", "callerid", "caller_id"]);
    let name = first_value(&params, &["name", "display", "display_remote"]);
    let number = if number.contains('$') { String::new() } else { extract_number(&number) };
    let name = clean_name(&name, &number);
    let event = normalize_event(event).or_else(|| {
        if path.trim_end_matches('/').ends_with("/incoming") {
            Some("incoming")
        } else {
            None
        }
    })?;
    let mut launch = Launch::default();
    if event == "incoming" {
        launch.incoming = true;
    } else {
        launch.event_name = event.to_string();
    }
    launch.number = number;
    launch.name = name;
    Some(launch)
}

fn first_value(params: &[(String, String)], keys: &[&str]) -> String {
    params
        .iter()
        .find(|(key, _)| keys.iter().any(|candidate| key == candidate))
        .map(|(_, value)| value.clone())
        .unwrap_or_default()
}

fn normalize_event(raw: &str) -> Option<&'static str> {
    match raw.trim().to_ascii_lowercase().replace('_', "-").as_str() {
        "incoming" | "incoming-call" => Some("incoming"),
        "outgoing" | "outgoing-call" => Some("outgoing"),
        "established" | "call-established" => Some("established"),
        "terminated" | "call-terminated" | "terminate" => Some("terminated"),
        "missed" | "missed-call" => Some("missed"),
        "rejected" | "reject" | "reject-incoming-call" => Some("rejected"),
        "hold" | "held" => Some("hold"),
        "unhold" | "unheld" | "resume" => Some("unhold"),
        "mute" => Some("mute"),
        "unmute" => Some("unmute"),
        "dnd-on" | "open-dnd" => Some("dnd-on"),
        "dnd-off" | "close-dnd" => Some("dnd-off"),
        _ => None,
    }
}

fn clean_name(name: &str, number: &str) -> String {
    let name = name.trim().trim_matches('"').trim();
    if name.is_empty() || name.contains('$') {
        return String::new();
    }
    let as_number = extract_number(name);
    if !number.is_empty() && (name == number || as_number == number) {
        return String::new();
    }
    name.to_string()
}

fn form_pairs(query: &str) -> Vec<(String, String)> {
    query
        .split('&')
        .filter(|part| !part.is_empty())
        .map(|part| {
            let (key, value) = part.split_once('=').unwrap_or((part, ""));
            (form_decode(key).to_ascii_lowercase(), form_decode(value))
        })
        .collect()
}

fn form_decode(raw: &str) -> String {
    let mut bytes = Vec::new();
    let input = raw.as_bytes();
    let mut index = 0;
    while index < input.len() {
        match input[index] {
            b'+' => {
                bytes.push(b' ');
                index += 1;
            }
            b'%' if index + 2 < input.len() => {
                let hex = std::str::from_utf8(&input[index + 1..index + 3]).unwrap_or("");
                if let Ok(byte) = u8::from_str_radix(hex, 16) {
                    bytes.push(byte);
                    index += 3;
                } else {
                    bytes.push(input[index]);
                    index += 1;
                }
            }
            byte => {
                bytes.push(byte);
                index += 1;
            }
        }
    }
    String::from_utf8_lossy(&bytes).into_owned()
}

fn allow_firewall(port: u16) {
    #[cfg(windows)]
    {
        let name = "YDialer callback";
        let _ = std::process::Command::new("netsh")
            .args(["advfirewall", "firewall", "delete", "rule", &format!("name={name}")])
            .output();
        let _ = std::process::Command::new("netsh")
            .args([
                "advfirewall",
                "firewall",
                "add",
                "rule",
                &format!("name={name}"),
                "dir=in",
                "action=allow",
                "protocol=TCP",
                &format!("localport={port}"),
                "profile=any",
            ])
            .output();
    }
    #[cfg(not(windows))]
    {
        let _ = port;
    }
}

#[cfg(test)]
mod tests {
    use super::*;

    #[test]
    fn reads_incoming_caller() {
        let raw = b"GET /yealink?event=incoming&number=sip:09121111111@192.168.100.97&name=%D8%B9%D9%84%DB%8C HTTP/1.1\r\nHost: pc\r\n\r\n";
        let launch = parse_http(raw).unwrap();
        assert!(launch.incoming);
        assert_eq!(launch.number, "09121111111");
        assert_eq!(launch.name, "علی");
        assert!(launch.event_name.is_empty());
    }

    #[test]
    fn drops_name_that_is_only_the_number() {
        let launch = parse_target("/yealink?event=established&number=402&name=402").unwrap();
        assert_eq!(launch.event_name, "established");
        assert_eq!(launch.number, "402");
        assert!(launch.name.is_empty());
    }

    #[test]
    fn ignores_pages_without_a_call_event() {
        assert!(parse_target("/favicon.ico").is_none());
    }

    #[test]
    fn action_urls_stay_within_yealink_limit() {
        let cfg = action_cfg("192.168.100.10", 18080);
        assert!(cfg.lines().all(|line| line.len() <= 511));
        assert!(cfg.contains("action_url.incoming_call = http://192.168.100.10:18080/yealink?event=incoming&number=$remote&name=$display_remote"));
    }

    #[test]
    fn only_the_phone_or_this_pc_can_notify() {
        assert!(accepts("192.168.100.142", "192.168.100.142".parse().unwrap()));
        assert!(accepts("192.168.100.142", "127.0.0.1".parse().unwrap()));
        assert!(!accepts("192.168.100.142", "192.168.100.50".parse().unwrap()));
    }
}
