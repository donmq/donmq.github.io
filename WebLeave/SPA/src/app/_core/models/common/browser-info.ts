export interface BrowserInfo {
  ipLocal: string;
  factory: string;
  loginDetect: LoginDetect;
}

export interface LoginDetect {
  detectID: number;
  userName: string;
  loggedByIP: string;
  loggedAt: string;
  expires: string;
}