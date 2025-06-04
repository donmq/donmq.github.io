import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { GroupBaseAndGroupLangDto, GroupBaseTitleExcel } from '@models/manage/group-base/group-base-and-group-lang';
import { OperationResult } from '@utilities/operation-result';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GroupBaseService {
  apiUrl = environment.apiUrl;

  modelEdit = new BehaviorSubject<number>(null);
  modelEditDetail = new BehaviorSubject<GroupBaseAndGroupLangDto>(null);
  currentModel = this.modelEdit.asObservable();

  constructor(private http: HttpClient) { }

    changeParamsDetail(model: number) {
      this.modelEdit.next(model);
    }

    changeModelDetail(model: GroupBaseAndGroupLangDto) {
      this.modelEditDetail.next(model);
    }

  getGroupBase() {
    return this.http.get<GroupBaseAndGroupLangDto[]>(this.apiUrl + 'GroupBase/GetData');
  }

  addGroup(model: GroupBaseAndGroupLangDto) {
    return this.http.post<OperationResult>(this.apiUrl + 'GroupBase/AddGroup', model);
  }

  editGroup(model: GroupBaseAndGroupLangDto) {
    return this.http.put<OperationResult>(this.apiUrl + 'GroupBase/EditGroup', model);
  }

  getDetailGroupBase(IDGroupBase: number){
    let params = new HttpParams()
      .set('IDGroupBase', IDGroupBase.toString());
    return this.http.get<GroupBaseAndGroupLangDto>(this.apiUrl + 'GroupBase/GetDetailGroupBase', {params});
  }

  removeGroup(gBID: number) {

    let params = new HttpParams()
    .set('gBID', gBID);
    return this.http.delete<OperationResult>(this.apiUrl + 'GroupBase/RemoveGroup', { params });
  }

  exportExcel(titleExcel: GroupBaseTitleExcel) {
    let params = new HttpParams().appendAll({ ...titleExcel });
    return this.http.get<OperationResult>(this.apiUrl + "GroupBase/ExportExcel", {params});
  }
}
