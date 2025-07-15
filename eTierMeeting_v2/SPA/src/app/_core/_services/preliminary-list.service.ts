import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { ColorString } from "highcharts";
import { NgxSpinnerService } from "ngx-spinner";
import { Observable } from "rxjs";
import { environment } from "../../../environments/environment";
import { OperationResult } from "../_models/operation-result";
import { Pagination, PaginationResult } from "../_models/pagination";
import { PreliminaryPlnoAdd } from "../_models/preliminary-add";
import { PreliminaryList } from "../_models/preliminary-list";

const API = environment.apiUrl;
@Injectable({
  providedIn: "root",
})
export class PreliminaryListService {
  constructor(
    private http: HttpClient,
    private _spinnerService: NgxSpinnerService
  ) {}

  getPreliminaryList(pagination: Pagination, search: string) {
    let params = new HttpParams();
    if (pagination.currentPage !== null && pagination.pageSize !== null) {
      params = params.append("pageNumber", pagination.currentPage.toString());
      params = params.append("pageSize", pagination.pageSize.toString());
    }
    return this.http.get<PaginationResult<PreliminaryList>>(
      `${API}PreliminaryPlno/GetAllPreliminaryPlno?search=` + search,
      { params }
    );
  }

  addPreliminary(preliminaryPlno: PreliminaryPlnoAdd, lang: string) {
    return this.http.post(
      `${API}PreliminaryPlno/AddPreliminaryPlno/` + lang,
      preliminaryPlno,
      {}
    );
  }

  updatePreliminary(preliminaryPlno: PreliminaryPlnoAdd, lang: string) {
    return this.http.post(
      `${API}PreliminaryPlno/UpdatePreliminaryPlno/` + lang,
      preliminaryPlno,
      {}
    );
  }
  removePreliminary(empNumber: string, lang: string) {
    return this.http.post(
      `${API}PreliminaryPlno/RemovePreliminaryPlno/` + empNumber + "/" + lang,
      {}
    );
  }
  getPreliminaryPlnos(empNumber: string) {
    return this.http.get<PreliminaryList>(
      `${API}PreliminaryPlno/GetPreliminaryPlnos?empNumber=${empNumber}`
    );
  }

  exportExcel(search: string) {
    return this.http
      .post(
        `${API}PreliminaryPlno/ExportExcel?search=` + search,
        {},
        { responseType: "blob", withCredentials: true }
      )
      .subscribe((result: Blob) => {
        if (result.type !== "application/xlsx") {
          alert(result.type);
        }
        const blob = new Blob([result]);
        const url = window.URL.createObjectURL(blob);
        const link = document.createElement("a");
        const currentTime = new Date();
        const filename =
          "PreliminaryList-" +
          currentTime.getFullYear().toString() +
          (currentTime.getMonth() + 1) +
          "_" +
          currentTime.getDate() +
          ".xlsx";
        link.href = url;
        link.setAttribute("download", filename);
        document.body.appendChild(link);
        link.click();
        this._spinnerService.hide();
      });
  }
}
