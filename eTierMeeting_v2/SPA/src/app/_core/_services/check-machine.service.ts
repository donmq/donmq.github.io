import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from '../../../environments/environment';
import { CheckMachineHome } from '../_models/check-machine-home';
import { ResultHistoryCheckMachine } from '../_models/export-pdf';

const API = environment.apiUrl;
@Injectable({
  providedIn: 'root'
})
export class CheckMachineService {
  constructor(private http: HttpClient,
    private _spinnerService: NgxSpinnerService) { }

  getMachine(machineName: string, lang: string) {
    return this.http.post<CheckMachineHome>(`${API}CheckMachine/GetMachine?idMachine=${machineName}&lang=${lang}`, {});
  }
  submitCheckMachine(listCheckMachine: CheckMachineHome) {
    return this.http.post<any>(`${API}CheckMachine/SubmitCheckMachine`, listCheckMachine);
  }
  submitCheckMachineAll(listCheckMachine: CheckMachineHome) {
    return this.http.post<any>(`${API}CheckMachine/SubmitCheckMachineAll`, listCheckMachine);
  }

  exportPDF(listCheckMachine: ResultHistoryCheckMachine) {
    this._spinnerService.show();
    return this.http.post(`${API}CheckMachine/ExportPDF`, listCheckMachine, { responseType: 'blob', withCredentials: true }).subscribe((result: Blob) => {
      if (result.type !== 'application/pdf') {
        alert(result.type);
      }
      const blob = new Blob([result]);
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement('a');
      const currentTime = new Date();
      const filename =
        'CheckMachineReportPdf-' +
        currentTime.getFullYear().toString() +
        (currentTime.getMonth() + 1) + '_' +
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
}
