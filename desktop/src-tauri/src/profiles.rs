#[derive(Clone, Copy)]
pub struct Profile {
    pub id: &'static str,
    pub label: &'static str,
    pub note: &'static str,
    pub reject: &'static [&'static str],
    pub hangup: &'static [&'static str],
    pub hold: &'static str,
    pub resume: &'static str,
    pub conference: &'static str,
}

const T31P: Profile = Profile {
    id: "t31p",
    label: "SIP-T31P",
    note: "رد تماس ورودی با X و بعد CANCEL. قطع تماس برقرار با CALLEND و بعد ONHOOK.",
    reject: &["X", "CANCEL"],
    hangup: &["CALLEND", "ONHOOK"],
    hold: "HOLD",
    resume: "UNHOLD",
    conference: "F_CONFERENCE",
};

const T41: Profile = Profile {
    id: "t41",
    label: "SIP-T41P / T41S",
    note: "رد تماس ورودی با X و بعد CANCEL. نگه‌داشتن با HOLD و ادامه با UNHOLD.",
    reject: &["X", "CANCEL"],
    hangup: &["CALLEND", "ONHOOK"],
    hold: "HOLD",
    resume: "UNHOLD",
    conference: "F_CONFERENCE",
};

const T46U: Profile = Profile {
    id: "t46u",
    label: "SIP-T46U",
    note: "رد تماس ورودی با REJECT و بعد X. قطع تماس با CALLEND و بعد ONHOOK.",
    reject: &["REJECT", "X"],
    hangup: &["CALLEND", "ONHOOK"],
    hold: "HOLD",
    resume: "UNHOLD",
    conference: "F_CONFERENCE",
};

pub fn all() -> &'static [Profile] {
    &[T31P, T41, T46U]
}

pub fn get(id: &str) -> &'static Profile {
    all().iter().find(|profile| profile.id == id).unwrap_or(&T31P)
}
