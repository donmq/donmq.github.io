import { Component, OnInit } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Users } from '@models/manage/user-manage/Users';
import { UserRoleManageService } from '@services/manage/user-role-manage.service';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-manage-role-admin',
  templateUrl: './manage-role-admin.component.html',
  styleUrls: ['./manage-role-admin.component.scss']
})
export class ManageRoleAdminComponent extends InjectBase implements OnInit {
  user: Users;
  viewrole: string;

  constructor(
    public bsModalRef: BsModalRef,
    private userRoleManageService: UserRoleManageService,
  ) {
    super()
  }

  ngOnInit() {
    this.setViewRole(this.user.isPermitted, this.user.rolePermitted);
  }

  SetPermit(key: string) {
    this.spinnerService.show();
    this.userRoleManageService.setPermit(this.user.userID, key).subscribe({
      next: (res: any) => {
        if (res.success)
          this.setViewRole(true, res.data);
      }, error: () => {
        this.snotifyService.error(
          this.translateService.instant('System.Message.UnknowError'),
          this.translateService.instant('System.Caption.Error')
        );
      }
    }).add(() => { this.spinnerService.hide() });
  }

  RemovePermit(key: string) {
    this.spinnerService.show();
    this.userRoleManageService.removePermit(this.user.userID, key).subscribe({
      next: (res: any) => {
        if (res.success)
          this.setViewRole(key == 'all' ? false : true, res.data);
      }, error: () => {
        this.snotifyService.error(
          this.translateService.instant('System.Message.UnknowError'),
          this.translateService.instant('System.Caption.Error')
        );
      }
    }).add(() => { this.spinnerService.hide() });
  }

  setViewRole(isPermitted: boolean, rolePermitted: number) {
    this.user.rolePermitted = rolePermitted;
    this.user.isPermitted = isPermitted;
    if (isPermitted) {
      if (rolePermitted == 1)
        this.viewrole = 'Moderator';
      else
        this.viewrole = 'View Only';
    }
    else
      this.viewrole = 'Nothing';
  }
}
