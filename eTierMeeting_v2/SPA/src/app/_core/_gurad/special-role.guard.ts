import { Injectable, } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { UserRoles } from '../_models/userRoles';
import { RolesConst } from '../_constants/roles.constants';
import { AuthService } from '../_services/auth.service';

@Injectable({
  providedIn: 'root'
})
export class SpecialRoleGuard implements CanActivate {
  listUserRoles: UserRoles[] = [];

  constructor(
    private _router: Router,
    private _authService: AuthService
  ) { }

  roles: string;
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    if (this._authService.loggedIn()) {
      const user = JSON.parse(localStorage.getItem('user'));

      if (user.listRoles !== undefined) {
        this.listUserRoles = user.listRoles.filter(i => i.roles === RolesConst.kiemTraAnToanMayMoc);
      }

      if (this.listUserRoles.length > 0) {
        return true;
      }
      this._router.navigate(["error-403"]);
      return false;
    }
    else {
      this._authService.urlBeforeRedit.next(state.url);
      this._router.navigate(["error", state.url.trim()]);
      return false
    }


  }
}
