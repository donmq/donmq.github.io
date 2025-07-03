import { Injectable } from "@angular/core";
import { AuthService } from '../_core/_services/auth.service';
import { ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from "@angular/router";
import { Observable } from "rxjs";

@Injectable({
  providedIn: "root",
})
export class AuthGuard  {
  constructor(private authService: AuthService, private router: Router) { }
  // canActivate(): boolean {
  //   console.log("AuthGuard#canActivate 被觸發了");
  //   if (this.authService.loggedIn()) {
  //     return true;
  //   }
  //   this.router.navigate(["/login"]);
  //   return false;
  // }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): boolean {
    if (this.authService.loggedIn()) {
      return true;
    }
    this.router.navigate(["login"]);
    return false;
    // console.log("AuthGuard#canActivate 被觸發了");
    // return true;
  }
}
