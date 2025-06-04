import { Router } from '@angular/router';
import { CommonConstants } from '@constants/common.constants';
import { LocalStorageConstants } from '@constants/local-storage.enum'
import { Users } from '@models/auth/users.model';
import { BrowserInfo } from '@models/common/browser-info'
import { UserForLoginParam } from '@params/auth/user-for-login.param';
import { AuthService } from '@services/auth/auth.service';
import { CommonService } from '@services/common/common.service'
import { firstValueFrom, tap } from 'rxjs';

export function appInitializer(commonService: CommonService, authService: AuthService, router: Router) {
  if (window.location.href.includes("no-auth"))
    return () => new Promise(resolve => { resolve(true) });

  return () => new Promise(async resolve => {
    const user: Users = JSON.parse(localStorage.getItem(LocalStorageConstants.USER));
    const ipLocal: string = localStorage.getItem(LocalStorageConstants.IPLOCAL);
    await firstValueFrom(commonService.getBrowserInfo(ipLocal ?? '', user?.username ?? '').pipe(
      tap(async (res: BrowserInfo) => {
          if (res?.ipLocal !== res?.loginDetect?.loggedByIP && user?.username !== CommonConstants.USER_ADMINISTRATOR) {
            const param: UserForLoginParam = <UserForLoginParam>{ username: user?.username, ipLocal: res?.ipLocal };
            await firstValueFrom(authService.logout(param, false));
          }
          else if (res?.loginDetect?.expires && new Date(res?.loginDetect?.expires) < new Date()) {
            const param: UserForLoginParam = <UserForLoginParam>{ username: user?.username, ipLocal: res?.ipLocal };
            await firstValueFrom(authService.loginExpired(param, false));
        }

        localStorage.setItem(LocalStorageConstants.FACTORY, res.factory);
        resolve(localStorage.setItem(LocalStorageConstants.IPLOCAL, res.ipLocal));
      })
    ));
  })
}
