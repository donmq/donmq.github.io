import { Injectable } from '@angular/core';
import { CanLoad, Route, Router, UrlTree } from '@angular/router';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class DashboardGuard implements CanLoad {
  user: any
  constructor(private router: Router) { }
  canLoad(route: Route): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    this.user = JSON.parse(localStorage.getItem(LocalStorageConstants.USER));
    let role = route.data['role'];
    let isRoleValid: boolean = this.user?.roles?.some(x => x.roleSym.includes(role));
    if (isRoleValid) {
      return true;
    } else {
      this.router.navigate(['/dashboard']);
      return false;
    }
  }
}
