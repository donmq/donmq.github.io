import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { Roles, RolesDTO } from '../_models/roles';


const API = environment.apiUrl;
@Injectable({
  providedIn: 'root'
})
export class RolesService {

  constructor(
    private http: HttpClient,
  ) { }

  getAllRoles() {
    return this.http.get<RolesDTO[]>(`${API}Roles/GetAllRoles`);
  }

}
