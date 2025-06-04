import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { UserManageService } from '@services/manage/user-manage.service';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Users } from '@models/manage/user-manage/Users';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-manage-user-edit',
  templateUrl: './manage-user-edit.component.html',
  styleUrls: ['./manage-user-edit.component.scss']
})
export class ManageUserEditComponent extends InjectBase implements OnInit {
  @Output() updateUsers = new EventEmitter();
  user: Users;
  editUserForm: UntypedFormGroup;

  constructor(
    public bsModalRef: BsModalRef,
    private fb: UntypedFormBuilder,
    private userManageService: UserManageService,
  ) {
    super()
  }

  ngOnInit() {
    this.updateUserForm();
  }

  updateUserForm() {
    this.editUserForm = this.fb.group({
      userName: [this.user.userName],
      fullName: [this.user.fullName, Validators.required],
      emailAddress: [this.user.emailAddress],
      hashPass: [''],
      empNumber: [this.user.employee?.empNumber],
      visible: [this.user.visible.toString()]
    });
  }
  updateUser() {
    this.user = Object.assign(this.user, this.editUserForm.value);

    this.spinnerService.show();
    this.userManageService.editUser(this.user).subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.snotifyService.success(this.translateService.instant('System.Message.UpdateOKMsg'),
            this.translateService.instant('System.Caption.Success'));
          this.updateUsers.emit();
          this.bsModalRef.hide();
        } else {
          this.snotifyService.error(this.translateService.instant('System.Message.UpdateErrorMsg'),
            this.translateService.instant('System.Caption.Error'));

        }
      },
      error: () => {
        this.snotifyService.error(
          this.translateService.instant('System.Message.UnknowError'),
          this.translateService.instant('System.Caption.Error')
        );
      }
    }).add(() => this.spinnerService.hide())
  }

  resetForm() {
    this.updateUserForm();
  }
}
