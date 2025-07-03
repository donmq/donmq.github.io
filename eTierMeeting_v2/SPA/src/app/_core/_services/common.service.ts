import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable, signal } from "@angular/core";
import { environment } from "../../../environments/environment";
import { eTM_Video_Play_LogDTO } from "../_models/eTM_Video_Play_LogDTO";
import { eTM_Meeting_Log_Page, eTM_Meeting_Log_PageParamDTO } from "../_models/eTM_Meeting_Log_Page";
import { ServerInfo } from "../_models/server-info";
import { KeyValuePair } from "../_utilities/key-value-pair";
import { OperationResult } from "../_utilities/operation-result";

@Injectable({
  providedIn: "root",
})
export class CommonService {
  baseUrl = environment.apiUrl;
  dataPreOrNext = signal<string>("");
  recordID = signal<string>("");
  end_time = signal<Date | null>(null);
  constructor(private http: HttpClient) { }

  getLineID(deptID: string) {
    let params = new HttpParams().set("deptID", deptID);
    return this.http.get<OperationResult>(this.baseUrl + "Common/getLineID", { params });
  }

  getRouteT5() {
    return this.http.get<OperationResult>(this.baseUrl + "Common/GetRouteT5");
  }

  getServerInfo() {
    return this.http.get<ServerInfo>(this.baseUrl + "Common/ServerInfo");
  }

  addVideoLog(videoLogDTO: eTM_Video_Play_LogDTO) {
    let params = new HttpParams().appendAll({ ...videoLogDTO });
    return this.http.get<OperationResult>(this.baseUrl + "Common/AddVideoLog", { params });
  }

  addMeetingLogPage(eTM_Meeting_Log_Page: eTM_Meeting_Log_Page) {
    return this.http.post<string>(this.baseUrl + "Common/AddMeetingLogPage", eTM_Meeting_Log_Page);
  }

  updateMeetingLogPage(params: eTM_Meeting_Log_PageParamDTO) {
    return this.http.put<OperationResult>(this.baseUrl + "Common/UpdateMeetingLogPage", params);
  }
}
