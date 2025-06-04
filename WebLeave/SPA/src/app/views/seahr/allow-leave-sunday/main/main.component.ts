import { Component, OnInit } from '@angular/core';
import { CommonConstants } from '@constants/common.constants';
import { LangConstants } from '@constants/lang.constants';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import {
  AllowLeaveSundayDto,
  AllowLeaveSundayParam,
  AllowLeaveSundaySource,
} from '@models/seahr/allow-leave-sunday';
import { DestroyService } from '@services/destroy.service';
import { AllowLeaveSundayService } from '@services/seahr/allow-leave-sunday.service';
import { InjectBase } from '@utilities/inject-base-app';
import { KeyValuePair } from '@utilities/key-value-pair';
import { Pagination } from '@utilities/pagination-utility';
import { take } from 'rxjs';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss'],
  providers: [DestroyService]
})
export class MainComponent extends InjectBase implements OnInit {
  parts: KeyValuePair[] = [];
  listData: AllowLeaveSundayDto[] = [];

  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 100,
  };

  checkboxAll: boolean = false;
  param: AllowLeaveSundayParam = <AllowLeaveSundayParam> {};

  commonConstants: typeof CommonConstants = CommonConstants;
  lang: string =
    localStorage.getItem(LocalStorageConstants.LANG) ?? LangConstants.VN;

  constructor(private _service: AllowLeaveSundayService) {
    super();
  }

  ngOnInit(): void {
    this.getParts();
    this._service.dataSource.pipe(take(1))
    .subscribe({
      next: result => {
        if (result) {
          this.param = result.param;
          this.pagination = result.pagination
          this.getPagination()
        }
      }
    })
  }

  ngOnDestroy(): void {
    const source = <AllowLeaveSundaySource> {
      pagination: this.pagination,
      param: this.param
    }
    this._service.dataSource.next(source)
  }

  search() {
    this.pagination.pageNumber = 1;
    this.getPagination();
  }

  pageChanged(event) {
    this.pagination.pageNumber = event.page;
    this.getPagination();
  }

  getPagination() {
    this.spinnerService.show();
    this._service.getPagination(this.pagination, this.param).subscribe({
      next: (res) => {
        this.listData = res.result;
        this.pagination = res.pagination;
        this.spinnerService.hide();
      },
      error: () => {
        this.snotifyService.error(
          this.translateService.instant('System.Message.SystemError'),
          this.translateService.instant('System.Caption.Error')
        );
        this.spinnerService.hide();
      },
    });
  }

  back() {
    this.router.navigate(['/seahr']);
  }

  clearPart = () => (delete this.param.partId);

  getParts() {
    this.spinnerService.show();
    this._service.getParts().subscribe((result) => {
      this.parts = result;
      this.spinnerService.hide();
    });
  }

  selectItem(emp: AllowLeaveSundayDto) {
    this.snotifyService.confirm(
      this.translateService.instant('System.Message.ConfirmDisableMsg'), 
      this.translateService.instant('System.Action.Disable'), 
      () => {
        this.spinnerService.show();
        this._service.disallow(emp.empID)
        .subscribe({
          next: (res) => {
            if(res.isSuccess)
            {
              this.snotifyService.success(
                this.translateService.instant('System.Message.UpdateOKMsg'),
                this.translateService.instant('System.Caption.Success')
              )
              this.spinnerService.hide();
              this.getPagination();
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
              this.translateService.instant('System.Caption.Error')
            );
            this.spinnerService.hide();
          }
        })
    },
    () => {
      emp.isSun = true;
    });
  }

  clear() {
    this.param = <AllowLeaveSundayParam> {};
    this.pagination = <Pagination> {
      pageNumber: 1,
      pageSize: 100
    }
    this.listData = []
  }

  add() {
    this.router.navigate(['/seahr/allow-leave-sunday/add']);
  }
}
