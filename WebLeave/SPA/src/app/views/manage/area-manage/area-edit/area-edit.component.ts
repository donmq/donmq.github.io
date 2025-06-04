import { Component, OnInit } from '@angular/core';
import { Company } from '@models/common/company';
import { AreaInformation } from '@models/manage/area-manage/area-infomation';
import { AreaManageService } from '@services/manage/area-manage.service';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-area-edit',
  templateUrl: './area-edit.component.html',
  styleUrls: ['./area-edit.component.scss']
})
export class AreaEditComponent extends InjectBase implements OnInit {
  companys: Company[] = [];
  areaEdit: AreaInformation;
  areaEditConst: AreaInformation;
  constructor(
    private areaManageService: AreaManageService,
  ) {
    super();
  }

  ngOnInit() {
    this.getCompanys();
    this.areaManageService.areaSource.asObservable().subscribe(res => this.areaEdit = res);
    if (this.areaEdit == null) {
      this.router.navigate(['/manage/area'])
    }
    this.areaEditConst = { ...this.areaEdit };
  }

  getCompanys() {
    this.commonService.getCompany().subscribe({
      next: (res) => { this.companys = res },
      error: (err) => { this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error')); }
    });
  }

  saveEdit() {
    this.areaEdit.areaName = `${this.areaEdit.areaNameVi.trim()} - ${this.areaEdit.areaNameZh.trim()}`;
    this.areaManageService.editArea(this.areaEdit).subscribe({
      next: (res) => {
        if (res) {
          this.snotifyService.success(this.translateService.instant('System.Message.UpdateOKMsg'), this.translateService.instant('System.Caption.Success'));
        } else {
          this.snotifyService.error(this.translateService.instant('System.Message.UpdateErrorMsg'), this.translateService.instant('System.Caption.Error'));
        }
      },
      error: (err) => { this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error')); },
      complete: () => { this.router.navigate(['/manage/area']) }
    })
  }
  clear() {
    this.areaEdit = { ...this.areaEditConst };
  }
}
