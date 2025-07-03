import { Injectable } from '@angular/core';
import { Route, Router } from '@angular/router';
import { LocalStorageConstants } from '@constants/storage.constants';

@Injectable({ providedIn: 'root' })
export class EfficiencyGuard  {
    constructor(private router: Router) { }

    canLoad(route: Route) {
        const user = JSON.parse(localStorage.getItem(LocalStorageConstants.USER));
        if (user != null) {
            return true;
        } else {
            this.router.navigate(['/dashboard']);
            return false;
        }
    }
}
