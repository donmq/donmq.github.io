import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CompanyManage } from '@models/manage/company-manage/company-manage';
import { CompanyManageService } from '@services/manage/company-manage.service';
import { DestroyService } from '@services/destroy.service';
import { takeUntil } from 'rxjs';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-company-main',
  templateUrl: './company-main.component.html',
  styleUrls: ['./company-main.component.scss'],
  providers: [DestroyService]
})
export class CompanyMainComponent extends InjectBase implements OnInit {
  listCompanyManage: CompanyManage[] = [];
  detailCompanyManage: CompanyManage = {} as CompanyManage;
  modalRef?: BsModalRef;
  constructor(
    private companyManageService: CompanyManageService,
    private modalService: BsModalService,
  ) {
    super();
  }

  ngOnInit() {
    this.getAllCompany();
  }
  getAllCompany() {
    this.spinnerService.show();
    this.companyManageService.getAllCompany()
      .pipe(takeUntil(this.destroyService.destroys$))
      .subscribe({
        next: (data) => {
          this.listCompanyManage = data;
          this.spinnerService.hide();
        },
        error: () => {
          this.spinnerService.hide();
          this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
        }
      });
  }
  addCompany() {
    this.router.navigate(['manage/company/add']);
  }
  editCompany(model: CompanyManage, edit: string) {
    this.companyManageService.changeParamsDetail(model);
    this.router.navigate(['manage/company/edit/' + edit]);
  }
  openModal(template: TemplateRef<any>, model: CompanyManage) {
    this.detailCompanyManage = model;
    this.modalRef = this.modalService.show(template);

  }
}
