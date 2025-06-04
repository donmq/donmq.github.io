import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { BuildingInformation } from '@models/manage/building-manage/building-information';
import { BuildingManageService } from '@services/manage/building-manage.service';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-building-main',
  templateUrl: './building-main.component.html',
  styleUrls: ['./building-main.component.css']
})
export class BuildingMainComponent extends InjectBase implements OnInit {
  buildings: BuildingInformation[] = [];
  buildingDetail: BuildingInformation;
  modalRef?: BsModalRef;
  constructor(private buildingManageService: BuildingManageService,
    private modalService: BsModalService,
  ) {
    super();
  }

  ngOnInit() {
    this.getBuildings();
  }

  getBuildings() {
    this.spinnerService.show();
    this.buildingManageService.getBuildings().subscribe(res => {
      this.buildings = res;
      this.spinnerService.hide();
    }, error => {
      this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
      this.spinnerService.hide();
    });


  }
  openModal(template: TemplateRef<any>, model: BuildingInformation) {
    this.buildingDetail = model;
    this.modalRef = this.modalService.show(template);

  }
  changePageAdd() {
    this.router.navigate(['/manage/building/add']);
  }
  changePageEdit(model: BuildingInformation) {
    this.buildingManageService.buildingSource.next(model);
    this.router.navigate(['/manage/building/edit']);
  }
}
