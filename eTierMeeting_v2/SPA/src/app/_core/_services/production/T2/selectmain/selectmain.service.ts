import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "@environments/environment";
import { ProductionT2SelectLineDTO } from "../../../../_models/productionT2SelectLineDTO";


@Injectable({
  providedIn: "root",
})
export class SelectmainService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {}

  GetListProductionT2CTB(){
    return this.http.get<ProductionT2SelectLineDTO>(this.baseUrl + 'ProductionT2CTBSection/GetListProductionT2CTB')
  }
}
