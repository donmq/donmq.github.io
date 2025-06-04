import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { environment } from '@env/environment';
import { CompanyManage } from '../../models/manage/company-manage/company-manage';
import { OperationResult } from '@utilities/operation-result';

@Injectable({
  providedIn: 'root'
})
export class CompanyManageService {
  apiUrl = environment.apiUrl;
  //edit
  modelEdit = new BehaviorSubject<CompanyManage>(null);
  currentModel = this.modelEdit.asObservable();
  
  constructor(
    private http: HttpClient,
  ) { }
  getAllCompany() {
    return this.http.get<CompanyManage[]>(this.apiUrl + 'Common/GetCompanys');
  }
  //change for Edit
  changeParamsDetail(model: CompanyManage) {
    this.modelEdit.next(model);
  }
  addCompany(model: CompanyManage){
    return this.http.post<OperationResult>(this.apiUrl + 'CompanyManage/AddCompany', model, {});
  }
  editCompany(model: CompanyManage){
    return this.http.put<OperationResult>(this.apiUrl + 'CompanyManage/EditCompany', model, {});
  }
}
