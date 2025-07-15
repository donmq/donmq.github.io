import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { Building } from '../_models/building';
import { Cell } from '../_models/cell';
import * as _ from 'lodash';
import { InventoryLine } from '../_models/inventoryLine';
import { ResultAllInventory } from '../_dtos/detail-inventory-params';
import { ReportHistoryInventoryParams, ReportKiemKeParam } from '../_dtos/ReportHistoryInventoryParams';
import { NgxSpinnerService } from 'ngx-spinner';


const API = environment.apiUrl;

@Injectable({
  providedIn: 'root'
})
export class ReportInventoryService {

  BuildingSource = new BehaviorSubject<any>({});

  currentBuilding = this.BuildingSource.asObservable();

  constructor(
    private http: HttpClient,
    private _spinnerService: NgxSpinnerService) { }

  changeBuilding(building: any) {
    this.BuildingSource.next(building);
  }

  getBuilding(): Observable<Building[][]> {
    return this.http.get(`${API}Building/GetAllBuilding`).pipe(
      map(
        (res: Building[]) => {
          const data = res.filter(x => x.buildingID !== 100);
          return _.chunk(data, 2);
        }
      ));
  }

  getBuildingOther(buildingId: number): Observable<Cell[][]> {
    return this.http.get(`${API}Cell/GetListCellByBuildingID?buildingID=${buildingId}`).pipe(map(
      (res: Cell[]) => {
        return _.chunk(res, 2);
      }
    ));
  }

  getListHistory(dateSearch: string, idBuilding: number, isCheck: number) {
    return this.http
      .post<InventoryLine[]>(`${API}HistoryInventory/GetListPlnoHistotry?dateSearch=${dateSearch}&idBuilding=${idBuilding}&isCheck=${isCheck}`, {});
  }

  GetListDetailHistoryInventory(plnoId: string, timeSoKiem: string, timePhucKiem: string, timeRutKiem: string, lang: string) {
    return this.http.post<ResultAllInventory>
      (`${API}HistoryInventory/GetListDetailHistoryInventory?plnoId=${plnoId}&timeSoKiem=${timeSoKiem}&timePhucKiem=${timePhucKiem}&timeRutKiem=${timeRutKiem}&lang=${lang}`, {}, { withCredentials: true });
  }

  exportPDF(reportHistory: ReportHistoryInventoryParams) {
    this._spinnerService.show();
    return this.http.post(`${API}ReportHistoryInventory/ExportFile`, reportHistory, { responseType: 'blob', withCredentials: true })
      .subscribe((result: Blob) => {
        if (result.type !== 'application/pdf') {
          alert(result.type);
        }
        const blob = new Blob([result]);
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        const currentTime = new Date();
        const filename =
          'CheckMachineReport-' +
          currentTime.getFullYear().toString() +
          (currentTime.getMonth() + 1) + '_' +
          currentTime.getDate() +
          currentTime
            .toLocaleTimeString()
            .replace(/[ ]|[,]|[:]/g, '')
            .trim() +
          '.pdf';
        link.href = url;
        link.setAttribute('download', filename);
        document.body.appendChild(link);
        link.click();
        this._spinnerService.hide();
      });
  }

  exportExcel(reportHistory: ReportHistoryInventoryParams) {
    this._spinnerService.show();
    return this.http.post(`${API}ReportHistoryInventory/ExportFile`, reportHistory, { responseType: 'blob', withCredentials: true })
      .subscribe((result: Blob) => {
        if (result.type !== 'application/xlsx') {
          alert(result.type);
        }
        const blob = new Blob([result]);
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        const currentTime = new Date();
        const filename =
          'CheckMachineReport-' +
          currentTime.getFullYear().toString() +
          (currentTime.getMonth() + 1) + '_' +
          currentTime.getDate() +
          currentTime
            .toLocaleTimeString()
            .replace(/[ ]|[,]|[:]/g, '')
            .trim() +
          '.xlsx';
        link.href = url;
        link.setAttribute('download', filename);
        document.body.appendChild(link);
        link.click();
        this._spinnerService.hide();
      });
  }


  exportExcelAllInventory(param: ReportKiemKeParam) {
    this._spinnerService.show();
    let params = new HttpParams().appendAll({ ...param })
    return this.http.get(`${API}ReportHistoryInventory/ExportAllInventory`, { responseType: 'blob', withCredentials: true, params: params })
      .subscribe((result: Blob) => {
        if (result.type !== 'application/xlsx') {
          alert(result.type);
        }
        const blob = new Blob([result]);
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        const currentTime = new Date();
        const filename =
          'Inventory_machine_Yearly_checking_' +
          currentTime.getFullYear().toString() + '_' +
          (currentTime.getMonth() + 1) + '_' +
          currentTime.getDate() + '_' +
          currentTime
            .toLocaleTimeString()
            .replace(/[ ]|[,]|[:]/g, '')
            .trim() +
          '.xlsx';
        link.href = url;
        link.setAttribute('download', filename);
        document.body.appendChild(link);
        link.click();
        this._spinnerService.hide();
      });
  }
}

