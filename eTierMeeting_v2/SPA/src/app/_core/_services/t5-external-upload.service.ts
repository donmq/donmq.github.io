import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Pagination, PaginationResult } from '../_utilities/pagination-helper';
import { eTM_HP_Efficiency_Data_External } from '../_models/eTM_HP_Efficiency_Data_External';
import { OperationResult } from '../_utilities/operation-result';

@Injectable({
    providedIn: 'root'
})
export class T5ExternalUploadService {
    baseUrl = environment.apiUrl;
    constructor(private http: HttpClient) { }

    getData(pagination: Pagination) { 
        let params = new HttpParams().set("pageNumber", pagination.pageNumber+'').set("pageSize", pagination.pageSize+'');
        return this.http.get<PaginationResult<eTM_HP_Efficiency_Data_External>>(this.baseUrl + "T5ExternalUpload/getData", { params });
    }

    uploadExcel(form: FormData) { 
        return this.http.post<OperationResult>(this.baseUrl + "T5ExternalUpload/uploadExcel", form);
    }

    getTemplate() { 
        return this.http.get(this.baseUrl + "T5ExternalUpload/downloadExcel", {responseType: 'blob' });
    }

}
