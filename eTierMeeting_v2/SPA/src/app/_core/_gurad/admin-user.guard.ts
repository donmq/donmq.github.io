import { Injectable, } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { UserRoles } from '../_models/userRoles';
import { RolesConst } from '../_constants/roles.constants';

@Injectable({
  providedIn: 'root'
})
export class AdminUserGuard implements CanActivate {
  listUserRoles: UserRoles[] = [];

  constructor(
    private _router: Router,
  ) { }

  roles: string;
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    const user = JSON.parse(localStorage.getItem('user'));

    if (user.listRoles !== undefined) {
      this.listUserRoles = user.listRoles.filter(i => i.roles === RolesConst.userManager);
    }

    if (this.listUserRoles.length > 0) {
      return true;
    }
    this._router.navigate(['/admin/erroradmin']);
    return false;

  }
}
