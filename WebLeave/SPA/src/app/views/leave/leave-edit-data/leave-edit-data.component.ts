import { Component, OnInit } from '@angular/core';
import { Detail } from '@models/leave/detail';
import { LeaveData } from '@models/leave/leaveDataViewModel';
import { LeaveEditDataService } from '@services/leave/leaveEditData.service';
import { Pagination } from '@utilities/pagination-utility';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { DestroyService } from '@services/destroy.service';
import { takeUntil } from 'rxjs';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-leave-edit-data',
  templateUrl: './leave-edit-data.component.html',
  styleUrls: ['./leave-edit-data.component.scss'],
  providers: [DestroyService]
})
export class LeaveEditDataComponent extends InjectBase implements OnInit {
  listEditLeave: LeaveData[] = [];
  detailEmployee: Detail = <Detail>{};
  lang: string = 'vi';
  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 20,
  }

  constructor(private service: LeaveEditDataService,
    ) {
      super();
    }

  ngOnInit(): void {
    this.getAllEditLeave();
    this.lang = localStorage.getItem(LocalStorageConstants.LANG);
    this.translateService.onLangChange.pipe(takeUntil(this.destroyService.destroys$)).subscribe((event) => {
      this.lang = event.lang;
    })
  }

  getAllEditLeave() {
    this.spinnerService.show();
    this.service.getAllEditLeave(this.pagination).subscribe({
      next: (res) => {
        this.spinnerService.hide();
        this.listEditLeave = res.result;
        this.pagination = res.pagination;
      },
      error: () => {
        this.spinnerService.hide();
        this.snotifyService.error(
          this.translateService.instant('System.Message.UnknowError'),
          this.translateService.instant('System.Caption.Error')
        );
      }
    })
  }
  pageChanged(event: any) {
    this.pagination.pageNumber = event.page;
    this.getAllEditLeave();
  }
  viewDetail(item: LeaveData) {
    this.service.paramLeaveDataEdit(item);
    this.router.navigate(['leave/requestleavedata']);
  }
  back() {
    this.router.navigate(['leave']);
  }
}
