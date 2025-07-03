import { HttpClient, HttpParams } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environments/environment";
import { DeliveryDTO } from "@models/deliveryDTO";

@Injectable({
  providedIn: "root",
})
export class DeliveryService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }

  getAllDelivery(deptId: string) {
    let params = new HttpParams().set("deptId", deptId);
    return this.http.get<DeliveryDTO[]>(this.baseUrl + "Delivery/getAllDelivery", { params });
  }
}
