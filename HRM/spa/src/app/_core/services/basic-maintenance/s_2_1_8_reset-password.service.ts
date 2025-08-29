import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { OperationResult } from '@utilities/operation-result';
import { Observable } from 'rxjs';
import { ResetPasswordParam } from '@models/basic-maintenance/2_1_8_reset-password';
import { IClearCache } from '@services/cache.service';

@Injectable({
  providedIn: 'root',
})
export class S_2_1_8_resetPasswordService implements IClearCache {
  apiUrl: string = environment.apiUrl + 'C_2_1_8_ResetPassword';
  constructor(private http: HttpClient) {}
  clearParams: () => void;

  resetPassword(param: ResetPasswordParam): Observable<OperationResult> {
    return this.http.put<OperationResult>(this.apiUrl + '/ResetPassword', param);
  }
}
