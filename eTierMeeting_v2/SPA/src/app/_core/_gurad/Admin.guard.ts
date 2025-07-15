import { Injectable, } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, Router, CanActivateChild } from '@angular/router';
import { Observable } from 'rxjs';

import { UserRoles } from '../_models/userRoles';
import { RolesConst } from '../_constants/roles.constants';


@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivateChild {
  listUserRoles: UserRoles[] = [];

  constructor(
    private router: Router,
  ) { }
  role: string;
  canActivateChild(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    const user = JSON.parse(localStorage.getItem('user'));

    if (user.listRoles != undefined) {
      this.listUserRoles = user.listRoles.filter(i => i.roles === RolesConst.admin);
    }
    if (this.listUserRoles.length > 0) {
      return true;
    }
    this.router.navigate(["error-403"]);
    return false;

  }
}
