import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, Params } from '@angular/router';
import { LeaveDetail } from '@models/common/leave-data';
import { Observable } from 'rxjs';
import { LeaveDetailService } from '../../services/leave/leave-detail.service';

@Injectable({ providedIn: 'root' })
export class LeaveDetailResolver implements Resolve<LeaveDetail> {

  constructor(private leaveDetailService: LeaveDetailService) { }

  resolve(route: ActivatedRouteSnapshot): Observable<LeaveDetail> | Promise<LeaveDetail> | LeaveDetail {
    const params: Params = route.params;
    return this.leaveDetailService.getLeaveDetail(params['leaveID']);
  }
}