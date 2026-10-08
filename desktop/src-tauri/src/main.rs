// Prevents additional console window on Windows in release, DO NOT REMOVE!!
#![cfg_attr(not(debug_assertions), windows_subsystem = "windows")]

fn main() {
    // A debug build is a console program. Windows startup launches it with
    // --autostart, so that console stays on screen and closing it kills the app.
    detach_startup_console();
    desktop_lib::run()
}

#[cfg(windows)]
fn detach_startup_console() {
    if !std::env::args().any(|arg| arg == "--autostart") {
        return;
    }
    unsafe {
        let mut owners = [0u32; 8];
        let attached = GetConsoleProcessList(owners.as_mut_ptr(), owners.len() as u32);
        // A release build has no console. If another process shares this console,
        // it belongs to the terminal that launched us and must stay visible.
        if attached != 1 {
            return;
        }
        let window = GetConsoleWindow();
        if !window.is_null() {
            ShowWindow(window, 0);
        }
        FreeConsole();
    }
}

#[cfg(not(windows))]
fn detach_startup_console() {}

#[cfg(windows)]
#[link(name = "kernel32")]
extern "system" {
    fn GetConsoleWindow() -> *mut core::ffi::c_void;
    fn GetConsoleProcessList(process_list: *mut u32, process_count: u32) -> u32;
    fn FreeConsole() -> i32;
}

#[cfg(windows)]
#[link(name = "user32")]
extern "system" {
    fn ShowWindow(hwnd: *mut core::ffi::c_void, cmd: i32) -> i32;
}
