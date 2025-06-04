import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { UserManageService } from '@services/manage/user-manage.service';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { Users } from '@models/manage/user-manage/Users';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-manage-user-add',
  templateUrl: './manage-user-add.component.html',
  styleUrls: ['./manage-user-add.component.css']
})
export class ManageUserAddComponent extends InjectBase implements OnInit {
  @Output() addUsers = new EventEmitter();
  user: Users;
  userRank = [
    { value: '1', display: 'ViewOnly' },
    { value: '2', display: 'ApplyOnly' },
    { value: '3', display: 'Approval' },
    { value: '6', display: 'ViewSEAHR' },
    { value: '4', display: 'SEA/HR' },
    { value: '5', display: 'FullAccess' }
  ];
  addUserForm: UntypedFormGroup;

  constructor(
    public bsModalRef: BsModalRef,
    private fb: UntypedFormBuilder,
    private userManageService: UserManageService,
  ) {
    super()
  }

  ngOnInit() {
    this.createUserForm();
  }

  createUserForm() {
    this.addUserForm = this.fb.group({
      userName: ['', Validators.required],
      fullName: ['', Validators.required],
      emailAddress: [''],
      userRank: [null, Validators.required],
      hashPass: [''],
      empNumber: [''],
      iSPermitted: ['true']
    });
  }

  addUser() {
    this.user = Object.assign({}, this.addUserForm.value);
    this.spinnerService.show();
    this.userManageService.addUser(this.user).subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.snotifyService.success(this.translateService.instant('System.Message.CreateOKMsg'),
            this.translateService.instant('System.Caption.Success'));
          this.addUsers.emit(this.user);
          this.bsModalRef.hide();
        } else {
          this.snotifyService.error(this.translateService.instant('System.Message.CreateErrorMsg'),
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
    this.createUserForm();
  }
}
