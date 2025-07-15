import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { KeyValuePair } from '../_utility/key-value-pair';
import { OperationResult } from '../_models/operation-result';
import { Machine_Safe_Check } from '../_models/machine_safe_check';

const API = environment.apiUrl + 'CheckMachineSafety/';
@Injectable({
  providedIn: 'root'
})
export class CheckMachineSafetyService {
  constructor(private http: HttpClient) { }

  getMachine(machineName: string, lang: string) {
    return this.http.post<Machine_Safe_Check>(`${API}GetMachine?idMachine=${machineName}&lang=${lang}`, {});
  }

  saveMachineSafetyCheck(param: FormData) {
    return this.http.post<OperationResult>(`${API}SaveMachineSafetyCheck`, param)
  }

  getQuestion(lang: string) {
    return this.http.get<KeyValuePair[]>(`${API}GetListQuestion?lang=${lang}`);
  }

}
