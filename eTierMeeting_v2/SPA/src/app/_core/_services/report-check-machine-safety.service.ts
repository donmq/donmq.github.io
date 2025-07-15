import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { OperationResult, OperationResultWithData } from '../_models/operation-result';
import { ReportCheckMachineSafetyParam } from '../_dtos/report-check-machine-safety';


const API = environment.apiUrl + 'ReportCheckMachineSafety/';

@Injectable({
  providedIn: 'root'
})
export class ReportCheckMachineSafetyService {

  constructor(
    private http: HttpClient) { }

  exportExcel(param: ReportCheckMachineSafetyParam) {
    const params = new HttpParams().appendAll({ ...param });
    return this.http.get<OperationResultWithData<any>>(`${API}ExportExcel`, { params });
  }
}

