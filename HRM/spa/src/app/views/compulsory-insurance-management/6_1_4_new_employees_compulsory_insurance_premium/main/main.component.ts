import { Component, effect, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ClassButton, IconButton } from '@constants/common.constants';
import { NewEmployeesCompulsoryInsurancePremium_Param, NewEmployeesCompulsoryInsurancePremium_Memory } from '@models/compulsory-insurance-management/6_1_4_new_employees_compulsory_insurance_premium';
import { S_6_1_4_NewEmployeesCompulsoryInsurancePremium } from '@services/compulsory-insurance-management/s_6_1_4_new_employees_compulsory_insurance_premium.service';
import { InjectBase } from '@utilities/inject-base-app';
import { KeyValuePair } from '@utilities/key-value-pair';
import { TabComponentModel } from '@views/_shared/tab-component/tab.component';
import { BsDatepickerConfig, BsDatepickerViewMode } from 'ngx-bootstrap/datepicker';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent extends InjectBase implements OnInit {
  @ViewChild('generationForm', { static: true }) generationForm: TemplateRef<any>;
  @ViewChild('reportForm', { static: true }) reportForm: TemplateRef<any>;
  tabs: TabComponentModel[] = [];
  title: string = '';
  programCode: string = '';

  bsConfig: Partial<BsDatepickerConfig> = <Partial<BsDatepickerConfig>>{};
  minMode: BsDatepickerViewMode = 'month';
  minSalaryDays: string = '1'
  maxSalaryDays: string = '31'
  iconButton = IconButton;
  classButton = ClassButton;

  selectedTab: string = ''

  generation_Param: NewEmployeesCompulsoryInsurancePremium_Param = <NewEmployeesCompulsoryInsurancePremium_Param>{}
  report_Param: NewEmployeesCompulsoryInsurancePremium_Param = <NewEmployeesCompulsoryInsurancePremium_Param>{}

  factoryList: KeyValuePair[] = [];
  generation_PermissionGroupList: KeyValuePair[] = [];
  report_PermissionGroupList: KeyValuePair[] = [];

  constructor(private service: S_6_1_4_NewEmployeesCompulsoryInsurancePremium) {
    super();
    this.programCode = this.route.snapshot.data['program'];
    this.translateService.onLangChange.pipe(takeUntilDestroyed()).subscribe(() => {
      this.title = this.functionUtility.getTitle(this.route.snapshot.data['program'])
      this.initTab()
      this.getDropDownList()
    });
  }

  ngOnInit(): void {
    this.title = this.functionUtility.getTitle(this.route.snapshot.data['program'])
    this.bsConfig = Object.assign(
      {},
      {
        isAnimated: true,
        dateInputFormat: 'YYYY/MM',
        minMode: this.minMode
      }
    );
    this.generation_Param = this.service.paramSearch().generation_Param;
    this.report_Param = this.service.paramSearch().report_Param;
    this.selectedTab = this.service.paramSearch().selected_Tab
    this.initTab()
    this.getDropDownList()
  }
  initTab() {
    this.tabs = [
      {
        id: 'generation',
        title: this.translateService.instant('CompulsoryInsuranceManagement.NewEmployeesCompulsoryInsurancePremium.InsuranceGeneration'),
        isEnable: true,
        content: this.generationForm
      },
      {
        id: 'report',
        title: this.translateService.instant('CompulsoryInsuranceManagement.NewEmployeesCompulsoryInsurancePremium.InsuranceReport'),
        isEnable: true,
        content: this.reportForm
      },
    ]
  }
  ngOnDestroy(): void {
    this.service.setParamSearch(<NewEmployeesCompulsoryInsurancePremium_Memory>{
      generation_Param: this.generation_Param,
      report_Param: this.report_Param,
      selected_Tab: this.selectedTab
    });
  }

  callFunction(func: string) {
    this[this.selectedTab + '_Param'].function_Type = func
    this[func]()
  }

  execute() {
    this.snotifyService.confirm(
      this.translateService.instant('CompulsoryInsuranceManagement.NewEmployeesCompulsoryInsurancePremium.ExecuteConfirm'),
      this.translateService.instant('System.Caption.Confirm'),
      () => {
        this.spinnerService.show();
        this.service.process(this[this.selectedTab + '_Param']).subscribe({
          next: (res) => {
            this.spinnerService.hide();
            if (res.isSuccess) {
              this.snotifyService.success(
                this.translateService.instant('System.Message.CreateOKMsg'),
                this.translateService.instant('System.Caption.Success')
              );
              this[this.selectedTab + '_Param'].total_Rows = res.data
            } else {
              this.snotifyService.error(
                this.translateService.instant(this.getTransKey('CompulsoryInsuranceManagement.NewEmployeesCompulsoryInsurancePremium', res.error)),
                this.translateService.instant('System.Caption.Error')
              );
            }
          },
          error: () => this.functionUtility.snotifySystemError()
        })
      }
    );
  }

  search() {
    this.spinnerService.show();
    this.service.process(this[this.selectedTab + '_Param']).subscribe({
      next: (res) => {
        this.spinnerService.hide();
        if (res.isSuccess) {
          this.snotifyService.success(
            this.translateService.instant('System.Message.SearchOKMsg'),
            this.translateService.instant('System.Caption.Success')
          );
          this[this.selectedTab + '_Param'].total_Rows = res.data
        } else {
          this.snotifyService.error(
            this.translateService.instant(this.getTransKey('CompulsoryInsuranceManagement.NewEmployeesCompulsoryInsurancePremium', res.error)),
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
      .process(this[this.selectedTab + '_Param'])
      .subscribe({
        next: (res) => {
          this.spinnerService.hide();
          if (res.isSuccess) {
            if (res.data.count > 0) {
              const fileName = this.functionUtility.getFileNameExport(this.programCode, 'Download')
              this.functionUtility.exportExcel(res.data.result, fileName);
            }
            this[this.selectedTab + '_Param'].total_Rows = res.data.count
          } else {
            this.snotifyService.error(
              this.translateService.instant(this.getTransKey('CompulsoryInsuranceManagement.NewEmployeesCompulsoryInsurancePremium', res.error)),
              this.translateService.instant('System.Caption.Error')
            );
          }
        },
        error: () => this.functionUtility.snotifySystemError()
      });
  }

  clearParam() {
    const initParam = structuredClone(this.service.initData[this.selectedTab + '_Param'])
    this[this.selectedTab + '_Param'] = initParam
  }

  getDropDownList() {
    this.service.getDropDownList(this[this.selectedTab + '_Param'])
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
    this[this.selectedTab + '_PermissionGroupList'] = structuredClone(keys.filter((x: { key: string; }) => x.key == "PE")).map(x => <KeyValuePair>{ key: x.key = x.value.substring(0, x.value.indexOf('-')), value: x.value })
    this.functionUtility.getNgSelectAllCheckbox(this[this.selectedTab + '_PermissionGroupList'])
  }
  changeTab() {
    this.getDropDownList()
  }

  onDateChange() {
    this[this.selectedTab + '_Param'].year_Month_Str = this.functionUtility.isValidDate(new Date(this[this.selectedTab + '_Param'].year_Month))
      ? this.functionUtility.getDateFormat(new Date(this[this.selectedTab + '_Param'].year_Month))
      : '';
  }

  onFactoryChange() {
    this.getDropDownList()
    this.deleteProperty('permission_Group')
  }
  onPaidSalaryDaysChange() {
    let value = this[this.selectedTab + '_Param'].paid_Salary_Days
    if (!this.functionUtility.checkEmpty(value)) {
      if (+value < +this.minSalaryDays)
        this[this.selectedTab + '_Param'].paid_Salary_Days = this.minSalaryDays
      if (+value > +this.maxSalaryDays)
        this[this.selectedTab + '_Param'].paid_Salary_Days = this.maxSalaryDays
    }
  }

  deleteProperty(name: string) {
    delete this[this.selectedTab + '_Param'][name]
  }

  getTransKey(parent: string, key: string): string {
    return this.functionUtility.hasTranslation(`${parent}.${key}`)
      ? `${parent}.${key}`
      : `${this.translateService.instant('CompulsoryInsuranceManagement.NewEmployeesCompulsoryInsurancePremium.InvalidErrorCode')} : ${key}`
  }
}
