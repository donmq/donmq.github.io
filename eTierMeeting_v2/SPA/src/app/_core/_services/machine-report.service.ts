import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { SearchMachineParams } from '../_dtos/search-machine-params';
import { ReportMachine } from '../_models/report-machine';

@Injectable({
  providedIn: 'root'
})
export class MachineReportService {
  url = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getMachineRport(searchMachineParams: SearchMachineParams) {
    searchMachineParams.pdcId = + searchMachineParams.pdcId;
    searchMachineParams.buildingCode = searchMachineParams.buildingCode;
    return this.http.post<ReportMachine>(this.url + 'ReportMachine/GetListReportMachine', searchMachineParams);
  }
}
