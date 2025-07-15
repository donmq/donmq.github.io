import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { environment } from '../../../environments/environment';
import { InventoryParams } from '../_dtos/inventory-params';
import { Building } from '../_models/building';
import { Cell } from '../_models/cell';
import { InventoryLine } from '../_models/inventoryLine';
import { SearchMachineInventory } from '../_models/search-machine-inventory';
import * as _ from 'lodash';
import { NgxSpinnerService } from 'ngx-spinner';
import { Hpa15 } from '../_models/hp-a15';
import { ListHpa15 } from '../_models/preliminary-list';

const API = environment.apiUrl;

@Injectable({
  providedIn: 'root'
})
export class InventoryService {
  optionsInventory = new BehaviorSubject<number>(0);
  codeName = new BehaviorSubject<string>('');
  listPlno = new BehaviorSubject<ListHpa15[]>(null);
  positionInventory = new BehaviorSubject<InventoryLine>(null);

  constructor(private http: HttpClient,
    private _spinnerService: NgxSpinnerService) { }

  getMachine(machineCode: string, lang: string) {
    return this.http.get<SearchMachineInventory>(`${API}Inventory/GetMachine?idMachine=${machineCode}&lang=${lang}`, {});
  }

  submitInventory(inventoryParams: InventoryParams) {
    return this.http.post<InventoryParams>(`${API}Inventory/SubmitInventory`, inventoryParams);
  }

  getOptionInvetory(options: number) {
    this.optionsInventory.next(options);
  }

  getCodeName(code: string) {
    this.codeName.next(code);
  }

  getPlno(listPlno: ListHpa15[]) {
    this.listPlno.next(listPlno);
  }

  getPositionInventory(item: InventoryLine) {
    this.positionInventory.next(item);
  }

  getCellInevntory(buildingID: string, checkGetData: string): Observable<InventoryLine[]> {
    return this.http.get(`${API}CellPlno/GetListPlnoByBuildingInventory?id=${buildingID}&checkGetData=${checkGetData}`).pipe(map(
      (res: InventoryLine[]) => {
        return res;
      }
    ));
  }

  getBuilding(): Observable<Building[]> {
    return this.http.get(`${API}Building/GetAllBuilding`).pipe(
      map(
        (res: Building[]) => {
          const data = res.filter(x => x.buildingID !== 100);
          return data;
        }
      ));
  }

  getBuildingOther(buildingId: number): Observable<Cell[]> {
    return this.http.get<Cell[]>(`${API}Cell/GetListCellByBuildingID?buildingID=${buildingId}`);
  }

  exportPDF(inventoryParams: InventoryParams) {
    this._spinnerService.show();
    return this.http.post(`${API}Inventory/ExportFile`, inventoryParams, { responseType: 'blob', withCredentials: true })
      .subscribe((result: Blob) => {
        if (result.type !== 'application/pdf') {
          alert(result.type);
        }
        const blob = new Blob([result]);
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        const currentTime = new Date();
        const filename =
          'ExportInventory-' +
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

  exportExcel(inventoryParams: InventoryParams) {
    this._spinnerService.show();
    return this.http.post(`${API}Inventory/ExportFile`, inventoryParams, { responseType: 'blob', withCredentials: true })
      .subscribe((result: Blob) => {
        if (result.type !== 'application/xlsx') {
          alert(result.type);
        }
        const blob = new Blob([result]);
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement('a');
        const currentTime = new Date();
        const filename =
          'ExportInventory-' +
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
}
