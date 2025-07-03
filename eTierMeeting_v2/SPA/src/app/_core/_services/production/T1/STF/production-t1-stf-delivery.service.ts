import { Observable } from 'rxjs';
import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environments/environment";
import { VW_Production_T1_STF_Delivery_Recor } from '@models/production/T1/STF/vW_Production_T1_STF_Delivery_Record';

@Injectable({
  providedIn: "root",
})
export class ProductionT1STFDeliveryService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getAllDelivery(deptId: string): Observable<VW_Production_T1_STF_Delivery_Recor[]> {
    let params = new HttpParams().set("deptId", deptId);
    return this.http.get<VW_Production_T1_STF_Delivery_Recor[]>(this.baseUrl + "ProductionT1STFDelivery/getAllDelivery", { params });
  }
}
