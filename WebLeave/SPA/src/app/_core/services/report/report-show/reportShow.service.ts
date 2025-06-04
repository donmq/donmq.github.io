import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ExportExcelDateDto } from '@models/report/report-show/export-excel-date-dto';
import { ExportExcelGridDto } from '@models/report/report-show/export-excel-grid-dto';
import { BehaviorSubject, Observable } from 'rxjs';
import { ReportIndexViewModel } from '@models/report/report-show/report-index-view-model.model';
import { ReportShowModel } from '@models/report/report-show/report-show-model.model';
import { ReportDateDetailParam } from '@params/report/report-show/report-date-detail-param.param';
import { ReportGridDetailParam } from '@params/report/report-show/report-grid-detail-param.param';
import { ReportShowParam } from '@params/report/report-show/report-show-param.param';
import { KeyValuePair } from '@utilities/key-value-pair';
import { environment } from '@env/environment';
import { OperationResult } from '@utilities/operation-result';
@Injectable({
  providedIn: 'root',
})
export class ReportShowService {
  apiUrl: string = environment.apiUrl;
  private paramSource = new BehaviorSubject<KeyValuePair>(<KeyValuePair>{});
  param = this.paramSource.asObservable();

  constructor(private http: HttpClient) {}

  updatedParam(data: KeyValuePair) {
    this.paramSource.next(data);
  }

  reportShow(param: ReportShowParam): Observable<ReportIndexViewModel> {
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<ReportIndexViewModel>(
      this.apiUrl + 'Report/ReportShowIndex',
      { params }
    );
  }

  reportGridDetail(
    param: ReportGridDetailParam
  ): Observable<ReportShowModel[]> {
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<ReportShowModel[]>(
      this.apiUrl + 'Report/ReportGridDetail',
      { params }
    );
  }

  reportDateDetail(
    param: ReportDateDetailParam
  ): Observable<ReportShowModel[]> {
    let params = new HttpParams().appendAll({ ...param });
    return this.http.get<ReportShowModel[]>(
      this.apiUrl + 'Report/ReportDateDetail',
      { params }
    );
  }

  // exportExcel(model: ExportExcelGridDto) {
  //   return this.http.post(this.apiUrl + 'Report/ExportGridDetail', model, {
  //     responseType: 'blob',
  //   });
  // }

  exportExcelDateDetail(model: ExportExcelDateDto) {
    return this.http.post<OperationResult>(this.apiUrl + 'Report/ExportDateDetail', model)
  }

  exportExcel(model: ExportExcelGridDto) {
    return this.http.post<OperationResult>(this.apiUrl + 'Report/ExportGridDetail', model)
  }
}
