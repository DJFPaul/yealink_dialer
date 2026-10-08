# YDialer

شماره‌گیر دسکتاپ یالینک با Tauri. تماس‌ها با Action URI خود تلفن گرفته می‌شود و مخاطبین از آدرس سرور شما خوانده می‌شود.

مدل‌ها: SIP-T31P، SIP-T41P / T41S، و SIP-T46U. مدل موقع اولین اجرا انتخاب می‌شود و بعداً از تنظیمات عوض می‌شود.

## اجرا

```
cd desktop
npm install
npm run tauri dev
```

نصب‌کننده ویندوز:

```
$env:TAURI_SIGNING_PRIVATE_KEY = "$env:USERPROFILE\.tauri\ydialer.key"
$env:TAURI_SIGNING_PRIVATE_KEY_PASSWORD = Get-Content "$env:USERPROFILE\.tauri\ydialer.key.password"
npm run tauri build
```

فایل نصبی `YDialer_1.0.0_x64-setup.exe` است و مسیرش در پایان ساخت چاپ می‌شود. نصب برای کاربر جاری است، میانبر منوی شروع و حذف از تنظیمات ویندوز دارد، و اگر WebView2 نباشد خودش آن را می‌گذارد.

بستن پنجره برنامه را خارج نمی‌کند؛ از آیکن کنار ساعت خارج شوید. گزینه «با ورود به ویندوز اجرا شود» برنامه را در استارتاپ کاربر ثبت می‌کند.

## به‌روزرسانی

کلید خصوصی امضا در `%USERPROFILE%\.tauri\ydialer.key` است. اگر این فایل یا رمزش گم شود، نسخه‌های نصب‌شده دیگر آپدیت را قبول نمی‌کنند.

برای هر نسخه جدید، شماره را در `src-tauri/tauri.conf.json` بالا ببرید و دوباره همان ساخت را بزنید. کنار نصب‌کننده یک فایل `.sig` ساخته می‌شود. هر دو را روی سرور دفترچه بگذارید:

- فایل نصب و امضا داخل `phonebook/data/releases`
- `phonebook/data/latest.json` با این شکل، و آدرس https همان سرور:

```json
{
  "version": "1.0.1",
  "notes": "توضیح کوتاه",
  "pub_date": "2026-10-08T12:00:00Z",
  "platforms": {
    "windows-x86_64": {
      "signature": "محتوای کامل فایل sig",
      "url": "https://panel.example.com/updates/files/YDialer_1.0.1_x64-setup.exe"
    }
  }
}
```

برنامه این فایل را از `https://همان-سرور-دفترچه/updates/latest.json` می‌خواند. آدرس دفترچه در تنظیمات باید https باشد. اگر نسخه جدیدی نباشد، سرور پاسخ خالی ۲۰۴ می‌دهد.
