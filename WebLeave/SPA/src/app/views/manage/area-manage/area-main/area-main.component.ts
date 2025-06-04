import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { AreaInformation } from '@models/manage/area-manage/area-infomation';
import { AreaManageService } from '@services/manage/area-manage.service';
import { InjectBase } from '@utilities/inject-base-app';
@Component({
  selector: 'app-area-main',
  templateUrl: './area-main.component.html',
  styleUrls: ['./area-main.component.css']
})
export class AreaMainComponent extends InjectBase implements OnInit {
  areas: AreaInformation[] = [];
  areaDetail: AreaInformation;
  modalRef?: BsModalRef;
  constructor(
    private areaManageService: AreaManageService,
    private modalService: BsModalService,
  ) {
    super();
  }

  ngOnInit() {
    this.getData();
  }

  getData() {
    this.spinnerService.show();
    this.areaManageService.getAreas().subscribe({
      next: (res) => {
        this.areas = res;
        this.spinnerService.hide();
      },
      error: (error) => {
        this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
        this.spinnerService.hide();
      },
      complete: () => { }
    });
  }

  openModal(template: TemplateRef<any>, model: AreaInformation) {
    this.areaDetail = model;
    this.modalRef = this.modalService.show(template);

  }
  changePageAdd() {
    this.router.navigate(['/manage/area/add'])
  }
  changePageEdit(model: AreaInformation) {
    this.areaManageService.areaSource.next(model);
    this.router.navigate(['/manage/area/edit']);
  }
}
