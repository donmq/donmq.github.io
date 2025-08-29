import { Component, OnInit } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ClassButton, IconButton } from '@constants/common.constants';
import { FactoryResignationAnalysisReport_Param } from '@models/attendance-maintenance/5_2_23_factory_resignation_analysis_report';
import { S_5_2_23_FactoryResignationAnalysisReport } from '@services/attendance-maintenance/s_5_2_23_factory_resignation_analysis_report.service';
import { InjectBase } from '@utilities/inject-base-app';
import { KeyValuePair } from '@utilities/key-value-pair';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent extends InjectBase implements OnInit {
  title: string = '';
  programCode: string = '';
  bsConfig: Partial<BsDatepickerConfig> = <Partial<BsDatepickerConfig>>{
    isAnimated: true,
    dateInputFormat: 'YYYY/MM/DD',
  };
  iconButton = IconButton;
  classButton = ClassButton;

  param: FactoryResignationAnalysisReport_Param = <FactoryResignationAnalysisReport_Param>{}
  i18n: string = 'AttendanceMaintenance.FactoryResignationAnalysisReport.'
  factoryList: KeyValuePair[] = [];
  permissionGroupList: KeyValuePair[] = [];

  constructor(private service: S_5_2_23_FactoryResignationAnalysisReport) {
    super();
    this.programCode = this.route.snapshot.data['program'];
    this.translateService.onLangChange.pipe(takeUntilDestroyed()).subscribe(() => {
      this.title = this.functionUtility.getTitle(this.route.snapshot.data['program'])
      this.getDropDownList()
    });
  }

  ngOnInit(): void {
    this.title = this.functionUtility.getTitle(this.route.snapshot.data['program'])
    this.param = this.service.paramSearch();
    this.getDropDownList()
  }
  ngOnDestroy(): void {
    this.service.setParamSearch(this.param);
  }
  callFunction(func: string) {
    this.param.function_Type = func
    this[func]()
  }
  search() {
    this.spinnerService.show();
    this.service.process(this.param).subscribe({
      next: (res) => {
        this.spinnerService.hide();
        if (res.isSuccess) {
          this.snotifyService.success(
            this.translateService.instant('System.Message.SearchOKMsg'),
            this.translateService.instant('System.Caption.Success')
          );
          this.param.total_Rows = res.data
        } else {
          this.snotifyService.error(
            this.translateService.instant(this.getTransKey(res.error)),
            this.translateService.instant('System.Caption.Error')
          );
        }
      },
      error: () => this.functionUtility.snotifySystemError()
    })
  }

  excel() {
    this.spinnerService.show();
    this.service
      .process(this.param)
      .subscribe({
        next: (res) => {
          this.spinnerService.hide();
          if (res.isSuccess) {
            if (res.data.count > 0){
              const fileName = this.functionUtility.getFileNameExport(this.programCode, 'Download')
              this.functionUtility.exportExcel(res.data.result, fileName);
            }
            this.param.total_Rows = res.data.count
          } else {
            this.snotifyService.error(
              this.translateService.instant(this.getTransKey( res.error)),
              this.translateService.instant('System.Caption.Error')
            );
          }
        },
        error: () => this.functionUtility.snotifySystemError()
      });
  }

  clear() {
    this.param = structuredClone(this.service.initData)
  }

  getDropDownList() {
    this.service.getDropDownList(this.param)
      .subscribe({
        next: (res) => {
          this.filterList(res)
        },
        error: () => {
          this.snotifyService.error(
            this.translateService.instant('System.Message.UnknowError'),
            this.translateService.instant('System.Caption.Error')
          );
        }
      });
  }

  filterList(keys: KeyValuePair[]) {
    this.factoryList = structuredClone(keys.filter((x: { key: string; }) => x.key == "FA")).map(x => <KeyValuePair>{ key: x.key = x.value.substring(0, x.value.indexOf('-')), value: x.value })
    this.permissionGroupList = structuredClone(keys.filter((x: { key: string; }) => x.key == "PE")).map(x => <KeyValuePair>{ key: x.key = x.value.substring(0, x.value.indexOf('-')), value: x.value })
    this.functionUtility.getNgSelectAllCheckbox(this.permissionGroupList)
  }

  onDateChange(item: string) {
    this.param[item + '_Str'] = this.functionUtility.isValidDate(new Date( this.param[item]))
      ? this.functionUtility.getDateFormat(new Date(this.param[item]))
      : '';
  }

  onFactoryChange() {
    this.getDropDownList()
    this.deleteProperty('permission_Group')
  }

  deleteProperty(name: string) {
    delete this.param[name]
  }
  getTransKey(key: string): string {
    return this.functionUtility.hasTranslation(`${this.i18n}${key}`)
      ? `${this.i18n}${key}`
      : `${this.translateService.instant(`${this.i18n}InvalidErrorCode`)} : ${key}`
  }
}
