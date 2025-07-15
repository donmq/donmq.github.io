import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { AssetsLendMaintainDto, AssetsLendMaintainParam } from '../_models/assets-lend-maintain';
import { HttpClient, HttpParams } from '@angular/common/http';
import { OperationResult } from '../_models/operation-result';
import { KeyValuePair } from '../_utility/key-value-pair';
import { Pagination, PaginationResult } from '../_models/pagination';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AssetsLendMaintainService {
  apiUrl: string = environment.apiUrl + 'AssetsLendMaintain/'

  constructor(private http: HttpClient) { }

  getData(pagination: Pagination, param: AssetsLendMaintainParam): Observable<PaginationResult<AssetsLendMaintainDto>> {
    let params = new HttpParams().appendAll({ ...pagination, ...param });
    return this.http.get<PaginationResult<AssetsLendMaintainDto>>(this.apiUrl + 'GetData', { params });
  }

  downloadExcel(param: AssetsLendMaintainParam) {
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get(this.apiUrl + 'DownloadExcel', { params, responseType: 'blob' });
  }

  uploadExcel(file: FormData) {
    return this.http.post<OperationResult>(this.apiUrl + 'UploadExcel', file, {} );
  }

  update(data: AssetsLendMaintainDto) {
    return this.http.put<OperationResult>(this.apiUrl + 'Update', data);
  }

  getListLendTo() {
    return this.http.get<KeyValuePair[]>(this.apiUrl + 'GetListLendTo');
  }
}
