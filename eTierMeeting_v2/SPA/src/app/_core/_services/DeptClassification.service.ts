import { HttpHeaders, HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { BehaviorSubject, Observable, throwError } from "rxjs";
import { map, catchError } from "rxjs/operators";
import { environment } from "../../../environments/environment";
import { PaginatedResult } from "../_models/pagination";

import { DeptClassification } from "../_models/deptclassification";
import { ProductionT1SelectLine } from "../_models/production-t1-select-line";
import { ProductionT2SelectLineDTO } from "../_models/productionT2SelectLineDTO";

@Injectable({
  providedIn: "root",
})
export class DeptClassificationService {
  baseUrl = environment.apiUrl;
  classificationSource = new BehaviorSubject<Object>({});
  currentclassification = this.classificationSource.asObservable();
  flagSource = new BehaviorSubject<string>("0");
  currentFlag = this.flagSource.asObservable();

  constructor(private http: HttpClient) { }

  // 搜尋
  search(
    page?,
    itemsPerPage?,
    paramSearch?: object
  ): Observable<PaginatedResult<DeptClassification[]>> {
    const paginatedResult: PaginatedResult<DeptClassification[]> =
      new PaginatedResult<DeptClassification[]>();
    let params = new HttpParams();
    if (page != null && itemsPerPage != null) {
      params = params.append("pageNumber", page);
      params = params.append("pageSize", itemsPerPage);
    }
    let url = this.baseUrl + "deptclassification/search";
    return this.http
      .post<any>(url, paramSearch, { observe: "response", params })
      .pipe(
        map((response) => {
          paginatedResult.result = response.body;
          if (response.headers.get("Pagination") != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get("Pagination")
            );
          }
          return paginatedResult;
        })
      );
  }

  // 線別下拉選單
  getDept() {
    return this.http.get<any>(this.baseUrl + "DeptClassification/getdept");
  }

  getDeptForSelectLine() {
    return this.http.get<ProductionT2SelectLineDTO>(this.baseUrl + "DeptClassification/getdeptforselectline");
  }

  // private handleError(error: HttpErrorResponse) {
  //   if (error.error instanceof ErrorEvent) {
  //     // A client-side or network error occurred. Handle it accordingly.
  //     console.error('An error occurred:', error.error.message);
  //   } else {
  //     // The backend returned an unsuccessful response code.
  //     // The response body may contain clues as to what went wrong,
  //     console.error(
  //       `Backend returned code ${error.status}, ` +
  //       `body was: ${error.error}`);
  //   }
  //   // return an ErrorObservable with a user-facing error message
  //   return new ErrorObservable(
  //     'Something bad happened; please try again later.');
  // };

  // 新增部門類別
  createDeptClassification(deptClassification: DeptClassification) {
    return this.http.post(
      this.baseUrl + "DeptClassification/addDeptClassification/",
      {
        Class_Kind: deptClassification.class_Kind,
        Dept_ID: deptClassification.dept_ID,
        Dept_Name: deptClassification.dept_Name,
        Class_Name: deptClassification.class_Name,
      }
    );
  }

  // 修改部門類別
  updateDeptClassification(deptClassification: DeptClassification) {
    return this.http.post(
      this.baseUrl + "DeptClassification/updateDeptClassification/",
      {
        Class_Kind: deptClassification.class_Kind,
        Dept_ID: deptClassification.dept_ID,
        Dept_Name: deptClassification.dept_Name,
        Class_Name: deptClassification.class_Name,
        Insert_At: deptClassification.insert_At,
        Insert_By: deptClassification.insert_By,
      }
    );
  }

  // 刪除部門類別
  deleteClassification(deptClassification: DeptClassification) {
    return this.http.post(
      this.baseUrl + "DeptClassification/deleteClassification/",
      deptClassification
    );
  }

  // 切換狀態(新增修改)
  changeDeptClassification(deptClassification: DeptClassification) {
    this.classificationSource.next(deptClassification);
  }
  changeFlag(flag: string) {
    this.flagSource.next(flag);
  }
}
