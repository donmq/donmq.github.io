import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { BehaviorSubject } from 'rxjs';
import { environment } from '@env/environment';
import { Employee, EmployeeDetalParam, ListEmployee } from '@models/manage/employee-manage/employee';
import { EmployExport } from '@models/manage/employee-manage/employExport';
import { LeaveData } from '@models/manage/employee-manage/leaveData';
import { KeyValuePair } from '@utilities/key-value-pair';
import { OperationResult } from '@utilities/operation-result';
import { PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { EmployeeParam } from '@params/manage/employee-param';


const API = environment.apiUrl;
@Injectable({
  providedIn: 'root'
})
export class EmployeeService {
  baseUrl = environment.apiUrl;
  employeeSource= new BehaviorSubject<ListEmployee>(null);
  currentemployee = this.employeeSource.asObservable();
  dataSource = new BehaviorSubject<EmployeeParam>(null)
  currentDataSource = this.dataSource.asObservable();

  constructor(private http: HttpClient,
              private spinnerService: NgxSpinnerService) { }

  //Page List
  getAll(pagination: PaginationParam, param: EmployeeParam){
    let params = new HttpParams().appendAll({...pagination, ...param});
    return this.http.get<PaginationResult<ListEmployee>>(API + 'Employees/SearchEmploy', { params });
  }

  exportExcel() {
    this.spinnerService.show();
    return this.http.get<OperationResult>(`${API}Employees/ExportExcel`)
  }


  //Page Edit

  //Lấy KeyValue để tạo option
  getListDeptID() {
    return this.http.get<KeyValuePair[]>(API + "Employees/getListDeptID");
  }

  getListPartID(deptID:number ) {
    return this.http.get<KeyValuePair[]>(`${API}Employees/getListPartID?DeptID=${deptID}`);
  }

  getListGroupBase() {
    return this.http.get<KeyValuePair[]>(`${API}Employees/getListGroupBase`);
  }

  getListPositionID() {
   return this.http.get<KeyValuePair[]>(API + "Employees/getListPositionID");
  }
  //Update data
  updateEmployee(employee: Employee) {
    return this.http.put<OperationResult>(`${API}Employees/UpdateEmploy`, employee);
  }

  //Page Detail
  //          1. Button
  //Nút vô hiệu hóa
  changeVisible(empID: number) {
    return this.http.get<OperationResult>(`${API}Employees/getDisable?empID=${empID}`);
  }
  //Nút Xóa dữ liệu
  remove(empID: number) {
    return this.http.delete<OperationResult>(`${API}Employees/RemoveEmploy?empID=${empID}`);
  }
  //Nút edit dữ liệu
  updateInDetail(employee: EmployExport) {
    return this.http.put<OperationResult>(`${API}Employees/UpdateInDetail`, employee);
  }

  //          2. In ra Detail
  //Lấy dữ liệu để in ra chỗ detail
  getDetail(EmpID: number, lang: string) {
    return this.http.get<EmployExport>(`${API}Employees/getDataDetail?EmpID=${EmpID}&lang=${lang}`);
  }

  getEditDetail(EmpID: number, lang: string) {
    return this.http.get(`${API}Employees/getDataDetail?EmpID=${EmpID}&lang=${lang}`);
  }


  //          3. Danh sách phép

  //Thanh search catalog
  getListCatelog(lang:string ) {
    return this.http.get<KeyValuePair[]>(`${API}Employees/ListCataLog?lang=${lang}`);
  }

  //Danh sách sau khi search
  searchDetail(pagination: PaginationParam, param: EmployeeDetalParam){
    let params = new HttpParams().appendAll({...pagination, ...param})
    return this.http.get<PaginationResult<LeaveData>>(API + 'Employees/SearchDetail', { params });
  }

}
