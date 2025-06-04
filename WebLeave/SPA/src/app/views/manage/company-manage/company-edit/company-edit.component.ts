import { Component, OnInit } from '@angular/core';
import { CompanyManage } from '@models/manage/company-manage/company-manage';
import { CompanyManageService } from '@services/manage/company-manage.service';
import { DestroyService } from '@services/destroy.service';
import { takeUntil } from 'rxjs';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-company-edit',
  templateUrl: './company-edit.component.html',
  styleUrls: ['./company-edit.component.scss'],
  providers: [DestroyService]
})
export class CompanyEditComponent extends InjectBase implements OnInit {
  editOrDetail: boolean;
  companyManage: CompanyManage = {} as CompanyManage;
  constructor(
    private companyManageService: CompanyManageService,

  ) {
    super();
  }

  ngOnInit() {
    this.spinnerService.show();
    if (this.route.snapshot.params['edit'] === 'edit') {
      this.editOrDetail = true;
    } else {
      this.editOrDetail = false;
    };
    this.companyManageService.currentModel
      .pipe(takeUntil(this.destroyService.destroys$))
      .subscribe(res => {
        if (res !== null) {
          this.spinnerService.hide();
          this.companyManage = res;
        } else {
          this.spinnerService.hide();
          this.router.navigate(['manage/company']);
        }
      }).unsubscribe();
  }
  editCompany(model: CompanyManage) {
    this.spinnerService.show();
    this.companyManageService.editCompany(model)
      .subscribe({
        next: (res) => {
          this.spinnerService.hide();
          if (res.isSuccess) {
            this.cancel();
            this.snotifyService.success(this.translateService.instant('System.Message.UpdateOKMsg'), this.translateService.instant('System.Caption.Success'));
          } else {
            this.snotifyService.error(this.translateService.instant('System.Message.UpdateErrorMsg'), this.translateService.instant('System.Caption.Error'));
          }
        },
        error: () => {
          this.spinnerService.hide();
          this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
        }
      });
  }
  cancel() {
    this.router.navigate(['manage/company']);
  }
}
