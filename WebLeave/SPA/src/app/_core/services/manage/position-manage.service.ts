import { PaginationResult, PaginationParam } from '@utilities/pagination-utility';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, tap } from 'rxjs';
import { environment } from '@env/environment';
import { PositionManage } from '@models/manage/position-manage/postion-manage';
import { OperationResult } from '@utilities/operation-result';

@Injectable({
  providedIn: 'root'
})
export class PositionManageService {
  apiUrl = environment.apiUrl;
  //edit
  modelEdit = new BehaviorSubject<number>(null);
  currentModel = this.modelEdit.asObservable();

  constructor(private http: HttpClient) { }
  getAllPosition(pagination: PaginationParam) {

    let params = new HttpParams().appendAll({ ...pagination });
    return this.http.get<PaginationResult<PositionManage>>(this.apiUrl + 'PositionManage/GetAllPosition', { params });
  }
  exportExcel(param: PaginationParam, detailPositionManage: PositionManage) {
    let params = new HttpParams().appendAll({ ...param , ...detailPositionManage});

    return this.http.get<OperationResult>(`${this.apiUrl}PositionManage/ExportExcelAll`,{ params })
  }
  //change for Edit
  changeParamsDetail(model: number) {
    this.modelEdit.next(model);
  }
  removePosition(IDPosition: number) {
    let params = new HttpParams()
      .set('IDPosition', IDPosition.toString());
    return this.http.delete<OperationResult>(this.apiUrl + 'PositionManage/RemovePosition', { params });
  }
  getDetailPosition(IDPosition: number) {
    let params = new HttpParams()
      .set('IDPosition', IDPosition.toString());
    return this.http.get<PositionManage>(this.apiUrl + 'PositionManage/GetDetailPosition', { params });
  }
  addPosition(model: PositionManage) {
    return this.http.post<OperationResult>(this.apiUrl + 'PositionManage/AddPosition', model, {});
  }
  editPosition(model: PositionManage) {
    return this.http.put<OperationResult>(this.apiUrl + 'PositionManage/EditPosition', model, {});
  }
}
