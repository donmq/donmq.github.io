import { LocalStorageConstants } from "@constants/storage.constants";

const port = ':5000';
const protocol = window.location.protocol;
const ip = window.location.hostname;
const apiUrl = `${protocol}//${ip}${port}`;
const factory = localStorage.getItem(LocalStorageConstants.FACTORY);
const area = localStorage.getItem(LocalStorageConstants.AREA);
const kanbanPort = area === "T" ? (factory === "SPC" ? ":8021" : ":8020") : "";
const kanbanIP = `${protocol}//${ip}${kanbanPort}`;
let kaizenUrl: string;
let ipKanban: string;
let ipKanbanSTF: string;
let ipKanbanGQT: string;
let prefix:string = kanbanIP.split(".").slice(0, 3).join(".");
switch (factory) {
  case "SHC":
    kaizenUrl = "https://10.4.0.14:3031";
    ipKanban = `${protocol}//10.4.0.133/EKS/html/demo.html?id=A2023060813142917010000&zoom=true`;
    break;
  case "CB":
    kaizenUrl = "https://10.9.0.51:3031";
    ipKanban = `${kanbanIP}/Web/KanBan/KanBanMain`;
    break;
  // case "SPC":
  //   kaizenUrl = "https://10.10.0.23:3031";
  //   break;
  case "TSH":
    kaizenUrl = "https://10.11.0.29:3031";
    ipKanban = `${protocol}//10.11.0.63/EKS/html/demo.html?id=A2023060813142917010000&zoom=true`;
}

ipKanbanSTF = `${kanbanIP}/Web/KanBan/KanBanMain_STF`;
ipKanbanGQT = `${kanbanIP}/Web/KanBan/KanBanMain_GQT`;

export const environment = {
  production: false,
  apiUrl: `${apiUrl}/api/`,
  baseUrl: `${apiUrl}/`,
  hubUrl: `${apiUrl}/`,
  allowedDomains: `${ip}${port}`,
  disallowedRoutes: `${ip}${port}/api/auth`,
  ip_img_path: `${protocol}//${ip}/`,
  ipKanban: ipKanban,
  ipKanbanSTF: `${kanbanIP}/Web/KanBan/KanBanMain_STF`,
  ipKanbanGQT: `${kanbanIP}/Web/KanBan/KanBanMain_GQT`,
  ipKaizen: kaizenUrl
};
