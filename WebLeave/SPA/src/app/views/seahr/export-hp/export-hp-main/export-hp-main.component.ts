import { Component, OnInit } from '@angular/core';
import { BsDatepickerConfig, DatepickerDateCustomClasses } from 'ngx-bootstrap/datepicker';
import { Area } from '@models/common/area';
import { Department } from '@models/common/department';
import { ExportLeave } from '@models/seahr/export-hp/export-leave';
import { ExportHPParam } from '@params/seahr/export-hp/export-hp-param';
import { ExportHpService } from '@services/seahr/export-hp.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { Pagination } from '@utilities/pagination-utility';
import { bsDatepickerConfig } from '@constants/bs-config.enum';
import { InjectBase } from '@utilities/inject-base-app';
import { takeUntil } from 'rxjs';
import { LocalStorageConstants } from '@constants/local-storage.enum';

@Component({
  selector: 'app-export-hp-main',
  templateUrl: './export-hp-main.component.html',
  styleUrls: ['./export-hp-main.component.css'],
})
export class ExportHpMainComponent extends InjectBase implements OnInit {
  lang: string = localStorage.getItem(LocalStorageConstants.LANG);
  bsConfig: Partial<BsDatepickerConfig> = bsDatepickerConfig;
  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 15,
  };
  areas: Area[] = [];
  departments: Department[] = [];
  leaves: KeyValuePair[] = [];
  area: number = 0;
  department: number = 0;
  leaveID: number = 0;
  startDate: string = '';
  endDate: string = '';
  dataHPAll: ExportLeave[] = [];
  dataHPShow: ExportLeave[] = [];
  countData: number = 0;
  now: Date = new Date();
  dateCustomStart: DatepickerDateCustomClasses[];
  dateCustomEnd: DatepickerDateCustomClasses[];
  constructor(
    private exportHPService: ExportHpService,
  ) {
    super();

    this.dateCustomStart = [
      { date: this.now, classes: ['bg-secondary-custom'] },
    ];

    this.dateCustomEnd = [
      { date: this.now, classes: ['bg-secondary-custom'] },
    ];
  }

  ngOnInit() {
    this.getAreas();
    this.getDepartments();
    this.translateService.onLangChange.pipe(takeUntil(this.destroyService.destroys$)).subscribe(res => {
      this.lang = res.lang;
      this.changeLangLeaves(this.lang)
    });
    this.changeLangLeaves(this.lang)
  }

  changeSelect() {
    if (new Date(this.startDate).getDate() == new Date().getDate()) {
      this.dateCustomStart = []
    }
    else {
      this.dateCustomStart = [
        { date: this.now, classes: ['bg-secondary-custom'] },
      ];
    }

    if (new Date(this.endDate).getDate() == new Date().getDate()) {
      this.dateCustomEnd = []
    }
    else {
      this.dateCustomEnd = [
        { date: this.now, classes: ['bg-secondary-custom'] },
      ];
    }
  }

  back() {
    this.router.navigate(['seahr']);
  }

  getAreas() {
    this.commonService.getAreas().subscribe(res => {
      this.areas = res;
    })
  }

  getDepartments() {
    this.commonService.getDepartments().subscribe({
      next: (res) => {
        this.departments = res;
      },
      error: (error) => { this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error')); }
    })
  }

  validate() {
    if (this.area === undefined || this.department === undefined) {
      this.snotifyService.warning(this.translateService.instant('System.Message.NoDataSelected'),
        this.translateService.instant('System.Caption.Warning'));
      return null;
    }
    if (this.functionUtility.checkEmpty(this.startDate) || this.functionUtility.checkEmpty(this.endDate)) {
      this.snotifyService.warning(this.translateService.instant('System.Message.SelectDate'),
        this.translateService.instant('System.Caption.Warning'));
      return null;
    }
    let fromDate = this.functionUtility.getDateFormat(new Date(this.startDate));
    let toDate = this.functionUtility.getDateFormat(new Date(this.endDate));
    if (fromDate > toDate) {

      this.snotifyService.warning(this.translateService.instant('System.Message.CompareDate'),
        this.translateService.instant('System.Caption.Warning'));
      return null;
    }
    let paramSearch: ExportHPParam = {
      fromDate: fromDate,
      toDate: toDate,
      areaID: this.area,
      departmentID: this.department,
      leaveType: this.leaveID,
    };
    return paramSearch;
  }
  search() {
    let paramSearch = this.validate();
    this.pagination.pageNumber = 1;
    if (paramSearch != null) {
      this.spinnerService.show();
      this.exportHPService.search(paramSearch, this.pagination).subscribe({
        next: (res) => {
          this.dataHPAll = res.result;
          this.countData = this.dataHPAll.length;
          if (this.dataHPAll.length === 0) {
            this.dataHPShow.length = 0;
            this.pagination = <Pagination>{
              pageNumber: 1,
              pageSize: 15,
            };
            this.snotifyService.warning(this.translateService.instant('System.Message.Nodata'),
              this.translateService.instant('System.Caption.Warning'));
          } else {
            this.dataHPShow = this.dataHPAll.slice(
              (this.pagination.pageNumber - 1) * this.pagination.pageSize,
              this.pagination.pageSize * this.pagination.pageNumber
            );
            this.pagination = res.pagination;
          }
        },
        error: (err) => {
          this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
          this.spinnerService.hide();
        },
        complete: () => { this.spinnerService.hide() }
      })
    }
  }

  pageChanged(event: any): void {
    this.pagination.pageNumber = event.page;
    this.dataHPShow = this.dataHPAll.slice(
      (this.pagination.pageNumber - 1) * this.pagination.pageSize,
      this.pagination.pageSize * this.pagination.pageNumber
    );
  }

  exportExcel(typeFile: string) {
    let paramSearch = this.validate();
    if (paramSearch != null) {
      this.spinnerService.show();
      this.exportHPService.exportHPExcel(paramSearch, typeFile).subscribe({
        next: (result) => {
          this.spinnerService.hide();
          if (result.isSuccess) {
            if (typeFile == 'xlsx')
              this.functionUtility.exportExcel(result.data, 'ev134', typeFile)
            else
              this.functionUtility.exportCSV(result.data, 'ev134')
          }
        },
        error: () => {
          this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
          this.spinnerService.hide();
        },
      });
    }
  }
  changeLangLeaves(lang: string) {
    const translations = {
      'en': [
        { key: 0, value: 'All types of leave' },
        { key: 1, value: 'Take leave (excluding maternity and compensation leave)' },
        { key: 2, value: 'Take all types of leave (excluding maternity leave)' },
        { key: 3, value: 'Take compensation leave' },
        { key: 4, value: 'Take all types of leave (excluding compensation leave)' },
      ],
      'vi': [
        { key: 0, value: 'Tất cả các loại phép' },
        { key: 1, value: 'Lấy loại phép (trừ thai sản trừ phép bù)' },
        { key: 2, value: 'Lấy các loại phép (trừ thai sản)' },
        { key: 3, value: 'Lấy các loại phép bù' },
        { key: 4, value: 'Lấy các loại phép (trừ phép bù)' },
      ],
      'zh': [
        { key: 0, value: '所有类型的假期' },
        { key: 1, value: '休假（不包括产假和补偿假）' },
        { key: 2, value: '休所有类型的假期（不包括产假）' },
        { key: 3, value: '休偿假' },
        { key: 4, value: '休所有类型的假期（不包括补偿假）' },
      ],
    };

    this.leaves = translations[lang] || translations['en'];
  }
}
