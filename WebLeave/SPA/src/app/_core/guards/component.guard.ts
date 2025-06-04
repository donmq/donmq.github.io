import { Injectable } from '@angular/core';
import { CanLoad, Route, Router, UrlTree } from '@angular/router';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ComponentGuard implements CanLoad {
  constructor(private router: Router) { }
  canLoad(route: Route): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    const user: any = JSON.parse(localStorage.getItem(LocalStorageConstants.USER));
    let role = route.data['role'];
    let isRoleValid: boolean = user?.roles?.some(x => x.subRoles?.some(y => y.roleSym.includes(role)));
    if (isRoleValid) {
      return true;
    } else {
      this.router.navigate(['/dashboard']);
      return false;
    }
  }
}
