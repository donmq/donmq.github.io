import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { Users } from '@models/auth/users.model';
import { UserForLoginParam } from '@params/auth/user-for-login.param';
import { AuthService } from '@services/auth/auth.service';
import { firstValueFrom } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(
    private authService: AuthService,
    private router: Router) { }

  async canActivate(): Promise<boolean> {
    if (this.authService.loggedIn()) {
      return true;
    }
    else {
      const user: Users = JSON.parse(localStorage.getItem(LocalStorageConstants.USER));
      const ipLocal: string = localStorage.getItem(LocalStorageConstants.IPLOCAL);
      if (user && user.username && ipLocal) {
        const param: UserForLoginParam = <UserForLoginParam>{ username: user.username, ipLocal: ipLocal };
        await firstValueFrom(this.authService.loginExpired(param));
      }

      this.router.navigate(['/login']);
      return false;
    }
  }
}
