# YDialer

شماره‌گیر ویندوز برای تلفن‌های SIP یالینک، به‌همراه سرور دفترچه. تماس بین کامپیوتر کاربر و تلفن همان شبکه برقرار می‌شود. سرور دفترچه مخاطبین و فایل آپدیت را می‌دهد.

| بخش | مسیر |
| --- | --- |
| سرور دفترچه و پنل | `phonebook/` |
| برنامه دسکتاپ | `desktop/` |

مدل‌ها: SIP-T31P، SIP-T41P / T41S، SIP-T46U.

## ۱. سرور دفترچه

### پیش‌نیاز

- Node.js نسخه ۲۰ یا جدیدتر
- یک دامنه با HTTPS جلوی سرور (مثلاً Caddy)
- پورت برنامه پیش‌فرض `8080` است

### نصب

```bash
cd phonebook
npm install
npm run build
```

`npm run build` پنل را در `phonebook/public` می‌سازد. سرور همین پوشه را سرو می‌کند.

### تنظیم محیط

این متغیرها را قبل از اجرا بگذارید:

| متغیر | معنی |
| --- | --- |
| `PORT` | پورت شنود. پیش‌فرض `8080` |
| `HOST` | آدرس شنود. پیش‌فرض `0.0.0.0` |
| `PUBLIC_BASE_URL` | آدرس عمومی HTTPS، بدون اسلش آخر. مثال: `https://panel.example.com` |
| `ADMIN_USER` | نام کاربر پنل. پیش‌فرض `admin` |
| `ADMIN_PASSWORD` | رمز پنل. خالی بودن آن پنل را بدون رمز باز می‌گذارد |

نمونه در PowerShell:

```powershell
$env:PORT = "8080"
$env:PUBLIC_BASE_URL = "https://panel.example.com"
$env:ADMIN_USER = "admin"
$env:ADMIN_PASSWORD = "یک-رمز-قوی"
npm start
```

با بالا آمدن سرور این پوشه‌ها ساخته می‌شوند:

- `phonebook/data` برای مخاطبین و تنظیمات
- `phonebook/data/releases` برای فایل نصب برنامه دسکتاپ

این پوشه را روی دیسک ماندگار نگه دارید. پاک شدنش دفترچه و فایل‌های آپدیت را از بین می‌برد.

### آدرس‌های عمومی

این مسیرها رمز پنل نمی‌خواهند:

- `https://panel.example.com/phonebook.xml` دفترچه برای تلفن
- `https://panel.example.com/updates/latest.json` اعلام نسخه دسکتاپ
- `https://panel.example.com/updates/files/<نام>` فقط فایل `.exe` و `.sig`

بقیه پنل با `ADMIN_PASSWORD` از Basic Auth محافظت می‌شود.

اگر `latest.json` نباشد، سرور برای آن آدرس پاسخ خالی ۲۰۴ می‌دهد و برنامه دسکتاپ پیامی نشان نمی‌دهد.

### HTTPS

برنامه Node فقط HTTP سرو می‌کند. Caddy (یا پروکسی مشابه) دامنه را به `127.0.0.1:8080` می‌فرستد و گواهی HTTPS را خودش می‌گذارد. `PUBLIC_BASE_URL` همان دامنه HTTPS است.

آدرس دفترچه داخل برنامه دسکتاپ هم با `https://` شروع می‌شود. با `http` بررسی آپدیت انجام نمی‌شود.

### داکر

در این مخزن Dockerfile نیست. برای اجرا داخل کانتینر همان دستورهای بالا را در تصویر بزنید و `phonebook/data` را به یک volume وصل کنید. `PUBLIC_BASE_URL` و `ADMIN_PASSWORD` را به‌صورت متغیر محیط به کانتینر بدهید.

### تلفن

در پنل مخاطب بسازید. روی تلفن بروید به Directory سپس Remote Phone Book و آدرس XML را بگذارید:

`https://panel.example.com/phonebook.xml`

برای هر گروه، آدرس همان گروه را از پنل کپی کنید.

## ۲. برنامه دسکتاپ

ساخت نصب‌کننده روی ویندوز است. خروجی یک فایل نصب برای کاربر جاری است: میانبر منوی شروع، حذف از تنظیمات ویندوز، و در صورت نبودن WebView2 نصب خاموش آن.

### پیش‌نیاز ساخت

- Node.js نسخه ۲۰ یا جدیدتر
- Rust پایدار
- ابزار ساخت ویندوز (Visual Studio Build Tools با workload مربوط به C++)
- کلید خصوصی امضا در `%USERPROFILE%\.tauri\ydialer.key` و رمزش در `%USERPROFILE%\.tauri\ydialer.key.password`

کلید عمومی از قبل داخل `desktop/src-tauri/tauri.conf.json` است. برای نسخه‌هایی که قبلاً نصب شده‌اند از همان جفت کلید استفاده کنید. کلید تازه باعث می‌شود آن نسخه‌ها آپدیت بعدی را رد کنند. کلید خصوصی را وارد گیت نکنید.

### اجرای توسعه

```powershell
cd desktop
npm install
npm run tauri dev
```

### ساخت نصب‌کننده

شماره نسخه در `desktop/src-tauri/tauri.conf.json`، فیلد `version` است. قبل از هر انتشار آن را بالا ببرید.

```powershell
cd desktop
$env:TAURI_SIGNING_PRIVATE_KEY = "$env:USERPROFILE\.tauri\ydialer.key"
$env:TAURI_SIGNING_PRIVATE_KEY_PASSWORD = Get-Content "$env:USERPROFILE\.tauri\ydialer.key.password"
npm run tauri build
```

در پایان ساخت، مسیر `YDialer_<نسخه>_x64-setup.exe` چاپ می‌شود. فایل `.sig` کنار آن است. هر دو معمولاً این‌جا هستند:

`desktop/src-tauri/target/release/bundle/nsis/`

### نصب روی کامپیوتر کاربر

فایل `YDialer_<نسخه>_x64-setup.exe` را اجرا کنید. نصب برای همان کاربر ویندوز است.

بعد از نصب:

1. مدل تلفن، آی‌پی تلفن، نام کاربری و رمز وب تلفن را وارد کنید.
2. آدرس دفترچه را با HTTPS بگذارید، مثلاً `https://panel.example.com/phonebook.xml`.
3. دکمه ثبت رویدادها را بزنید تا آدرس زنگ روی تلفن نوشته شود.

بستن پنجره برنامه را خارج نمی‌کند. خروج از آیکن کنار ساعت است. گزینه «با ورود به ویندوز اجرا شود» در تنظیمات است.

زنگ و شماره‌گیری از سرور رد نمی‌شود. تلفن باید آی‌پی کامپیوتر همان کاربر را در شبکه محلی ببیند. در Action URL تلفن آدرس `127.0.0.1` نگذارید.

## ۳. منتشر کردن نسخه جدید

پیام آپدیت وقتی به کاربر می‌رسد که فایل نصب و `latest.json` روی سرور دفترچه باشند. push به گیت‌هاب این پیام را نمی‌سازد.

1. نسخه را در `desktop/src-tauri/tauri.conf.json` بالا ببرید.
2. نصب‌کننده امضاشده را با دستور بخش قبل بسازید.
3. فایل `.exe` و `.sig` را در `phonebook/data/releases` روی سرور بگذارید.
4. `phonebook/data/latest.json` را بنویسید. مقدار `signature` متن کامل فایل `.sig` است:

```json
{
  "version": "1.0.2",
  "notes": "توضیح کوتاه",
  "pub_date": "2026-10-08T12:00:00Z",
  "platforms": {
    "windows-x86_64": {
      "signature": "محتوای کامل فایل sig",
      "url": "https://panel.example.com/updates/files/YDialer_1.0.2_x64-setup.exe"
    }
  }
}
```

5. سرور دفترچه را در حال اجرا نگه دارید. فایل‌ها از همان پوشه داده خوانده می‌شوند و نیاز به ساخت دوباره برنامه سرور ندارند.

دفعه بعد که برنامه کاربر باز شود، اگر نسخه `latest.json` بزرگ‌تر باشد و امضا درست باشد، بنر «نسخه … آماده نصب است» نشان داده می‌شود. در تنظیمات هم دکمه «بررسی به‌روزرسانی» همین بررسی را دستی انجام می‌دهد.

## ۴. چه چیزی در گیت می‌ماند

در گیت: کد سرور، کد دسکتاپ، و کلید عمومی داخل `tauri.conf.json`.

بیرون از گیت: `node_modules`، خروجی ساخت، `.env`، کلید خصوصی، `phonebook/data/latest.json`، فایل‌های `phonebook/data/releases`، و دفترچه واقعی مشتریان روی سرور.
