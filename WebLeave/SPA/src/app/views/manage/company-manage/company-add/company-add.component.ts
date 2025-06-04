import { Component, OnInit } from '@angular/core';
import { CompanyManage } from '@models/manage/company-manage/company-manage';
import { CompanyManageService } from '@services/manage/company-manage.service';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-company-add',
  templateUrl: './company-add.component.html',
  styleUrls: ['./company-add.component.scss']
})
export class CompanyAddComponent extends InjectBase implements OnInit {
  companyManage: CompanyManage = {} as CompanyManage;
  constructor(
    private companyManageService: CompanyManageService,
  ) {
    super();
  }

  ngOnInit() {
  }
  addCompany(model: CompanyManage) {
    this.spinnerService.show();
    this.companyManageService.addCompany(model)
      .subscribe({
        next: (res) => {
          this.spinnerService.hide();
          if (res.isSuccess) {
            this.cancel();
            this.snotifyService.success(this.translateService.instant('System.Message.CreateOKMsg'), this.translateService.instant('System.Caption.Success'));
          } else {
            this.snotifyService.error(this.translateService.instant('System.Message.CreateErrorMsg'), this.translateService.instant('System.Caption.Error'));
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
