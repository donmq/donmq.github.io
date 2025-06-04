import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable } from 'rxjs';
import { LeaveData } from '../../models/common/leave-data';
import { SeaEditLeaveService } from '../../services/seahr/sea-edit-leave.service';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';

@Injectable({
  providedIn: 'root'
})
export class SeaEditLeaveResolver implements Resolve<PaginationResult<LeaveData>> {
  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 20
  };

  constructor(private seaEditLeaveService: SeaEditLeaveService) { }

  resolve(route: ActivatedRouteSnapshot): Observable<PaginationResult<LeaveData>> {
    return this.seaEditLeaveService.getAllEditLeave(this.pagination);
  }
}
