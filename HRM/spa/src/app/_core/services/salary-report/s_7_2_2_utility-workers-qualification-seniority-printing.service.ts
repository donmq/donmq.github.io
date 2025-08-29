import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, signal } from '@angular/core';
import { toObservable } from '@angular/core/rxjs-interop';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { environment } from '@env/environment';
import { D_7_2_2_UtilityWorkersQualificationSeniorityPrinting_Dto, D_7_2_2_UtilityWorkersQualificationSeniorityPrinting_Param } from '@models/salary-report/7_2_2_utility-workers-qualification-seniority-printing';
import { IClearCache } from '@services/cache.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';

@Injectable({
    providedIn: 'root'
})
export class S_7_2_2_UtilityWorkersQualificationSeniorityPrinting implements IClearCache {
    get language(): string { return localStorage.getItem(LocalStorageConstants.LANG) }
    apiUrl: string = environment.apiUrl + "C_7_2_2_UtilityWorkersQualificationSeniorityPrinting/"
    initData: D_7_2_2_UtilityWorkersQualificationSeniorityPrinting_Dto = <D_7_2_2_UtilityWorkersQualificationSeniorityPrinting_Dto>{
        param: <D_7_2_2_UtilityWorkersQualificationSeniorityPrinting_Param>{},
        totalRows: 0
    }
    programSource = signal<D_7_2_2_UtilityWorkersQualificationSeniorityPrinting_Dto>(structuredClone(this.initData));
    programSource$ = toObservable(this.programSource);
    setSource = (source: D_7_2_2_UtilityWorkersQualificationSeniorityPrinting_Dto) => this.programSource.set(source);
    clearParams = () => {
        this.programSource.set(structuredClone(this.initData))
    }
    constructor(private _http: HttpClient) { }

    search(param: D_7_2_2_UtilityWorkersQualificationSeniorityPrinting_Param) {
        param.language = this.language
        let params = new HttpParams().appendAll({ ...param });
        return this._http.get<number>(this.apiUrl + 'Search', { params })
    }
    download(param: D_7_2_2_UtilityWorkersQualificationSeniorityPrinting_Param) {
        param.language = this.language;
        let params = new HttpParams().appendAll({ ...param });
        return this._http.get<OperationResult>(this.apiUrl + 'DownloadFileExcel', { params });
    }
    getListFactory() {
        let params = new HttpParams().appendAll({ language: this.language })
        return this._http.get<KeyValuePair[]>(this.apiUrl + 'GetListFactory', { params });
    }
    getListDepartment(factory: string) {
        let params = new HttpParams().appendAll({ factory, language: this.language })
        return this._http.get<KeyValuePair[]>(this.apiUrl + 'GetListDepartment', { params });
    }
}
