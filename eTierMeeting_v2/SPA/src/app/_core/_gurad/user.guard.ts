import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../_services/auth.service';

@Injectable({ providedIn: 'root' })
export class UserGuard implements CanActivate {
    constructor(private _router: Router, private _authService: AuthService) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
        if (this._authService.loggedIn()) {
            return true
        }
        else {
            this._authService.urlBeforeRedit.next(state.url);
            this._router.navigate(["error", state.url.trim()]);
            return false
        }
    }
}