import { Component, OnInit } from '@angular/core';
import { DestroyService } from '@services/destroy.service';
import { firstValueFrom, takeUntil, tap } from 'rxjs';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { Detail } from '@models/leave/detail';
import { LeaveEditDataService } from '@services/leave/leaveEditData.service';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-leave-edit-data-detail',
  templateUrl: './leave-edit-data-detail.component.html',
  styleUrls: ['./leave-edit-data-detail.component.scss'],
  providers: [DestroyService],
})
export class LeaveEditDataDetailComponent extends InjectBase implements OnInit {
  detailEmployee: Detail = <Detail>{};
  empID: number;
  leavID: number;
  lang: string = 'vi';
  listComment: string[] = []

  constructor(
    private service: LeaveEditDataService,
  ) {
    super();
  }

  async ngOnInit() {
    await this.getData();
    await this.getDetailEmploy();
    this.lang = localStorage.getItem(LocalStorageConstants.LANG);
    this.translateService.onLangChange.pipe(takeUntil(this.destroyService.destroys$)).subscribe((event) => {
      this.lang = event.lang;
      this.getDetailEmploy();
    });
  }

  getData() {
    this.service.curentLeaveDataSource.subscribe({
      next: (res) => {
        this.empID = res.empID;
        this.leavID = res.leaveID;
      },
    });
  }

  getLeaveById(id: number) {
    this.service.getDetailById(id).subscribe({
      next: (item) => {
        this.detailEmployee.listLeave = item;
      },
    });
  }

  confirmEditLeave() {
    this.spinnerService.show();
    this.service.editLeave(this.leavID).subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.getLeaveById(this.leavID);
          this.snotifyService.success(this.translateService.instant('System.Message.UpdateOKMsg'), this.translateService.instant('System.Caption.Success'));
        } else {
          this.snotifyService.error(this.translateService.instant('System.Message.UpdateErrorMsg'), this.translateService.instant('System.Caption.Error'));
        }
        this.spinnerService.hide();
      },
      error: () => {
        this.spinnerService.hide();
        this.snotifyService.error(
          this.translateService.instant('System.Message.UnknowError'),
          this.translateService.instant('System.Caption.Error')
        );
      },
      complete: () => {
        this.spinnerService.hide();
      }
    });
  }

  async getDetailEmploy() {
    this.detailEmployee = await firstValueFrom(
      this.service.getDetailEmployy(this.leavID).pipe(tap(
        async (res) => {
          if (res) {
            res.listLeave.comment = await this.functionUtility.changeCommentLanguage(res.listLeave.comment);
            this.listComment = this.detailEmployee.listLeave.comment.split('-').map(x => x.includes(',') ? x.replace(',', ' -') : x);
          } else {
            this.back();
          }
        },
      ))
    )
  }
  back() {
    this.router.navigate(['leave/edit']);
  }
}
