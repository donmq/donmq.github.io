import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { environment } from '@env/environment';
import { Departments } from '../../models/manage/departments';
import { DepartmentParams } from '@params/manage/departmentParams';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';

@Injectable({
  providedIn: 'root',
})
export class DepartmentService {
  baseURL = environment.apiUrl;
  departmentSource= new BehaviorSubject<Departments>(<Departments>{});
  currentDepartmentSource = this.departmentSource.asObservable();
  dataSource = new BehaviorSubject<DepartmentParams>(null)
  currentDataSource = this.dataSource.asObservable();
  constructor(private http: HttpClient) {}

  viewAndEdit(item:Departments){
    this.departmentSource.next(item);
  }
  getAllAreas(){
    return this.http.get<KeyValuePair[]>(this.baseURL + 'Department/GetAllAreas');
  }

  getAllBuildings(){
    return this.http.get<KeyValuePair[]>(this.baseURL + 'Department/GetAllBuildings');
  }

  getAllDepartments(pagination: Pagination, search: DepartmentParams) : Observable<PaginationResult<Departments>>
  {
    let areaId = search.areaID === 'All' || search.areaID === null ? '' : search.areaID;
    const params = new HttpParams()
    .set('pageNumber', pagination.pageNumber.toString())
    .set('pageSize', pagination.pageSize.toString())
    .set('AreaID', areaId)
    .set("deptCode", search.deptCode)
    return this.http.get<PaginationResult<Departments>>(this.baseURL + 'Department/GetAllDepartments',{params});
  }
  add(model:Departments){
    return this.http.post<OperationResult>(this.baseURL + 'Department/Add',model);
  }
  update(model:Departments){
    return this.http.put<OperationResult>(this.baseURL + 'Department/Update',model);
  }
}
