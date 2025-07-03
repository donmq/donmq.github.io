import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { VW_T2_Meeting_LogParam } from '../_models/vW_T2_Meeting_LogDTO';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MeetingAuditReportService {
  baseUrl: string = environment.apiUrl + 'C_2_3_1_Meeting_Audit_Report/';

  constructor(private http: HttpClient) { }

  exportExcel(param: VW_T2_Meeting_LogParam) {
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get(this.baseUrl + 'ExportExcel', { params, responseType: 'blob' });
  }

}

