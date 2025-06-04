import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Users } from '@models/manage/user-manage/Users';
import { UserRoleManageService } from '@services/manage/user-role-manage.service';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-manage-role-report',
  templateUrl: './manage-role-report.component.html',
  styleUrls: ['./manage-role-report.component.scss']
})
export class ManageRoleReportComponent extends InjectBase implements OnInit {
  user: Users;
  viewreport: string;

  constructor(
    public bsModalRef: BsModalRef,
    private userRoleManageService: UserRoleManageService,
  ) {
    super()
  }

  ngOnInit() {
    this.setViewRole(this.user.roleReport);
  }

  SetReport(key: string) {
    this.spinnerService.show();
    this.userRoleManageService.setReport(this.user.userID, key).subscribe({
      next: (res) => {
        if (res.isSuccess)
          this.setViewRole(res.data);
      }, error: () => {
        this.snotifyService.error(
          this.translateService.instant('System.Message.UnknowError'),
          this.translateService.instant('System.Caption.Error')
        );
      }
    }).add(() => { this.spinnerService.hide() });
  }

  RemoveReport(key: string) {
    this.spinnerService.show();
    this.userRoleManageService.removeReport(this.user.userID, key).subscribe({
      next: (res) => {
        if (res.isSuccess)
          this.setViewRole(res.data);
      }, error: () => {
        this.snotifyService.error(
          this.translateService.instant('System.Message.UnknowError'),
          this.translateService.instant('System.Caption.Error')
        );
      }
    }).add(() => { this.spinnerService.hide() });
  }

  setViewRole(roleReport: number) {
    this.user.roleReport = roleReport;
    switch (roleReport) {
      case 0:
        this.viewreport = 'Nothing';
        break;
      case 1:
        this.viewreport = 'View Only';
        break;
      default:
        this.viewreport = 'Full Access';
        break;
    }
  }
}
