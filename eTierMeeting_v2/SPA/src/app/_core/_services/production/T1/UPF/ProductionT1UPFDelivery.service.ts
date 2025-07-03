import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@environments/environment';
import { VW_Production_T1_UPF_Delivery_RecordDTO } from '../../../../_models/production/T1/UPF/vW_Production_T1_UPF_Delivery_RecordDTO';

@Injectable({
  providedIn: 'root'
})
export class ProductionT1UPFDeliveryService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getAllDelivery(deptId: string) {
    let params = new HttpParams().set("deptId", deptId.toString());
    return this.http.get<VW_Production_T1_UPF_Delivery_RecordDTO[]>(this.baseUrl + "ProductionT1UPFDelivery/getAllDelivery", { params });
  }
}
