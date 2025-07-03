import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environments/environment";
import { eTM_Video } from "../../../../_models/production/T1/UPF/eTM_Video";

@Injectable({
  providedIn: "root",
})
export class ProductionT1UpfSafetyService {
  private apiUrl: string = environment.apiUrl;
  constructor(private http: HttpClient) {}

  getTodayData = (deptId: string) => {
    let params = new HttpParams().set("deptId", deptId);
    return this.http.get<eTM_Video[]>(
      this.apiUrl + "ProductionT1UPFSafety/GetTodayData",
      {
        params,
      }
    );
  };
}
