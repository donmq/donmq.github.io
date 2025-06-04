import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot } from '@angular/router';
import { Observable, forkJoin } from 'rxjs';
import { SeaConfirmService } from '../../services/seahr/sea-confirm.service';
import { SeaConfirmResolverDto } from '@models/seahr/sea-confirm';
import { CommonService } from '@services/common/common.service';

@Injectable({ providedIn: 'root' })
export class SeaConfirmResolver implements Resolve<SeaConfirmResolverDto> {

  constructor(
    private seaConfirmService: SeaConfirmService,
    private commonService: CommonService) { }

  resolve(_route: ActivatedRouteSnapshot): Observable<any> {
    const category$ = this.seaConfirmService.getCategories();
    const department$ = this.seaConfirmService.getDepartments();
    const commentArchives$ = this.commonService.getCommentArchives();
    return forkJoin({ category_List: category$, department_List: department$, comment_List: commentArchives$ });
  }
}
