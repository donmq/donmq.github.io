import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { Category } from '@models/manage/category-management/category';
import { CategoryDetail } from '@models/manage/category-management/category-detail';
import { OperationResult } from '@utilities/operation-result';
import { PaginationParam, PaginationResult } from '@utilities/pagination-utility';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  baseUrl = environment.apiUrl
  constructor(private http: HttpClient) { }

  getAllCategory(pagination: PaginationParam) {
    let params = new HttpParams().appendAll({ ...pagination });
    return this.http.get<PaginationResult<Category>>(`${this.baseUrl}Category/GetAll`, { params });
  }

  getEditDetail(id: number) {
    let params = new HttpParams().set('id', id.toString());
    return this.http.get<CategoryDetail>(`${this.baseUrl}Category/GetEditDetail`, { params });
  }

  create(categoryDetail: CategoryDetail) {
    return this.http.post<OperationResult>(`${this.baseUrl}Category/Create`, categoryDetail);
  }

  edit(categoryDetail: CategoryDetail) {
    return this.http.put<OperationResult>(`${this.baseUrl}Category/Update`, categoryDetail);
  }

}
