import { LangConstants } from '@constants/lang.constants';
import { ViewDataConstants } from '@constants/view-data.constants';
import { catchError, firstValueFrom, of, Subject, takeUntil, tap } from 'rxjs';
import { Pagination } from '@utilities/pagination-utility';
import { ViewConfirmDaily } from '@models/seahr/view-confirm-daily.model';
import { Component, OnInit } from '@angular/core';
import { ViewConfirmDailyService } from '@services/seahr/view-confirm-daily.service';
import * as moment from 'moment';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { CommonConstants } from '@constants/common.constants';
import { InjectBase } from '@utilities/inject-base-app';
import { ViewConfirmDailyParam } from '@params/seahr/view-confirm-daily-param';

@Component({
  selector: 'app-view-confirm-daily',
  templateUrl: './view-confirm-daily.component.html',
  styleUrls: ['./view-confirm-daily.component.scss']
})
export class ViewConfirmDailyMainComponent extends InjectBase implements OnInit {
  viewConfirmDaily: ViewConfirmDaily[] = [];
  date = new Date();
  param: ViewConfirmDailyParam = <ViewConfirmDailyParam>{}
  dateFrom = new Date(this.date.getFullYear(), this.date.getMonth(), 1);
  dateTo = new Date(this.date.getFullYear(), this.date.getMonth() + 1, 0);
  viewDataConstants: typeof ViewDataConstants = ViewDataConstants;
  commonConstants: typeof CommonConstants = CommonConstants;
  lang: string = localStorage.getItem(LocalStorageConstants.LANG);
  unsubscribe = new Subject<void>();
  pagination: Pagination = {
    pageNumber: 1,
    pageSize: 20,
  } as Pagination
  bsConfig = Object.assign(
    {
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY',
      containerClass: 'theme-dark-blue',
    }
  );

  constructor(
    private viewConfirmDailyService: ViewConfirmDailyService,
  ) {
    super();
  }

  async ngOnInit() {
    this.viewConfirmDailyService.dataSource.subscribe({
      next: result => {
        if (result) {
          this.param = result;
          this.dateFrom = this.param.dateFrom ? new Date(this.param.dateFrom) : new Date().toFirstDateOfMonth();
          this.dateTo = this.param.dateTo ? new Date(this.param.dateTo) : new Date().toLastDateOfMonth();
        }
      }
    })
    this.translateService.onLangChange.subscribe(async res => {
      this.lang = res.lang === 'zh' ? LangConstants.ZH_TW : res.lang;
    });
    this.loadData()
  }

  loadData() {
    this.spinnerService.show();
    this.param.lang = this.lang;
    this.param.dateFrom = this.dateFrom ? moment(this.dateFrom, 'YYYY/MM/DD').format('YYYY-MM-DD') : '';
    this.param.dateTo = this.dateTo ? moment(this.dateTo, 'YYYY/MM/DD').format('YYYY-MM-DD') : '';

    firstValueFrom(this.viewConfirmDailyService.getViewConfirmDaily(this.param, this.pagination).pipe(
      tap(res => {
        this.spinnerService.hide();
        this.viewConfirmDaily = res.result;
        this.pagination = res.pagination;
      }),
      catchError(error => {
        this.spinnerService.hide();
        this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
        return of(null);
      })
    ));
  }

  btnSearch() {
    this.pagination.pageNumber = 1;
    this.loadData();
  }

  btnBack() {
    this.viewConfirmDailyService.dataSource.next(this.param)
    this.router.navigate(['/seahr/']);
  }
  leaveDetail(item: any) {
    this.router.navigate(['/leave/detail', item.leaveID]).then(
      () => {
        this.viewConfirmDailyService.dataSource.next(this.param)
      },
      (error) => {}
    );
  }

  btnExportExcel() {
    this.param.lang = this.lang;
    this.param.dateFrom = this.dateFrom ? moment(this.dateFrom, 'YYYY/MM/DD').format('YYYY-MM-DD') : '';
    this.param.dateTo = this.dateTo ? moment(this.dateTo, 'YYYY/MM/DD').format('YYYY-MM-DD') : '';

    this.initLang()
    this.spinnerService.show();
    this.viewConfirmDailyService.exportExcel(this.param)
      .pipe(takeUntil(this.unsubscribe))
      .subscribe({
        next: (result) => {
          this.spinnerService.hide();
          result.isSuccess ? this.functionUtility.exportExcel(result.data, 'View_Data')
            : this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
        },
        error: () => {
          this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
          this.spinnerService.hide();
        },
      });
  }

  private initLang(): void {
    this.param.label_PartCode = this.translateService.instant('SeaHr_ViewData.PartCode');
    this.param.label_DeptName = this.translateService.instant('SeaHr_ViewData.DeptName');
    this.param.label_Employee = this.translateService.instant('SeaHr_ViewData.EmpName');
    this.param.label_NumberID = this.translateService.instant('SeaHr_ViewData.EmpNumber');
    this.param.label_Category = this.translateService.instant('SeaHr_ViewData.Category');
    this.param.label_TimeStart = this.translateService.instant('SeaHr_ViewData.TimeStart');
    this.param.label_DateStart = this.translateService.instant('SeaHr_ViewData.DateStart');
    this.param.label_TimeEnd = this.translateService.instant('SeaHr_ViewData.TimeEnd');
    this.param.label_DateEnd = this.translateService.instant('SeaHr_ViewData.DateEnd');
    this.param.label_LeaveDay = this.translateService.instant('SeaHr_ViewData.LeaveDay');
    this.param.label_Status = this.translateService.instant('SeaHr_ViewData.StatusExcel');
    this.param.label_UpdateTime = this.translateService.instant('SeaHr_ViewData.UpdateTime');
  }

  ngOnDestroy() {
    this.unsubscribe.next();
    this.unsubscribe.complete();
  }

  pageChanged(event: any): void {
    this.pagination.pageNumber = event.page;
    this.loadData();
  }

}
