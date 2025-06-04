
import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from "@angular/router";
import { catchError, Observable, of } from "rxjs";
import { Users } from "../../models/manage/user-manage/Users";
import { UserManageService } from "../../services/manage/user-manage.service";


@Injectable()
export class ManageUserRoleResolver implements Resolve<Users> {

  constructor(private router: Router, private userService: UserManageService) { }

  resolve(route: ActivatedRouteSnapshot): Observable<Users> {
    return this.userService.getUser(route.params['id']).pipe(
      catchError(error => {
        this.router.navigate(['/user']);
        return of();
      })
    )
  }
}
