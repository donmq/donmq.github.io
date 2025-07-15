import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { MachineScan } from '../_models/machine-scan';

const API = environment.apiUrl;
@Injectable({
  providedIn: 'root'
})
export class MachineMoveService {

  constructor(private http: HttpClient) { }

  getMachine(idMachine: string, lang: string) {
    return this.http.post<MachineScan>(`${API}Machine/GetMachineByID?idMachine=${idMachine}&lang=${lang}`, {});
  }

  moveMachine(idMachine: string, fromEmploy: string, toEmploy: string, fromPlno: string, toPlno: string) {
    const dataMove = {
      idMachine: idMachine,
      fromEmploy: fromEmploy,
      toEmploy: toEmploy,
      fromPlno: fromPlno,
      toPlno: toPlno
    };
    let params = new HttpParams();
    if (dataMove !== null) {
      params = params.append('idMachine', idMachine);
      params = params.append('fromEmploy', fromEmploy);
      params = params.append('toEmploy', toEmploy);
      params = params.append('fromPlno', fromPlno);
      params = params.append('toPlno', toPlno);
    }
    return this.http.post<any>(`${API}Machine/MoveMachine`, dataMove, { params, withCredentials: true });
  }
}
