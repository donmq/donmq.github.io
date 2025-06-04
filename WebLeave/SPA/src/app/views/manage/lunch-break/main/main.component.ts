import { Component, OnInit } from '@angular/core';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { LunchBreak } from '@models/common/lunch-break';
import { LunchBreakService } from '@services/manage/lunch-break.service';
import { InjectBase } from '@utilities/inject-base-app';
import { Pagination } from '@utilities/pagination-utility';
import { BsModalService } from 'ngx-bootstrap/modal';
import { takeUntil } from 'rxjs';
import { FormComponent } from '../form/form.component';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent extends InjectBase implements OnInit {
  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 20
  }
  datas: LunchBreak[] = [];
  language: string = localStorage.getItem(LocalStorageConstants.LANG)?.toLowerCase();
  constructor(
    private _lunchBreakService: LunchBreakService,
    private modalService: BsModalService
  ) {
    super();
  }

  ngOnInit(): void {
    this.clear();

    this.translateService.onLangChange.pipe(takeUntil(this.destroyService.destroys$)).subscribe(event => {
      this.language = event.lang;
    });

    this._lunchBreakService.lunchBreakEmitter.pipe(takeUntil(this.destroyService.destroys$)).subscribe((res) => {
      if (res)
        this.search();
    })
  }

  back() {
    this.router.navigateByUrl('/manage');
  }

  getDataPagination() {
    this._lunchBreakService.getDataPagination(this.pagination)
      .subscribe({
        next: (res) => {
          this.pagination = res.pagination;
          this.datas = res.result;
        },
        error: () => {
          this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
        }
      })
  }

  search() {
    this.pagination.pageNumber == 1 ? this.getDataPagination() : this.pagination.pageNumber = 1;
  }

  detail(id: number) {
    this.spinnerService.show();
    this._lunchBreakService.getDetail(id)
      .subscribe({
        next: (res) => {
          let data = res;
          data.type = "DETAIL";
          this.spinnerService.hide();
          this._lunchBreakService.changeData(data);
          this.modalService.show(FormComponent);
        }, error: () => {
          this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
          this.spinnerService.hide();
        }
      })
  }

  create() {
    let data = <LunchBreak>{
      id: 0,
      seq: 1,
      visible: true,
      type: 'CREATE'
    };
    this._lunchBreakService.changeData(data);
    this.modalService.show(FormComponent);
  }

  update(id: number) {
    this.spinnerService.show();
    this._lunchBreakService.getDetail(id)
      .subscribe({
        next: (res) => {
          let data = res;
          data.type = "UPDATE";
          this.spinnerService.hide();
          this._lunchBreakService.changeData(data);
          this.modalService.show(FormComponent);
        }, error: () => {
          this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
          this.spinnerService.hide();
        }
      })
  }

  delete(id: number) {
    this.snotifyService.confirm(
      this.translateService.instant('System.Message.ConfirmDeleteMsg'),
      this.translateService.instant('System.Caption.Confirm'),
      () => {
        this.spinnerService.show();
        this._lunchBreakService.delete(id)
          .subscribe({
            next: (res) => {
              this.spinnerService.hide();
              if (res.isSuccess) {
                this.snotifyService.success(this.translateService.instant('System.Message.DeleteOKMsg'), this.translateService.instant('System.Caption.Success'));
                this.search();
              }
              else {
                this.snotifyService.error(this.translateService.instant('System.Message.DeleteErrorMsg'), this.translateService.instant('System.Caption.Error'));
              }
            }, error: () => {
              this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
              this.spinnerService.hide();
            }
          });
      }
    );
  }

  pageChanged(event: any) {
    this.pagination.pageNumber = event.page;
    this.getDataPagination();
  }

  clear() {
    this.pagination = <Pagination>{
      pageNumber: 1,
      pageSize: 20
    }
    this.getDataPagination();
  }
}
