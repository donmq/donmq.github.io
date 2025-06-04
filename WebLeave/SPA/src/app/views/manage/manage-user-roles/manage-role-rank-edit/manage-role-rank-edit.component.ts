import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup } from '@angular/forms';
import { UserRoleManageService } from '@services/manage/user-role-manage.service';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Users } from '@models/manage/user-manage/Users';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-manage-role-rank-edit',
  templateUrl: './manage-role-rank-edit.component.html',
  styleUrls: ['./manage-role-rank-edit.component.scss']
})
export class ManageRoleRankEditComponent extends InjectBase implements OnInit {
  @Output() updateRoles = new EventEmitter();
  user: Users;
  roleName: string;
  editRoleRank: UntypedFormGroup;

  constructor(
    public bsModalRef: BsModalRef,
    private fb: UntypedFormBuilder,
    private userRoleManageService: UserRoleManageService,
  ) {
    super()
  }

  ngOnInit() {
    this.updateRoleForm();
  }

  updateRoleForm() {
    this.editRoleRank = this.fb.group({
      roleName: [this.roleName],
      roleRank: [this.user.userRank.toString()],
      isInherit: [false]
    });
  }
  updateRoleRank() {
    if (this.editRoleRank.value.roleRank == this.user.userRank) {
      this.snotifyService.warning(this.translateService.instant('System.Message.NoDataSelected'), this.translateService.instant('System.Caption.Warning'));
    }
    else {
      this.spinnerService.show();
      this.userRoleManageService.updateRoleRank(this.user.userID, this.editRoleRank.value.roleRank, this.editRoleRank.value.isInherit)
        .subscribe({
          next: (res) => {
            if (res.isSuccess) {
              this.updateRoles.emit(true);
              this.bsModalRef.hide();
              this.snotifyService.success(
                this.translateService.instant('System.Message.UpdateOKMsg'),
                this.translateService.instant('System.Caption.Success'));
            }
            else {
              this.snotifyService.error(
                this.translateService.instant('System.Message.UpdateErrorMsg'),
                this.translateService.instant('System.Caption.Error'));
            }
          }, error: () => {
            this.snotifyService.error(
              this.translateService.instant('System.Message.UnknowError'),
              this.translateService.instant('System.Caption.Error')
            );
            this.spinnerService.hide();
          }
        })
    }
  }

  getRoleGroup(userRank: string) {
    switch (userRank) {
      case '1':
        return this.translateService.instant('Manage.UserManage.ViewOnly');
      case '2':
        return this.translateService.instant('Manage.UserManage.ApplyOnly');
      case '3':
        return this.translateService.instant('Manage.UserManage.Approval');
      case '6':
        return this.translateService.instant('Manage.UserManage.ViewSEAHR');
      case '4':
        return "SEA/HR";
      case '5':
        return this.translateService.instant('Manage.UserManage.FullAccess');
      default:
        return null;
    }
  }

  resetForm() {
    this.updateRoleForm();
  }
}
