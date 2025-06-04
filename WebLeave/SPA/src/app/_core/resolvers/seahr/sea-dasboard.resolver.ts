import { Injectable } from '@angular/core';
import { Resolve } from '@angular/router';
import { AuthService } from '@services/auth/auth.service';

@Injectable({
  providedIn: 'root',
})
export class SeaHrDataResolver implements Resolve<any> {
  constructor(private authService: AuthService) { }

  resolve(): Promise<any> {
    return this.authService.preloadData();
  }
}
