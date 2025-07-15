import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BehaviorSubject } from 'rxjs';
import { environment } from '../../../environments/environment';
import { CheckMachineHisstoryParams } from '../_dtos/search-check-machine-history-params';
import { DataHistoryCheckMachine } from '../_models/data-history-check-machine';
import { HistoryCheckMachine } from '../_models/list-history-check-machine';
import { Pagination, PaginationResult } from '../_models/pagination';

const API = environment.apiUrl;

@Injectable({
  providedIn: 'root'
})
export class CheckMachineHistoryService {
  machineSource = new BehaviorSubject<Object>({});
  currentBrand = this.machineSource.asObservable();
  flagSource = new BehaviorSubject<string>('0');
  currentFlag = this.flagSource.asObservable();
  constructor(private http: HttpClient,
    private _spinnerService: NgxSpinnerService) { }

  getDetailHistoryCheckMachine(historyCheckMachineID: number) {
    return this.http.post<DataHistoryCheckMachine>(`${API}CheckMachineHistory/GetDetailHistoryCheckMachine?historyCheckMachineID=${historyCheckMachineID}`, {});
  }

  searchHistoryCheckMachine(checkMachineHisstoryParams: CheckMachineHisstoryParams, pagination: Pagination) {
    let params = new HttpParams();
    if (pagination.currentPage !== null && pagination.pageSize !== null) {
      params = params.append('pageNumber', pagination.currentPage.toString());
      params = params.append('pageSize', pagination.pageSize.toString());
    }
    return this.http.post<PaginationResult<HistoryCheckMachine>>(`${API}CheckMachineHistory/SearchHistoryCheckMachine`, checkMachineHisstoryParams, { params: params });
  }

  changeData(dataHistoryCheckMachine: DataHistoryCheckMachine) {
    this.machineSource.next(dataHistoryCheckMachine);
  }

  changeExport(historyCheckMachine: HistoryCheckMachine) {
    this.machineSource.next(historyCheckMachine);
  }

  exportPDF(historyCheckMachine: HistoryCheckMachine) {
    this._spinnerService.show();
    if (historyCheckMachine.createBy && historyCheckMachine.createBy.length > 20) {
      historyCheckMachine.createBy = historyCheckMachine.createBy.substring(0, 20);
    }
    return this.http.post(`${API}CheckMachineHistory/ExportFile`, historyCheckMachine, { responseType: 'blob', withCredentials: true }).subscribe((result: Blob) => {
      if (result.type !== 'application/pdf') {
        alert(result.type);
      }
      const blob = new Blob([result]);
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      const currentTime = new Date();
      const filename =
        'CheckMachineReport-' +
        currentTime.getFullYear().toString() +
        (currentTime.getMonth() + 1) + "_" +
        currentTime.getDate() +
        currentTime
          .toLocaleTimeString()
          .replace(/[ ]|[,]|[:]/g, '')
          .trim() +
        '.pdf';
      link.href = url;
      link.setAttribute('download', filename);
      document.body.appendChild(link);
      link.click();
      this._spinnerService.hide();
    });
  }

  exportExcel(historyCheckMachine: HistoryCheckMachine) {
    this._spinnerService.show();
    if (historyCheckMachine.createBy && historyCheckMachine.createBy.length > 20) {
      historyCheckMachine.createBy = historyCheckMachine.createBy.substring(0, 20);
    }
    return this.http.post(`${API}CheckMachineHistory/ExportFile`, historyCheckMachine, { responseType: 'blob', withCredentials: true }).subscribe((result: Blob) => {
      if (result.type !== 'application/xlsx') {
        alert(result.type);
      }
      const blob = new Blob([result]);
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      const currentTime = new Date();
      const filename =
        'CheckMachineReport-' +
        currentTime.getFullYear().toString() +
        (currentTime.getMonth() + 1) + "_" +
        currentTime.getDate() +
        currentTime
          .toLocaleTimeString()
          .replace(/[ ]|[,]|[:]/g, '')
          .trim() +
        '.xlsx';
      link.href = url;
      link.setAttribute('download', filename);
      document.body.appendChild(link);
      link.click();
      this._spinnerService.hide();
    });
  }
}
