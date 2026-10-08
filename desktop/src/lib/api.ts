import { invoke } from "@tauri-apps/api/core";

export type Settings = {
  phoneIp: string;
  sipAccount: string;
  webUsername: string;
  webPassword: string;
  useSsl: boolean;
  phoneModel: string;
  contactsUrl: string;
  actionCfgUrl: string;
  autostart: boolean;
  setupComplete: boolean;
};

export type PhoneModel = { id: string; label: string; note: string };
export type Contact = { name: string; number: string };
export type HistoryEntry = { at: string; direction: string; number: string; name: string; result: string };
export type Launch = { number: string; name: string; incoming: boolean; eventName: string; autostart: boolean };
export type CallbackInfo = { port: number; address: string; incomingUrl: string; listening: boolean };

export const getSettings = () => invoke<Settings>("get_settings");
export const saveSettings = (settings: Settings) => invoke<Settings>("save_settings", { settings });
export const listModels = () => invoke<PhoneModel[]>("list_models");
export const takeLaunch = () => invoke<Launch>("take_launch");
export const dial = (number: string) => invoke<void>("dial", { number });
export const phoneAction = (action: string, number?: string) => invoke<void>("phone_action", { action, number });
export const fetchContacts = () => invoke<Contact[]>("fetch_contacts");
export const callbackInfo = () => invoke<CallbackInfo>("callback_info");
export const syncActionUrls = () => invoke<string>("sync_action_urls");
export const listHistory = () => invoke<HistoryEntry[]>("list_history");
export const rememberCall = (direction: string, number: string, name: string, result: string) =>
  invoke<void>("remember_call", { direction, number, name, result });
export const finishLastCall = (result: string) => invoke<boolean>("finish_last_call", { result });
export const importLegacy = (folder: string) => invoke<Settings>("import_legacy", { folder });
