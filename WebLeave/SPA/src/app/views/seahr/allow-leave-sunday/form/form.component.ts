import { Component, OnInit } from '@angular/core';
import { CommonConstants } from '@constants/common.constants';
import { LangConstants } from '@constants/lang.constants';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { AllowLeaveSundayDto, AllowLeaveSundayParam } from '@models/seahr/allow-leave-sunday';
import { AllowLeaveSundayService } from '@services/seahr/allow-leave-sunday.service';
import { InjectBase } from '@utilities/inject-base-app';
import { KeyValuePair } from '@utilities/key-value-pair';
import { Pagination } from '@utilities/pagination-utility';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.scss']
})
export class FormComponent extends InjectBase implements OnInit {
  parts: KeyValuePair[] = [];
  listData: AllowLeaveSundayDto[] = [];

  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 100,
  };

  checkboxAll: boolean = false;
  param: AllowLeaveSundayParam = <AllowLeaveSundayParam> {};
  empSelected: number[] = [];

  commonConstants: typeof CommonConstants = CommonConstants;
  lang: string =
    localStorage.getItem(LocalStorageConstants.LANG) ?? LangConstants.VN;

  constructor(private _service: AllowLeaveSundayService) {
    super();
  }

  ngOnInit(): void {
    this.getParts();
  }

  search() {
    this.pagination.pageNumber = 1;
    this.getEmployee();
  }

  pageChanged(event) {
    this.pagination.pageNumber = event.page;
    this.getEmployee();
  }

  getEmployee() {
    this.spinnerService.show();
    this._service.getEmployee(this.param)
    .subscribe({
        next: (res) => {
          this.listData = res;
          this.spinnerService.hide();
          this.checkboxAll = false;
          this.empSelected = [];
        },
        error: () => {
          this.snotifyService.error(
            this.translateService.instant('System.Message.SystemError'),
            this.translateService.instant('System.Caption.Error'))
          this.spinnerService.hide();
        }
      }
    );
  }

  back() {
    this.router.navigate(['/seahr/allow-leave-sunday']);
  }

  partChange(e: any) {
    if (!this.functionUtility.checkEmpty(e)) this.param.partId = e;
  }

  clearPart = () => delete this.param.partId;

  getParts() {
    this.spinnerService.show();
    this._service.getParts().subscribe((result) => {
      this.parts = result;
      this.spinnerService.hide();
    });
  }

  checkboxAllChanged() {
    this.listData.forEach(item => { item.isSun = this.checkboxAll });

    if(this.checkboxAll)
      this.empSelected = structuredClone(this.listData.map(x => x.empID));
    else
      this.empSelected = [];
  }

  selectItem() {
    this.empSelected = structuredClone(this.listData.filter(x => x.isSun).map(x => x.empID))
    this.checkboxAll = this.listData.every(x => x.isSun);
  }

  allowLeave() {
    this.spinnerService.show()
    this._service.allowLeave(this.empSelected)
      .subscribe({
        next: (res) => {
          if(res.isSuccess)
          {
            this.snotifyService.success(
              this.translateService.instant('System.Message.UpdateOKMsg'),
              this.translateService.instant('System.Caption.Success')
            )
            this.spinnerService.hide();
            this.back();
          } else {
            this.snotifyService.success(
              this.translateService.instant(res.error ?? 'System.Message.UpdateErrorMsg'),
              this.translateService.instant('System.Caption.Error')
            )
            this.spinnerService.hide();
          }
        },
        error: () => {
          this.snotifyService.error(
            this.translateService.instant('System.Message.SystemError'),
            this.translateService.instant('System.Caption.Error'))
          this.spinnerService.hide();
        }
      })
  }
}
