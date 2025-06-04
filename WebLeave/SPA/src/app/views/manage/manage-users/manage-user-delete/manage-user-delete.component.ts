import { Component, OnInit } from '@angular/core';
import { UserManageService } from '@services/manage/user-manage.service';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-manage-user-delete',
  templateUrl: './manage-user-delete.component.html',
  styleUrls: ['./manage-user-delete.component.scss']
})
export class ManageUserDeleteComponent extends InjectBase implements OnInit {
  data: Array<{ userName: string, fullName: string, status: number }> = [];
  success_delete: number = 0;
  failed_delete: number = 0;
  not_found: number = 0;
  accept: string = '.xls, .xlsm, .xlsx';
  fileImportExcel: File = null;

  constructor(
    private userManageService: UserManageService,
  ) {
    super()
  }

  ngOnInit() {
  }

  uploadExcel() {
    if (this.fileImportExcel == null) {
      return this.snotifyService.warning(this.translateService.instant('System.Message.InvalidFile'), this.translateService.instant('System.Caption.Warning'));
    }
    this.snotifyService.confirm(this.translateService.instant('System.Message.ConfirmDeleteRangeMsg'), this.translateService.instant('System.Action.Delete'),
      () => {
        this.spinnerService.show();
        this.userManageService.uploadExcel(this.fileImportExcel).subscribe({
          next: (res) => {
            if (res.isSuccess) {
              this.data = res.data;
              this.success_delete = this.data.filter(x => x.status === 1).length;
              this.failed_delete = this.data.filter(x => x.status === 0).length;
              this.not_found = this.data.filter(x => x.status === -1).length;
              this.snotifyService.success(
                this.translateService.instant('System.Message.UploadOKMsg'),
                this.translateService.instant('System.Caption.Success')
              );
            }
            else {
              this.snotifyService.error(
                this.translateService.instant('System.Message.UploadErrorMsg'),
                this.translateService.instant('System.Caption.Error')
              );
            }
          }, error: () => {
            this.snotifyService.error(
              this.translateService.instant('System.Message.UnknowError'),
              this.translateService.instant('System.Caption.Error')
            );
          }
        }).add(() => { this.spinnerService.hide(); this.onRemoveFile(); });
      });
  }

  onSelectFile(event: any) {
    if (event.target.files && event.target.files[0]) {
      const fileNameExtension = event.target.files[0].name.split('.').pop();
      if (!this.accept.includes(fileNameExtension)) {
        this.onRemoveFile();
        return this.snotifyService.warning(this.translateService.instant('System.Message.AllowExcelFile'), this.translateService.instant('System.Caption.Warning'));
      }
      this.fileImportExcel = event.target.files[0];
    }
  }

  onRemoveFile() {
    (<HTMLInputElement>document.getElementById("input_uploadFile")).value = null;
    this.fileImportExcel = null;
  }

  getNameStatus(value: number) {
    switch (value) {
      case -1:
        return this.translateService.instant('Manage.UserManage.NotFound');
      case 0:
        return this.translateService.instant('Manage.UserManage.NotExpert');
      case 1:
        return this.translateService.instant('Manage.UserManage.DeleteSuccess');
      default:
        return null;
    }
  }

}
