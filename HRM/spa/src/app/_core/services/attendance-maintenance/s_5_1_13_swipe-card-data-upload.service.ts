import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { ResolveFn } from '@angular/router';
import { LangConstants } from '@constants/lang-constants';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import { HRMS_Att_Swipe_Card_Upload } from '@models/attendance-maintenance/5_1_13_swipe-card-data-upload';
import { FunctionUtility } from '@utilities/function-utility';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root'
})
export class S_5_1_13_SwipeCardDataUploadService implements IClearCache {
  get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
  baseUrl = `${environment.apiUrl}C_5_1_13_SwipeCardDataUpload`;
  constructor(private _http: HttpClient,
    private _functionUtility: FunctionUtility
  ) { }
  clearParams: () => void;

  getFactories() {
    return this._http.get<KeyValuePair[]>(`${this.baseUrl}/GetFactories`, { params: { language: this.language } });
  }

  excute(model: HRMS_Att_Swipe_Card_Upload) {
    let formData = this._functionUtility.toFormData(model);
    return this._http.post<OperationResult>(`${this.baseUrl}/Excute`, formData);
  }
}

export const resolverFactories: ResolveFn<KeyValuePair[]> = () => {
  return inject(S_5_1_13_SwipeCardDataUploadService).getFactories();
};
