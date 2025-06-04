import { Component, OnInit } from '@angular/core';
import { CommonConstants } from '@constants/common.constants';
import { DestroyService } from '@services/destroy.service';
import * as moment from 'moment';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { takeUntil } from 'rxjs';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { Users } from '@models/auth/users.model';
import { LeaveDataHistory } from '@params/seahr/seahr-history/leavedata-history';
import { SearchHistoryParams } from '@params/seahr/seahr-history/search-history-params';
import { SeahrHistoryService } from '@services/seahr/seahr-history.service';
import { Pagination } from '@utilities/pagination-utility';
import { KeyValuePair } from '@utilities/key-value-pair';
import { LangConstants } from '@constants/lang.constants';
import { InjectBase } from '@utilities/inject-base-app';
import { KeyValueUtility } from '@utilities/key-value-utility';

@Component({
  selector: 'app-seahr-history',
  templateUrl: './seahr-history.component.html',
  styleUrls: ['./seahr-history.component.scss'],
  providers: [DestroyService]
})
export class SeahrHistoryComponent extends InjectBase implements OnInit {
  leaveDataHistory: LeaveDataHistory[] = [];
  countEachCategory: KeyValueUtility[] = [];
  date = new Date();
  minDate: Date;
  maxDate: Date;
  searchHistoryParams: SearchHistoryParams = <SearchHistoryParams>{
    partList: [],
  };
  pagination: Pagination = {
    pageNumber: 1,
    pageSize: 100,
  } as Pagination

  bsConfig: Partial<BsDatepickerConfig> = {
    isAnimated: true,
    containerClass: 'theme-dark-blue',
    dateInputFormat: 'DD/MM/YYYY'
  }
  fromDateDate: Date = new Date();
  toDateDate: Date = new Date();

  category: KeyValuePair[] = [];
  departments: KeyValuePair[] = [];
  parts: KeyValuePair[] = [];
  status: KeyValuePair[] = [];
  user: Users = JSON.parse(localStorage.getItem(LocalStorageConstants.USER));
  languageCurrent: string = localStorage.getItem(LocalStorageConstants.LANG);
  commonConstants = CommonConstants;
  langConstants = LangConstants;
  constructor(
    private seahrHistoryService: SeahrHistoryService,
  ) {
    super()
  }

  ngOnInit(): void {
    this.getCategory(this.languageCurrent);
    this.getDepartments();
    this.loadStatus()

    this.seahrHistoryService.dataSource.subscribe({
      next: result => {
        if (result) {
          this.searchHistoryParams = result;

          this.fromDateDate = this.searchHistoryParams.startTime == null ? null : new Date(this.searchHistoryParams.startTime);
          this.toDateDate = this.searchHistoryParams.endTime == null ? null : new Date(this.searchHistoryParams.endTime);
          this.getPart()
        }
        else {
          this.fromDateDate = this.fromDateDate?.toFirstDateOfMonth();
          this.toDateDate = this.toDateDate?.toLastDateOfMonth();
        }
      }
    })


    this.translateService.onLangChange.pipe(takeUntil(this.destroyService.destroys$)).subscribe(res => {
      this.languageCurrent = res.lang;
      this.loadStatus()
      this.getCategory(this.languageCurrent);
      this.getLeaveData()
    });
    this.getLeaveData()
  }
  loadStatus() {
    this.status = [
      { key: 0, value: this.translateService.instant('SeaHr.SeaHrHistory.Default') },
      { key: 1, value: this.translateService.instant('SeaHr.SeaHrHistory.Pending') },
      { key: 5, value: this.translateService.instant('SeaHr.SeaHrHistory.LockPending') },
      { key: 2, value: this.translateService.instant('SeaHr.SeaHrHistory.Approved') },
      { key: 3, value: this.translateService.instant('SeaHr.SeaHrHistory.Refuse') },
      { key: 4, value: this.translateService.instant('SeaHr.SeaHrHistory.CompleteList') },
      { key: 6, value: this.translateService.instant('SeaHr.SeaHrHistory.Deleted') },
    ];
  }
  getCategory(lang: string) {
    this.searchHistoryParams.lang = lang;
    this.seahrHistoryService.getCategory(this.searchHistoryParams.lang)
      .pipe(takeUntil(this.destroyService.destroys$))
      .subscribe({
        next: (data) => {
          this.category = data;
        },
        error: () => {
          this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
        }
      });
  }
  getDepartments() {
    this.seahrHistoryService.getDepartments()
      .pipe(takeUntil(this.destroyService.destroys$))
      .subscribe({
        next: (data) => {
          this.departments = data;
        },
        error: () => {
          this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
        }
      });
  }
  getPartData() {
    this.searchHistoryParams.partList = [];
    this.getPart();
  }
  getPart() {
    if (this.searchHistoryParams.departmentId !== undefined && this.searchHistoryParams.departmentId !== null) {
      this.seahrHistoryService.getPart(this.searchHistoryParams.departmentId)
        .pipe(takeUntil(this.destroyService.destroys$))
        .subscribe({
          next: (data) => {
            this.parts = data;
          },
          error: () => {
            this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
          }
        });
    }
  }
  getLeaveData() {
    this.checkDate()
    this.spinnerService.show();
    this.seahrHistoryService.getLeaveData(this.searchHistoryParams, this.pagination).pipe(takeUntil(this.destroyService.destroys$)).subscribe({
      next: (data) => {
        this.leaveDataHistory = data.leaveData.result;
        this.countEachCategory = data.countEachCategory;
        this.pagination = data.leaveData.pagination;
        this.spinnerService.hide();
        this.leaveDataHistory.forEach(element => {
          if (element.approved === 1) {
            element.status = this.translateService.instant(this.commonConstants.COMMON_STATUS1); // pending
          } else if (element.approved === 2) {
            element.status = this.translateService.instant(this.commonConstants.COMMON_STATUS2); // approved
          } else if (element.approved === 3) {
            element.status = this.translateService.instant(this.commonConstants.COMMON_STATUS3); // refuse
          } else {
            element.status = this.translateService.instant(this.commonConstants.COMMON_STATUS4); // complete
          }
        });
      },
      error: () => {
        this.spinnerService.hide();
        this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
      }
    });
  }
  btnBack() {
    this.seahrHistoryService.dataSource.next(this.searchHistoryParams)
    this.router.navigate(['/seahr/']);
  }

  onKeyUpEmpID() {
    if (this.searchHistoryParams.empId && this.searchHistoryParams.empId.length === 5) {
      this.btnSearch();
    }
  }

  btnSearch() {
    this.pagination.pageNumber = 1;
    this.getLeaveData();
  }
  btnExportExcel() {
    this.checkDate()
    if (this.searchHistoryParams.startTime > this.searchHistoryParams.endTime)
      return this.snotifyService.warning(this.translateService.instant('System.Message.CompareDate'),
        this.translateService.instant('System.Caption.Warning'));
    let startTime = moment(this.searchHistoryParams.startTime, 'YYYY/MM/DD').format('YYYY-MM-DD');
    let endTime = moment(this.searchHistoryParams.endTime, 'YYYY/MM/DD').format('YYYY-MM-DD');
    this.searchHistoryParams.startTime = startTime;
    this.searchHistoryParams.endTime = endTime;

    this.spinnerService.show();
    this.seahrHistoryService.exportExcel(this.searchHistoryParams, this.pagination).subscribe({
      next: (result) => {
        this.spinnerService.hide();
        result.isSuccess ? this.functionUtility.exportExcel(result.data, 'SeaHr_History')
          : this.snotifyService.error(this.translateService.instant('System.Message.SystemError'),
            this.translateService.instant('System.Caption.Error'));
      },
      error: () => {
        this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
        this.spinnerService.hide();
      },
    });
  }

  checkDate() {
    if (!this.functionUtility.checkEmpty(this.fromDateDate))
      this.searchHistoryParams.startTime = this.functionUtility.getDateFormat(this.fromDateDate);
    else
      this.deleteProperty('startTime')

    if (!this.functionUtility.checkEmpty(this.toDateDate))
      this.searchHistoryParams.endTime = this.functionUtility.getDateFormat(this.toDateDate);
    else
      this.deleteProperty('endTime')
  }

  pageChanged(event: any): void {
    this.pagination.pageNumber = event.page;
    this.getLeaveData();
  }
  clearStatus() {
    this.deleteProperty('status')
  }
  clearCategory() {
    this.deleteProperty('categoryId')
  }
  clearDepartment() {
    this.deleteProperty('departmentId')
    this.searchHistoryParams.partList = [];
    this.parts = [];
  }
  clearPart() {
    this.searchHistoryParams.partList = [];
  }
  openCategory() {
    this.getCategory(this.languageCurrent);
  }
  toDetail(leaveID: number) {
    this.router.navigate([`/leave/detail/${leaveID}`]).then(
      () => {
        this.seahrHistoryService.dataSource.next(this.searchHistoryParams)
      },
      (error) => { }
    );
  }
  deleteProperty(name: string) {
    delete this.searchHistoryParams[name]
  }
}
