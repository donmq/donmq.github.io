import { Component, OnInit } from '@angular/core';
import { Category } from '@models/manage/category-management/category';
import { CategoryDetail } from '@models/manage/category-management/category-detail';
import { CategoryService } from '@services/manage/category.service';
import { InjectBase } from '@utilities/inject-base-app';
import { OperationResult } from '@utilities/operation-result';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-category-management-add-edit',
  templateUrl: './category-management-add-edit.component.html',
  styleUrls: ['./category-management-add-edit.component.scss']
})
export class CategoryManagementAddEditComponent extends InjectBase implements OnInit {
  categoryDetail: CategoryDetail = <CategoryDetail>{};
  _categoryDetail: CategoryDetail;
  data = {
    cate: <Category>{},
    type: ''
  };
  constructor(
    public bsModalRef: BsModalRef,
    private service: CategoryService,
  ) {
    super();
  }

  ngOnInit(): void {
    this.data.cate && this.getEditDetail(this.data.cate.cateID);
    this._categoryDetail = { ...this.categoryDetail };
  }

  getEditDetail(id: number) {
    this.spinnerService.show();
    this.service.getEditDetail(id).subscribe({
      next: (res: CategoryDetail) => {
        this.categoryDetail = res;
        this._categoryDetail = { ...this.categoryDetail };
      },
      error: (err) => {
        this.spinnerService.hide();
        this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
      },
      complete: () => this.spinnerService.hide()
    });
  }

  onSubmit() {
    this.data.type === 'edit' ? this.edit() : this.add();
  }

  add() {
    this.spinnerService.show();
    this.service.create(this.categoryDetail).subscribe({
      next: (res: OperationResult) => {
        if (res.isSuccess) {
          this.snotifyService.success(this.translateService.instant('System.Message.CreateOKMsg'), this.translateService.instant('System.Caption.Success'));
          this.spinnerService.hide();
          this.bsModalRef.hide();
        } else {
          this.snotifyService.error(this.translateService.instant('System.Message.CreateErrorMsg'), this.translateService.instant('System.Caption.Error'));
        }
      },
      error: (err) => {
        this.spinnerService.hide();
        this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
      }
    });
  }

  edit() {
    this.spinnerService.show();
    this.service.edit(this.categoryDetail).subscribe({
      next: (res: OperationResult) => {
        if (res.isSuccess) {
          this.spinnerService.hide();
          this.bsModalRef.hide();
          this.reset()
          this.snotifyService.success(this.translateService.instant('System.Message.UpdateOKMsg'), this.translateService.instant('System.Caption.Success'));
        }
        else {
          this.snotifyService.error(this.translateService.instant('System.Message.UpdateErrorMsg'), this.translateService.instant('System.Caption.Error'));
        }
      },
      error: (err) => {
        this.spinnerService.hide();
        this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
      }
    });
  }

  reset() {
    this.categoryDetail = { ...this._categoryDetail };
  }

}
