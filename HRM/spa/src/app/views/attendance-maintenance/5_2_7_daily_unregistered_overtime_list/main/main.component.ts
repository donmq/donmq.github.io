import { S_5_2_7_DailyUnregisteredOvertimeList } from '@services/attendance-maintenance/s_5_2_7_daily_unregistered_overtime_list.service';
import { Component, effect, OnDestroy, OnInit, ViewChild } from '@angular/core';
import {
  ClassButton,
  IconButton
} from '@constants/common.constants';
import {
  DailyUnregisteredOvertimeList_Memory,
  DailyUnregisteredOvertimeList_Param
} from '@models/attendance-maintenance/5_2_7_daily_unregistered_overtime_list';
import { InjectBase } from '@utilities/inject-base-app';
import { KeyValuePair } from '@utilities/key-value-pair';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { ActivatedRoute } from '@angular/router';
import { FormGroup, NgForm } from '@angular/forms';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent extends InjectBase implements OnInit, OnDestroy {
  @ViewChild('mainForm') public mainForm: NgForm;
  title: string = '';
  programCode: string = '';
  bsConfig: Partial<BsDatepickerConfig> = <Partial<BsDatepickerConfig>>{
    isAnimated: true,
    dateInputFormat: 'YYYY/MM/DD',
  };

  total: number = 0;
  iconButton = IconButton;
  classButton = ClassButton;
  param: DailyUnregisteredOvertimeList_Param = <DailyUnregisteredOvertimeList_Param>{};
  factoryList: KeyValuePair[] = [];
  departmentList: KeyValuePair[] = [];
  allowGetData: boolean = false

  constructor(
    private activatedRoute: ActivatedRoute,
    private service: S_5_2_7_DailyUnregisteredOvertimeList
  ) {
    super();
    this.programCode = this.route.snapshot.data['program'];
    this.translateService.onLangChange.pipe(takeUntilDestroyed()).subscribe(() => {
      this.title = this.functionUtility.getTitle(this.route.snapshot.data['program'])
      this.retryGetDropDownList()
    });
    effect(() => {
      this.param = this.service.paramSearch().param;
      this.total = this.service.paramSearch().total
      if (this.total > 0) {
        if (!this.functionUtility.checkFunction('Search')) {
          this.clear();
          this.allowGetData = false
        }
        else
          this.allowGetData = true
      }
      this.retryGetDropDownList();
    });
  }
  ngOnInit(): void {
    this.title = this.functionUtility.getTitle(this.route.snapshot.data['program'])
    this.activatedRoute.data.subscribe(
      (role) => {
        this.filterList(role.dataResolved)
      }).unsubscribe();
  }
  ngAfterViewChecked() {
    if (this.allowGetData && this.mainForm) {
      const form: FormGroup = this.mainForm.form
      const values = Object.values(form.value)
      const isLoaded = !values.every(x => x == undefined)
      if (isLoaded) {
        if (form.valid)
          this.search(false);
        this.allowGetData = false
      }
    }
  }
  ngOnDestroy(): void {
    this.service.setParamSearch(<DailyUnregisteredOvertimeList_Memory>{ param: this.param, total: this.total });
  }
  retryGetDropDownList() {
    this.service.getDropDownList(this.param.factory)
      .subscribe({
        next: (res) => {
          this.filterList(res)
        },
        error: () => this.functionUtility.snotifySystemError(false)
      });
  }
  filterList(keys: KeyValuePair[]) {
    this.factoryList = structuredClone(keys.filter((x: { key: string; }) => x.key == "FA")).map(x => <KeyValuePair>{ key: x.key = x.value.substring(0, x.value.indexOf('-')), value: x.value })
    this.departmentList = structuredClone(keys.filter((x: { key: string; }) => x.key == "DE")).map(x => <KeyValuePair>{ key: x.key = x.value.substring(0, x.value.indexOf('-')), value: x.value })
  }
  search = (isSearch: boolean) => {
    this.spinnerService.show();
    this.param.lang = localStorage.getItem(LocalStorageConstants.LANG)
    this.service
      .search(this.param)
      .subscribe({
        next: (res) => {
          this.spinnerService.hide();
          if (res.isSuccess) {
            this.total = res.data;
            if (isSearch)
              this.snotifyService.success(
                this.translateService.instant('System.Message.SearchOKMsg'),
                this.translateService.instant('System.Caption.Success')
              );
          }
          else {
            this.snotifyService.error(
              this.translateService.instant(`AttendanceMaintenance.DailyUnregisteredOvertimeList.${res.error}`),
              this.translateService.instant('System.Caption.Error'));
          }
        },
        error: () => this.functionUtility.snotifySystemError()
      });
  };
  clear() {
    this.param = <DailyUnregisteredOvertimeList_Param>{}
    this.total = 0
  }
  excel() {
    this.spinnerService.show();
    this.param.lang = localStorage.getItem(LocalStorageConstants.LANG)
    this.service.excel(this.param).subscribe({
      next: (res) => {
        this.spinnerService.hide()
        if (res.isSuccess) {
          const fileName = this.functionUtility.getFileNameExport(this.programCode, 'Download')
          this.functionUtility.exportExcel(res.data.result, fileName);
          this.total = res.data.count;
        }
        else {
          this.snotifyService.error(
            this.translateService.instant(`AttendanceMaintenance.DailyUnregisteredOvertimeList.${res.error}`),
            this.translateService.instant('System.Caption.Error'));
        }
      },
      error: () => this.functionUtility.snotifySystemError()
    });
  }
  onFactoryChange() {
    this.retryGetDropDownList()
    this.deleteProperty('department')
  }
  onDateChange(name: string) {
    this.param[`${name}_Str`] = this.param[name] ? this.functionUtility.getDateFormat(new Date(this.param[name])) : '';
  }
  deleteProperty(name: string) {
    delete this.param[name]
  }
}
