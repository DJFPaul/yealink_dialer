<script lang="ts">
  import { listen } from "@tauri-apps/api/event";
  import { onMount } from "svelte";
  import {
    appVersion,
    callbackInfo,
    checkUpdate,
    dial,
    fetchContacts,
    finishLastCall,
    getSettings,
    importLegacy,
    installUpdate,
    listHistory,
    listModels,
    phoneAction,
    rememberCall,
    saveSettings,
    syncActionUrls,
    takeLaunch,
    type CallbackInfo,
    type Contact,
    type HistoryEntry,
    type Launch,
    type PhoneModel,
    type Settings,
    type UpdateNotice
  } from "$lib/api";

  const keys = ["1", "2", "3", "4", "5", "6", "7", "8", "9", "*", "0", "#"];

  let settings = $state<Settings>(emptySettings());
  let draft = $state<Settings>(emptySettings());
  let models = $state<PhoneModel[]>([]);
  let contacts = $state<Contact[]>([]);
  let history = $state<HistoryEntry[]>([]);
  let view = $state<"setup" | "dial" | "contacts" | "history" | "settings">("setup");
  let number = $state("");
  let contactName = $state("");
  let lockedName = $state(false);
  let phase = $state<"idle" | "ringing" | "calling" | "active" | "held">("idle");
  let status = $state("");
  let held = $state(false);
  let muted = $state(false);
  let dnd = $state(false);
  let busy = $state(false);
  let error = $state("");
  let notice = $state("");
  let query = $state("");
  let phonebookUrl = $state("");
  let contactsError = $state("");
  let transferOpen = $state(false);
  let transferTo = $state("");
  let legacyFolder = $state("");
  let showPassword = $state(false);
  let callback = $state<CallbackInfo | null>(null);
  let appVersionLabel = $state("");
  let update = $state<UpdateNotice | null>(null);

  let modelLabel = $derived(models.find((model) => model.id === settings.phoneModel)?.label ?? "تلفن");
  let filtered = $derived(
    contacts.filter((contact) => {
      const text = query.trim();
      if (!text) return true;
      return contact.name.includes(text) || contact.number.includes(text);
    })
  );
  let inCall = $derived(phase === "ringing" || phase === "calling" || phase === "active" || phase === "held");

  onMount(() => {
    const onKey = (event: KeyboardEvent) => {
      if (view !== "dial") return;
      const target = event.target as HTMLElement | null;
      const typing = target?.tagName === "INPUT" || target?.tagName === "TEXTAREA";
      if (event.key === "Enter") {
        event.preventDefault();
        if (phase === "ringing") answer();
        else if (!inCall) call();
        return;
      }
      if (typing) return;
      if (/^[0-9*#+]$/.test(event.key)) press(event.key);
      else if (event.key === "Backspace") backspace();
    };
    window.addEventListener("keydown", onKey);
    const timer = setInterval(() => refreshContacts(true), 5 * 60 * 1000);
    let unlisten = () => {};
    let unlistenProgress = () => {};
    (async () => {
      settings = await getSettings();
      draft = { ...settings };
      phonebookUrl = settings.contactsUrl;
      callback = await callbackInfo().catch(() => null);
      appVersionLabel = await appVersion().catch(() => "");
      models = await listModels();
      history = await listHistory();
      view = settings.setupComplete ? "dial" : "setup";
      unlisten = await listen<Launch>("launch", (event) => applyLaunch(event.payload));
      unlistenProgress = await listen<{ received: number; total: number | null }>("update-progress", (event) => {
        const total = event.payload.total;
        notice = total
          ? `در حال دریافت نسخه جدید، ${Math.min(100, Math.round((event.payload.received / total) * 100))} درصد`
          : "در حال دریافت نسخه جدید...";
      });
      applyLaunch(await takeLaunch());
      refreshContacts(true);
      update = await checkUpdate(true).catch(() => null);
    })().catch((reason) => {
      error = explain(reason);
    });
    return () => {
      window.removeEventListener("keydown", onKey);
      clearInterval(timer);
      unlisten();
      unlistenProgress();
    };
  });

  function emptySettings(): Settings {
    return {
      phoneIp: "",
      sipAccount: "",
      webUsername: "admin",
      webPassword: "",
      useSsl: false,
      phoneModel: "t31p",
      contactsUrl: "",
      actionCfgUrl: "http://127.0.0.1:8080/api/yealink-action.cfg",
      autostart: true,
      setupComplete: false
    };
  }

  function explain(reason: unknown) {
    if (typeof reason === "string") return reason;
    if (reason instanceof Error) return reason.message;
    return "خطای ناشناخته";
  }

  function matchContact(value: string) {
    const digits = value.replace(/[^\d]/g, "");
    if (digits.length < 3) return "";
    const exact = contacts.find((contact) => contact.number.replace(/[^\d]/g, "") === digits);
    if (exact) return exact.name;
    const tail = contacts.find((contact) => {
      const other = contact.number.replace(/[^\d]/g, "");
      return other.length >= 3 && (digits.endsWith(other) || other.endsWith(digits));
    });
    return tail?.name ?? "";
  }

  function press(key: string) {
    lockedName = false;
    number += key;
    contactName = matchContact(number);
  }

  function backspace() {
    lockedName = false;
    number = number.slice(0, -1);
    contactName = matchContact(number);
  }

  function onNumberInput() {
    lockedName = false;
    contactName = matchContact(number);
  }

  function applyLaunch(launch: Launch) {
    if (!launch.number && !launch.eventName && !launch.incoming) return;
    if (!settings.setupComplete) {
      if (launch.number) number = launch.number;
      return;
    }
    view = "dial";
    if (launch.eventName) {
      applyEvent(launch.eventName, launch.number, launch.name);
      return;
    }
    if (launch.number) number = launch.number;
    if (launch.name) {
      contactName = launch.name;
      lockedName = true;
    } else {
      lockedName = false;
      contactName = matchContact(number);
    }
    if (launch.incoming) {
      phase = "ringing";
      status = "تماس ورودی";
      if (!contactName) contactName = "تماس ورودی";
      record("ورودی", "زنگ");
    } else if (launch.number) {
      call();
    }
  }

  async function applyEvent(eventName: string, eventNumber: string, eventContact: string) {
    if (eventNumber) number = eventNumber;
    if (eventContact) {
      contactName = eventContact;
      lockedName = true;
    } else if (eventNumber) {
      lockedName = false;
      contactName = matchContact(eventNumber);
    }
    view = "dial";
    const wasRinging = phase === "ringing";
    if (eventName === "established") {
      phase = "active";
      held = false;
      status = "تماس برقرار شد";
      await finishOrRemember("برقرار", wasRinging ? "ورودی" : "خروجی");
    } else if (eventName === "outgoing") {
      phase = "calling";
      status = "تماس خروجی";
      await finishOrRemember("در حال تماس", "خروجی");
    } else if (eventName === "terminated") {
      phase = "idle";
      held = false;
      muted = false;
      if (wasRinging) {
        status = "تماس بی‌پاسخ";
        if (!contactName) contactName = "تماس بی‌پاسخ";
        await finishOrRemember("بی‌پاسخ", "ورودی");
      } else {
        status = "قطع شد";
        await finishOrRemember("قطع شد", "خروجی");
      }
    } else if (eventName === "missed" || eventName === "rejected") {
      phase = "idle";
      status = eventName === "rejected" ? "رد شد" : "تماس بی‌پاسخ";
      if (!contactName) contactName = status;
      await finishOrRemember(eventName === "rejected" ? "رد شده" : "بی‌پاسخ", "ورودی");
    } else if (eventName === "hold") {
      held = true;
      phase = "held";
      status = "تماس در انتظار";
    } else if (eventName === "unhold") {
      held = false;
      phase = "active";
      status = "تماس برقرار شد";
    } else if (eventName === "mute") {
      muted = true;
      status = "بی‌صدا";
    } else if (eventName === "unmute") {
      muted = false;
      status = "صدا وصل شد";
    } else if (eventName === "dnd-on") {
      dnd = true;
      status = "مزاحم نشوید روشن شد";
    } else if (eventName === "dnd-off") {
      dnd = false;
      status = "مزاحم نشوید خاموش شد";
    }
  }

  async function call() {
    if (!number || busy || phase === "ringing") return;
    busy = true;
    error = "";
    phase = "calling";
    status = "در حال تماس";
    try {
      await dial(number);
      phase = "active";
      await record("خروجی", "در حال تماس");
    } catch (reason) {
      phase = "idle";
      status = "";
      error = explain(reason);
    } finally {
      busy = false;
    }
  }

  async function answer() {
    await run("answer");
    if (error) return;
    phase = "active";
    status = "تماس برقرار شد";
    await finishOrRemember("پاسخ داده", "ورودی");
  }

  async function reject() {
    await run("reject");
    if (error) return;
    phase = "idle";
    status = "رد شد";
    await finishOrRemember("رد شده", "ورودی");
  }

  async function hangup() {
    await run("hangup");
    if (error) return;
    phase = "idle";
    held = false;
    muted = false;
    status = "قطع شد";
    await finishOrRemember("قطع شد", "خروجی");
  }

  async function toggleHold() {
    await run(held ? "unhold" : "hold");
    if (error) return;
    held = !held;
    phase = held ? "held" : "active";
    status = held ? "تماس در انتظار" : "تماس برقرار شد";
  }

  async function toggleMute() {
    await run("mute");
    if (error) return;
    muted = !muted;
    status = muted ? "بی‌صدا" : "صدا وصل شد";
  }

  async function toggleDnd() {
    await run(dnd ? "dnd_off" : "dnd_on");
    if (error) return;
    dnd = !dnd;
    status = dnd ? "مزاحم نشوید روشن شد" : "مزاحم نشوید خاموش شد";
  }

  async function transfer() {
    await run("transfer", transferTo);
    if (error) return;
    status = `در حال انتقال به ${transferTo}`;
    transferOpen = false;
    transferTo = "";
  }

  async function run(action: string, extra?: string) {
    busy = true;
    error = "";
    try {
      await phoneAction(action, extra);
    } catch (reason) {
      error = explain(reason);
    } finally {
      busy = false;
    }
  }

  async function record(direction: string, result: string) {
    try {
      await rememberCall(direction, number, contactName, result);
      history = await listHistory();
    } catch (reason) {
      error = explain(reason);
    }
  }

  async function finishOrRemember(result: string, direction: string) {
    try {
      const updated = await finishLastCall(result);
      if (!updated) await rememberCall(direction, number, contactName, result);
      history = await listHistory();
    } catch (reason) {
      error = explain(reason);
    }
  }

  async function loadPhonebook() {
    const url = phonebookUrl.trim();
    if (!url) {
      contactsError = "آدرس دفترچه تلفن را وارد کنید.";
      return;
    }
    busy = true;
    error = "";
    notice = "";
    try {
      settings = await saveSettings({ ...settings, contactsUrl: url });
      draft = { ...settings };
      phonebookUrl = settings.contactsUrl;
      query = "";
      await refreshContacts(false);
    } catch (reason) {
      contactsError = explain(reason);
      error = contactsError;
    } finally {
      busy = false;
    }
  }

  async function refreshContacts(quiet = false) {
    if (!settings.contactsUrl) {
      contacts = [];
      contactsError = "";
      return;
    }
    try {
      contacts = await fetchContacts();
      contactsError = "";
      if (!lockedName) contactName = matchContact(number);
      if (!quiet) notice = `${contacts.length} مخاطب خوانده شد`;
    } catch (reason) {
      contactsError = explain(reason);
      if (!quiet) error = contactsError;
    }
  }

  async function save(complete: boolean) {
    busy = true;
    error = "";
    notice = "";
    try {
      settings = await saveSettings({ ...draft, setupComplete: complete || draft.setupComplete });
      draft = { ...settings };
      view = "dial";
      notice = "ذخیره شد";
    } catch (reason) {
      error = explain(reason);
    } finally {
      busy = false;
    }
  }

  async function testCall() {
    await save(true);
    if (error) return;
    number = "012345678910";
    view = "dial";
    await call();
  }

  async function syncPhone() {
    await save(true);
    if (error) return;
    busy = true;
    try {
      notice = await syncActionUrls();
    } catch (reason) {
      error = explain(reason);
    } finally {
      busy = false;
    }
  }

  async function readLegacy() {
    error = "";
    notice = "";
    try {
      const imported = await importLegacy(legacyFolder);
      draft.phoneIp = imported.phoneIp;
      draft.sipAccount = imported.sipAccount;
      draft.webUsername = imported.webUsername;
      draft.webPassword = imported.webPassword;
      draft.useSsl = imported.useSsl;
      notice = "تنظیمات قبلی خوانده شد. ذخیره را بزنید.";
    } catch (reason) {
      error = explain(reason);
    }
  }

  function chooseContact(contact: Contact) {
    number = contact.number;
    contactName = contact.name;
    lockedName = true;
    view = "dial";
  }

  async function lookForUpdate() {
    busy = true;
    error = "";
    notice = "";
    try {
      update = await checkUpdate(false);
      notice = update ? `نسخه ${update.version} آماده نصب است.` : "همین نسخه آخرین نسخه است.";
    } catch (reason) {
      error = explain(reason);
    } finally {
      busy = false;
    }
  }

  async function applyUpdate() {
    busy = true;
    error = "";
    notice = "در حال دریافت نسخه جدید...";
    try {
      await installUpdate();
      notice = "نصب شروع شد. برنامه بسته می‌شود و دوباره باز می‌شود.";
    } catch (reason) {
      error = explain(reason);
      busy = false;
    }
  }

  function statusClass() {
    if (phase === "ringing" || phase === "calling" || phase === "held") return phase === "held" ? "ring" : phase === "calling" ? "call" : "ring";
    if (phase === "active") return "live";
    if (status.includes("بی‌پاسخ") || status.includes("رد") || status.includes("قطع") || dnd) return "stop";
    return "";
  }
</script>

<main class="app">
  <header class="top">
    <div class="brand">
      <strong>{view === "setup" ? "راه‌اندازی شماره‌گیر" : "شماره‌گیر"}</strong>
      <span>{settings.phoneIp || "تلفن هنوز تنظیم نشده"}</span>
    </div>
    {#if settings.setupComplete}<span class="badge">{modelLabel}</span>{/if}
  </header>

  {#if error}<div class="banner">{error}</div>{/if}
  {#if notice}<div class="banner ok">{notice}</div>{/if}
  {#if update}
    <div class="banner ok update">
      <span>نسخه {update.version} آماده نصب است.{update.notes ? ` ${update.notes}` : ""}</span>
      <button class="text-btn" type="button" disabled={busy} onclick={applyUpdate}>نصب و باز کردن دوباره</button>
    </div>
  {/if}

  <section class="stage">
    {#if view === "setup" || view === "settings"}
      <div class="models">
        {#each models as model (model.id)}
          <button type="button" class:on={draft.phoneModel === model.id} onclick={() => (draft.phoneModel = model.id)}>
            <strong>{model.label}</strong>
            <span>{model.note}</span>
          </button>
        {/each}
      </div>
      <form class="form card" onsubmit={(event) => { event.preventDefault(); save(true); }}>
        <label class="field">آی‌پی تلفن<input class="ltr" bind:value={draft.phoneIp} placeholder="192.168.1.20" required /></label>
        <label class="field">حساب SIP<input class="ltr" bind:value={draft.sipAccount} placeholder="1001@192.168.1.2" /></label>
        <label class="field">نام کاربری وب<input class="ltr" bind:value={draft.webUsername} required /></label>
        <label class="field">رمز وب
          <input class="ltr" type={showPassword ? "text" : "password"} bind:value={draft.webPassword} />
        </label>
        <button class="text-btn" type="button" onclick={() => (showPassword = !showPassword)}>{showPassword ? "پنهان کردن رمز" : "نمایش رمز"}</button>
        <label class="field">آدرس دفترچه تلفن<input class="ltr" bind:value={draft.contactsUrl} placeholder="http://192.168.100.97:8080/phonebook.xml" /></label>
        <label class="field">آدرس تماس ورودی روی تلفن
          <textarea class="url" readonly rows="3" value={callback?.incomingUrl || "بعد از وارد کردن آی‌پی تلفن، این آدرس ساخته می‌شود."}></textarea>
        </label>
        <p class="muted">در تلفن، Features سپس Action URL، فیلد Incoming call باید همین آدرس باشد. ۱۲۷.۰.۰.۱ را نگذارید؛ از دید تلفن آن آدرس خود تلفن است. دکمه ثبت رویدادها این آدرس را برای زنگ، پاسخ و قطع می‌نویسد.</p>
        <label class="check"><input type="checkbox" bind:checked={draft.useSsl} /> اتصال HTTPS به تلفن</label>
        <label class="check"><input type="checkbox" bind:checked={draft.autostart} /> با ورود به ویندوز اجرا شود</label>
        <button class="go" type="submit" disabled={busy}>{view === "setup" ? "شروع" : "ذخیره"}</button>
        {#if view === "settings"}
          <div class="row">
            <button class="text-btn" type="button" disabled={busy} onclick={testCall}>تماس آزمایشی</button>
            <button class="text-btn" type="button" disabled={busy} onclick={syncPhone}>ثبت رویدادها</button>
            <button class="text-btn" type="button" disabled={busy} onclick={lookForUpdate}>بررسی به‌روزرسانی</button>
          </div>
          {#if appVersionLabel}<p class="muted">نسخه {appVersionLabel}</p>{/if}
          <label class="field">پوشه data برنامه قبلی
            <div class="row">
              <input class="ltr" bind:value={legacyFolder} placeholder="C:\YDialer\data" />
              <button class="text-btn" type="button" onclick={readLegacy}>خواندن</button>
            </div>
          </label>
        {/if}
      </form>
    {:else if view === "contacts"}
      <form class="form" onsubmit={(event) => { event.preventDefault(); loadPhonebook(); }}>
        <label class="field">آدرس دفترچه تلفن
          <input class="ltr" placeholder="http://192.168.100.97:8080/phonebook.xml" bind:value={phonebookUrl} />
        </label>
        <button class="go" type="submit" disabled={busy}>خواندن دفترچه</button>
      </form>
      <input class="number" placeholder="جستجوی نام یا شماره" bind:value={query} />
      <div class="list">
        {#if contactsError}
          <p class="muted">{contactsError}</p>
        {:else if !settings.contactsUrl}
          <p class="muted">آدرس را بنویسید و خواندن دفترچه را بزنید.</p>
        {:else if filtered.length === 0}
          <p class="muted">مخاطبی پیدا نشد.</p>
        {:else}
          {#each filtered as contact (contact.number + contact.name)}
            <button type="button" onclick={() => chooseContact(contact)}>
              <b>{contact.name || "بدون نام"}</b>
              <div class="ltr">{contact.number}</div>
            </button>
          {/each}
        {/if}
      </div>
      <button class="text-btn" type="button" onclick={() => refreshContacts(false)}>به‌روزرسانی</button>
    {:else if view === "history"}
      <div class="list">
        {#if history.length === 0}
          <p class="muted">تماسی ثبت نشده است.</p>
        {:else}
          {#each history as entry, index (entry.at + entry.number + index)}
            <button type="button" class="history" onclick={() => chooseContact({ name: entry.name, number: entry.number })}>
              <span class="spread"><b>{entry.name || entry.number}</b><small>{entry.result}</small></span>
              <span class="spread"><span class="ltr">{entry.number}</span><small>{entry.direction} · {entry.at}</small></span>
            </button>
          {/each}
        {/if}
      </div>
    {:else}
      <div class="card">
        <div class="contact-name">{contactName}</div>
        <div class="number-row">
          <input class="number" dir="ltr" bind:value={number} oninput={onNumberInput} placeholder="شماره" />
        </div>
        <div class="status {statusClass()}">{status}</div>
      </div>

      {#if !inCall}
        <div class="pad">
          {#each keys as key (key)}
            <button type="button" onclick={() => press(key)}>{key}</button>
          {/each}
          <button type="button" onclick={() => press("+")}>+</button>
          <button type="button" class="wide" onclick={backspace}>حذف</button>
        </div>
      {/if}

      <div class="actions">
        {#if phase === "ringing"}
          <button class="go" type="button" disabled={busy} onclick={answer}>پاسخ</button>
          <button class="stop" type="button" disabled={busy} onclick={reject}>رد کردن</button>
        {:else if inCall}
          <button class="stop" type="button" disabled={busy} onclick={hangup}>قطع تماس</button>
          <div class="tools">
            <button type="button" disabled={busy} onclick={toggleHold}>{held ? "ادامه" : "نگه"}</button>
            <button type="button" class:on={muted} disabled={busy} onclick={toggleMute}>{muted ? "صدا" : "بی‌صدا"}</button>
            <button type="button" disabled={busy} onclick={() => (transferOpen = !transferOpen)}>انتقال</button>
            <button type="button" disabled={busy} onclick={() => run("conference")}>کنفرانس</button>
            <button type="button" disabled={busy} onclick={() => run("speaker")}>اسپیکر</button>
            <button type="button" disabled={busy} onclick={() => run("headset")}>هدست</button>
          </div>
          {#if transferOpen}
            <div class="transfer">
              <input dir="ltr" bind:value={transferTo} placeholder="شماره مقصد" />
              <button class="go" type="button" disabled={busy} onclick={transfer}>انتقال</button>
            </div>
          {/if}
        {:else}
          <button class="go" type="button" disabled={busy || number.trim() === ""} onclick={call}>تماس</button>
          <div class="tools">
            <button type="button" disabled={busy} onclick={() => run("redial")}>تکرار</button>
            <button type="button" class:on={dnd} disabled={busy} onclick={toggleDnd}>{dnd ? "DND روشن" : "مزاحم نشوید"}</button>
          </div>
        {/if}
      </div>
    {/if}
  </section>

  {#if settings.setupComplete}
    <nav class="nav">
      <button type="button" class:on={view === "dial"} onclick={() => (view = "dial")}>شماره‌گیر</button>
      <button type="button" class:on={view === "contacts"} onclick={() => { if (!phonebookUrl) phonebookUrl = settings.contactsUrl; view = "contacts"; refreshContacts(true); }}>مخاطبین</button>
      <button type="button" class:on={view === "history"} onclick={() => (view = "history")}>تاریخچه</button>
      <button type="button" class:on={view === "settings"} onclick={() => { draft = { ...settings }; view = "settings"; callbackInfo().then((info) => (callback = info)); }}>تنظیمات</button>
    </nav>
  {/if}
</main>
