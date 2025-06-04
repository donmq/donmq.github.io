import { Component, OnInit } from '@angular/core';
import { Company } from '@models/common/company';
import { AreaInformation } from '@models/manage/area-manage/area-infomation';
import { AreaManageService } from '@services/manage/area-manage.service';
import { InjectBase } from '@utilities/inject-base-app';
@Component({
  selector: 'app-area-add',
  templateUrl: './area-add.component.html',
  styleUrls: ['./area-add.component.scss']
})
export class AreaAddComponent extends InjectBase implements OnInit {
  area: AreaInformation = <AreaInformation>{
    visible: null
  };
  companys: Company[] = [];
  constructor(
    private areaManageService: AreaManageService,
  ) {
    super();
  }

  ngOnInit() {
    this.getCompanys();
  }

  getCompanys() {
    this.commonService.getCompany().subscribe({
      next: (res) => {
        this.companys = res;
        this.area.companyID = this.companys[0].companyID;
      },
      error: (err) => { this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error')); }
    });
  }

  clear() {
    this.area = <AreaInformation>{
      visible: null,
      companyID: this.companys[0].companyID
    }
  }

  saveAdd() {
    this.area.areaName = `${this.area.areaNameVi.trim()} - ${this.area.areaNameZh.trim()}`;
    this.areaManageService.addArea(this.area).subscribe({
      next: (res) => {
        if (res) {
          this.snotifyService.success(this.translateService.instant('System.Message.CreateOKMsg'), this.translateService.instant('System.Caption.Success'));
        } else {
          this.snotifyService.error(this.translateService.instant('System.Message.CreateErrorMsg'), this.translateService.instant('System.Caption.Error'));
        }
      },
      error: (err) => { this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error')); },
      complete: () => { this.router.navigate(['/manage/area']); }
    })
  }
}
