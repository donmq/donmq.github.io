import { LeaveData } from '@models/common/leave-data';
import { leaveDataHistory } from '@models/leave/leave-history/leaveDataHistory';
import { LeaveCategory } from '@models/leave/leave-history/leaveCategory';
import { Component, OnInit } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { LangConstants } from '@constants/lang.constants';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { Users } from '@models/auth/users.model';
import { LeaveHistoryService } from '@services/leave/leave-history.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { Pagination } from '@utilities/pagination-utility';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';
import { CommonConstants } from '@constants/common.constants';
import { HistoryExportParam, SearchHistoryParams } from '@models/leave/leave-history/searchHistoryParams';
import { take, takeUntil } from 'rxjs';
import { DestroyService } from '@services/destroy.service';
import { InjectBase } from '@utilities/inject-base-app';
@Component({
  selector: 'app-leave-history',
  templateUrl: './leave-history.component.html',
  styleUrls: ['./leave-history.component.scss'],
  providers: [DestroyService]
})
export class LeaveHistoryComponent extends InjectBase implements OnInit {
  langConstants: typeof LangConstants = LangConstants;
  commonConstants: typeof CommonConstants = CommonConstants;
  bsDatepickerConfig: Partial<BsDatepickerConfig> = {
    isAnimated: true,
    containerClass: 'theme-dark-blue',
    dateInputFormat: 'DD/MM/YYYY'
  }
  listCategory: LeaveCategory[] = [];
  categoryList: KeyValuePair[] = [];
  historyParam: SearchHistoryParams = <SearchHistoryParams>{};
  leaveData: LeaveData[] = [];
  date: Date = new Date();
  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 100
  };
  lang: string = localStorage.getItem(LocalStorageConstants.LANG) ?? LangConstants.VN;
  userId: Users = JSON.parse(localStorage.getItem(LocalStorageConstants.USER));
  startTimeDate: Date = new Date();
  endTimeDate: Date = new Date();
  param: SearchHistoryParams = <SearchHistoryParams>{
    categoryId: 0,
    empId: "",
    userID: this.userId.userID,
    startTime: this.date.toFirstDateOfMonth().toStringDate(),
    endTime: this.date.toLastDateOfMonth().toStringDate(),
    lang: this.lang,
    status: 0
  };

  sumLeaveDay: number = 0;

  status: KeyValuePair[] = [
    { key: 0, value: 'Tất Cả Trạng Thái' },
    { key: 1, value: 'Dang cho duyet' },
    { key: 2, value: 'Da duyet' },
    { key: 3, value: 'Bi tu choi' },
    { key: 4, value: 'Hr xac nhan' },
    { key: 6, value: 'Da xoa' },
  ];

  constructor(
    private leaveHistoryService: LeaveHistoryService,
  ) {
    super();
    const res: KeyValuePair[] = this.route.snapshot.data['res'];
    this.categoryList = res;
    this.categoryList.unshift({ key: 0, value: this.translateService.instant('Leave.HistoryLeave.AllCategory') });
  }

  ngOnInit() {
    this.loadStatus()
    this.leaveHistoryService.dataSource.pipe(take(1)).subscribe({
      next: result => {
        if (result) {
          this.param = result;
          this.startTimeDate = this.param.startTime ? new Date(this.param.startTime) : new Date().toFirstDateOfMonth();
          this.endTimeDate = this.param.endTime ? new Date(this.param.endTime) : new Date().toLastDateOfMonth();
          this.getData();
        }
        else {
          this.startTimeDate = this.startTimeDate?.toFirstDateOfMonth();
          this.endTimeDate = this.startTimeDate?.toLastDateOfMonth();
          this.getData();
        }
      }
    })
    this.translateService.onLangChange.pipe(takeUntil(this.destroyService.destroys$)).subscribe(res => {
      this.lang = res.lang;
      this.getCategory(this.lang);
      this.loadStatus()
    });
  }

  back() {
    this.leaveHistoryService.dataSource.next(this.param)
    this.router.navigateByUrl('/leave');
  }

  leaveDetail(item: any) {
    this.router.navigate(['/leave/detail', item.leaveID]).then(
      () => {
        this.leaveHistoryService.dataSource.next(this.param)
      },
      () => { }
    );
  }
  loadStatus() {
    this.status = [
      { key: 1, value: this.translateService.instant('Leave.HistoryLeave.Pending') },
      { key: 2, value: this.translateService.instant('Leave.HistoryLeave.Approve') },
      { key: 3, value: this.translateService.instant('Leave.HistoryLeave.Refuse') },
      { key: 4, value: this.translateService.instant('Leave.HistoryLeave.Complete') },
      { key: 6, value: this.translateService.instant('Leave.HistoryLeave.Delete') },
    ];
  }

  getCategory(lang: string) {
    this.param.lang = lang;
    this.leaveHistoryService.getCategory(this.param.lang)
      .pipe(takeUntil(this.destroyService.destroys$))
      .subscribe({
        next: (data) => {
          this.categoryList = data;
          this.categoryList.unshift({ key: 0, value: this.translateService.instant('Leave.HistoryLeave.AllCategory') });
        }
      });
  }

  getData() {
    this.param.startTime = this.startTimeDate ? this.functionUtility.getDateFormat(this.startTimeDate) : '';
    this.param.endTime = this.endTimeDate ? this.functionUtility.getDateFormat(this.endTimeDate) : '';

    this.sumLeaveDay = 0;
    if (this.param.startTime > this.param.endTime) {
      this.snotifyService.warning(this.translateService.instant('System.Message.CompareDate'),
        this.translateService.instant('System.Caption.Warning'));
      return;
    }
    this.spinnerService.show();
    this.param.categoryId = this.param.categoryId == null ? 0 : this.param.categoryId;
    this.param.status = this.param.status == null ? 0 : this.param.status;

    this.leaveHistoryService.search(this.param, this.pagination)
      .pipe(takeUntil(this.destroyService.destroys$))
      .subscribe({
        next: (res: leaveDataHistory) => {
          this.leaveData = res.leaveData.result;

          this.pagination = res.leaveData.pagination;
          this.totalLeaveDay();
          this.spinnerService.hide();
        }, error: () => {
          this.snotifyService.error(
            this.translateService.instant('System.Message.UnknowError'),
            this.translateService.instant('System.Caption.Error')
          );
          this.spinnerService.hide();
        }
      });
  }

  search() {
    this.pagination.pageNumber === 1 ? this.getData() : this.pagination.pageNumber = 1;
  }

  totalLeaveDay() {
    this.sumLeaveDay = this.leaveData.filter(x => x.approved == 4).map(x => x.leaveDay).reduce((a, b) => (parseFloat(a.toString()) + parseFloat(b.toString())), 0);
  }

  onKeyUpEmpID() {
    if (this.param.empId && this.param.empId.length === 5) {
      this.search();
    }
  }

  pageChanged(event: PageChangedEvent) {
    this.pagination.pageNumber = event.page;
    this.getData();
  }

  exportExcelHistory() {
    this.spinnerService.show();
    let params: HistoryExportParam = {
      ...this.param,
      category: this.translateService.instant('Leave.HistoryLeave.Category'),
      deptName: this.translateService.instant('Leave.HistoryLeave.DeptName'),
      partName: this.translateService.instant('Leave.HistoryLeave.PartName'),
      employee: this.translateService.instant('Leave.HistoryLeave.Employee'),
      leaveDay: this.translateService.instant('Leave.HistoryLeave.LeaveDay'),
      status1: this.translateService.instant('Leave.HistoryLeave.Status1'),
      numberId: this.translateService.instant('Leave.HistoryLeave.EmployeeId'),
      timeStart: this.translateService.instant('Leave.HistoryLeave.TimeStart'),
      timeEnd: this.translateService.instant('Leave.HistoryLeave.TimeEnd'),
      update: this.translateService.instant('Leave.HistoryLeave.Update'),
    }

    // change Excel
    this.leaveHistoryService.exportExcelHistory(params, this.pagination).subscribe({
      next: (result) => {
        this.spinnerService.hide();
        console.log(result);
        result.isSuccess ? this.functionUtility.exportExcel(result.data, 'Leave_History')
          : this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
      },
      error: () => {
        this.snotifyService.error(
          this.translateService.instant('System.Message.UnknowError'),
          this.translateService.instant('System.Caption.Error')
        );
        this.spinnerService.hide();
      },
    });
  }
}
