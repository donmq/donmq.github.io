import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { UserRoles } from '../_models/userRoles';


const API = environment.apiUrl;
@Injectable({
  providedIn: 'root'
})
export class UserRolesService {

  constructor(
    private http: HttpClient,
  ) { }


  getRoleByUser(userName: string) {
    return this.http.get<UserRoles[]>(`${API}UserRoles/GetRoleByUser?user=${userName}`);
  }

}
