import { Pagination } from '@utilities/pagination-utility';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { IconButton } from '@constants/common.constants';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ActionConstants, MessageConstants } from '@constants/message.enum';
import { PositionManage } from '@models/manage/position-manage/postion-manage';
import { PositionManageService } from '@services/manage/position-manage.service';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { DestroyService } from '@services/destroy.service';
import { takeUntil } from 'rxjs';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-position-main',
  templateUrl: './position-main.component.html',
  styleUrls: ['./position-main.component.scss'],
  providers: [DestroyService]
})
export class PositionMainComponent extends InjectBase implements OnInit {
  listPositionManage: PositionManage[] = [];
  detailPositionManage: PositionManage = {} as PositionManage;
  modalRef?: BsModalRef;
  languageCurrent: string = localStorage.getItem(LocalStorageConstants.LANG);
  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 20
  };
  constructor(
    private positionManageService: PositionManageService,
    private modalService: BsModalService,
  ) {
    super()
  }

  ngOnInit() {
    this.getAllPosition();
    this.translateService.onLangChange.subscribe(async res => {
      this.languageCurrent = res.lang;
    });
  }
  getAllPosition() {
    this.spinnerService.show();
    this.positionManageService.getAllPosition(this.pagination).pipe(takeUntil(this.destroyService.destroys$)).subscribe({
      next: (data) => {
        this.listPositionManage = data.result;
        this.pagination = data.pagination;
        this.spinnerService.hide();
      },
      error: () => {
        this.spinnerService.hide();
        this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
      }
    });
  }
  addPosition() {
    this.router.navigate(['manage/position/add']);
  }
  editPosition(model: PositionManage, edit: string) {
    this.positionManageService.changeParamsDetail(model.positionID);
    this.router.navigate(['manage/position/edit/' + edit]);
  }
  exportExcel() {
    //change lang excel
    this.detailPositionManage.positionSymExcel = this.translateService.instant('Manage.PositionFunction.PositionSym');
    this.detailPositionManage.positionNameExcel = this.translateService.instant('Manage.PositionFunction.PositionName');
    this.spinnerService.show();
    this.positionManageService.exportExcel(this.pagination, this.detailPositionManage).subscribe({
      next: (result) => {
        this.spinnerService.hide();
        result.isSuccess ? this.functionUtility.exportExcel(result.data, 'Position')
          : this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
      },
      error: () => {
        this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
        this.spinnerService.hide();
      },
    });
  }
  removePosition(IDPosition: number) {
    this.snotifyService.confirm(MessageConstants.CONFIRM_DELETE, ActionConstants.DELETE, () => {
      this.positionManageService.removePosition(IDPosition).subscribe(res => {
        if (res.isSuccess) {
          this.getAllPosition();
          this.snotifyService.success(this.translateService.instant('System.Message.DeleteOKMsg'), this.translateService.instant('System.Caption.Success'));
        } else {
          this.snotifyService.error(this.translateService.instant('System.Message.DeleteErrorMsg'), this.translateService.instant('System.Caption.Error'));
        }
      });
    })
  }
  openModal(template: TemplateRef<any>, model: PositionManage) {
    this.detailPositionManage = model;
    this.modalRef = this.modalService.show(template);

  }
  pageChanged(event: any) {
    this.pagination.pageNumber = event.page;
    this.getAllPosition();
  }
}
