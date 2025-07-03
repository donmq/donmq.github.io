import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';
import { LocalStorageConstants } from '@constants/storage.constants';

@Injectable({ providedIn: 'root' })
export class PageInitGuard  {
  constructor(private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    const center_Level: string = route?.data?.center_Level;
    const tier_Level: string = route?.data?.tier_Level;

    if (!center_Level && !tier_Level) {
      this.router.navigate(['/dashboard']);
      return false;
    }

    localStorage.setItem(LocalStorageConstants.CENTER_LEVEL, center_Level);
    localStorage.setItem(LocalStorageConstants.TIER_LEVEL, tier_Level);
    return true;
  }
}
