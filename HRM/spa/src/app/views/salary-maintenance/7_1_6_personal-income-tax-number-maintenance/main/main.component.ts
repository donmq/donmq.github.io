import { S_7_1_6_PersonalIncomeTaxNumberMaintenanceService } from '@services/salary-maintenance/s_7_1_6_personal-income-tax-number-maintenance.service';
import { IconButton, Placeholder } from '@constants/common.constants';
import { PersonalIncomeTaxNumberMaintenanceDto, PersonalIncomeTaxNumberMaintenanceParam, PersonalIncomeTaxNumberMaintenanceSource } from '@models/salary-maintenance/7_1_6_personal-income-tax-number-maintenance';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { InjectBase } from '@utilities/inject-base-app';
import { Pagination } from '@utilities/pagination-utility';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { BsDatepickerConfig, BsDatepickerViewMode } from 'ngx-bootstrap/datepicker';
import { KeyValuePair } from '@utilities/key-value-pair';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent extends InjectBase implements OnInit, OnDestroy {
  title: string = '';
  programCode: string = '';
  pagination: Pagination = <Pagination>{};
  minMode: BsDatepickerViewMode = 'year';
  bsConfig: Partial<BsDatepickerConfig> = {
    dateInputFormat: 'YYYY',
    minMode: this.minMode
  };
  listFactory: KeyValuePair[] = [];
  param: PersonalIncomeTaxNumberMaintenanceParam = <PersonalIncomeTaxNumberMaintenanceParam>{};
  data: PersonalIncomeTaxNumberMaintenanceDto[] = [];
  selectedData: PersonalIncomeTaxNumberMaintenanceDto
  iconButton = IconButton;
  placeholder = Placeholder
  accept = '.xls, .xlsx, .xlsm';

  constructor(
    private service: S_7_1_6_PersonalIncomeTaxNumberMaintenanceService
  ) {
    super();
    this.programCode = this.route.snapshot.data['program'];
    this.translateService.onLangChange.pipe(takeUntilDestroyed()).subscribe(() => {
      this.title = this.functionUtility.getTitle(this.route.snapshot.data['program'])
      this.getListFactory();
      this.processData()
    });
  }

  ngOnInit(): void {
    this.title = this.functionUtility.getTitle(this.route.snapshot.data['program'])
    this.getSource()
  }

  ngOnDestroy(): void {
    this.service.setParamSearch(<PersonalIncomeTaxNumberMaintenanceSource>{
      param: this.param,
      pagination: this.pagination,
      selectedData: this.selectedData,
      data: this.data
    });
  }
  getSource() {
    const { param, pagination, data } = this.service.paramSearch();
    this.param = param;
    this.pagination = pagination;
    this.data = data;
    this.getListFactory();
    this.processData()
  }
  processData() {
    if (this.data.length > 0) {
      if (this.functionUtility.checkFunction('Search') && this.checkRequiredParams()) {
        this.getData()
      }
      else
        this.clear()
    }
  }
  checkRequiredParams(): boolean {
    return !this.functionUtility.checkEmpty(this.param.factory);
  }

  //#region getEmployeeData
  getListFactory() {
    this.service.getListFactory().subscribe({
      next: (res) => {
        this.listFactory = res;
      },
      error: () => this.functionUtility.snotifySystemError(false)
    });
  }
  //#endregion

  //#region getData
  getData(isSearch?: boolean, isDelete?: boolean, isUpload?: boolean) {
    this.spinnerService.show();
    this.service.getData(this.pagination, this.param).subscribe({
      next: (res) => {
        this.spinnerService.hide();
        this.data = res.result;
        this.pagination = res.pagination;
        if (isSearch)
          this.functionUtility.snotifySuccessError(true, 'System.Message.QuerySuccess')
        if (isDelete)
          this.functionUtility.snotifySuccessError(true, 'System.Message.DeleteOKMsg')
        if (isUpload)
          this.functionUtility.snotifySuccessError(true, 'System.Message.UploadOKMsg')
      },
      error: () => this.functionUtility.snotifySystemError()
    });
  }

  search(isSearch: boolean) {
    this.pagination.pageNumber === 1 ? this.getData(isSearch) : this.pagination.pageNumber = 1;
  }
  //#endregion

  //#region clear
  clear() {
    this.pagination.pageNumber = 1;
    this.pagination.totalCount = 0;
    this.param = <PersonalIncomeTaxNumberMaintenanceParam>{};
    this.data = [];
  }
  //#endregion

  //#region download
  download(isTemplate: boolean) {
    this.spinnerService.show();
    this.service.downloadExcel(this.param, isTemplate).subscribe({
      next: (res) => {
        if (res.isSuccess) {
          const fileName = !isTemplate
            ? this.functionUtility.getFileNameExport(this.programCode, 'Download')
            : this.functionUtility.getFileNameExport(this.programCode, 'Template')
          this.functionUtility.exportExcel(res.data, fileName);
        }
        else this.functionUtility.snotifySuccessError(false, res.error)
        this.spinnerService.hide();
      },
      error: () => this.functionUtility.snotifySystemError()
    });
  }
  //#endregion

  //#region upload
  upload(event: any, isUpload: boolean) {
    if (event.target.files && event.target.files[0]) {
      this.spinnerService.show();
      const fileNameExtension = event.target.files[0].name.split('.').pop();
      if (!this.accept.includes(fileNameExtension.toLowerCase())) {
        event.target.value = '';
        this.spinnerService.hide();
        return this.snotifyService.warning(
          this.translateService.instant('System.Message.AllowExcelFile'),
          this.translateService.instant('System.Caption.Error')
        );
      }
      const formData = new FormData();
      formData.append('file', event.target.files[0]);
      this.service.uploadExcel(formData).subscribe({
        next: (res) => {
          event.target.value = '';
          if (res.isSuccess) {
            if (!this.functionUtility.checkFunction('Search'))
              this.clear()
            else
              this.getData(false, false, isUpload);
          } else {
            if (!this.functionUtility.checkEmpty(res.data)) {
              const fileName = this.functionUtility.getFileNameExport(this.programCode, 'Report')
              this.functionUtility.exportExcel(res.data, fileName);
            }
            this.functionUtility.snotifySuccessError(false, res.error);
          }
          this.spinnerService.hide()
        },
        error: () => {
          event.target.value = '';
          this.functionUtility.snotifySystemError();
        }
      });
    }
  }
  //#endregion

  //#region add-edit-delete
  add() {
    this.router.navigate([`${this.router.routerState.snapshot.url}/add`]);
  }

  edit(item: PersonalIncomeTaxNumberMaintenanceDto) {
    this.selectedData = item
    this.router.navigate([`${this.router.routerState.snapshot.url}/edit`]);
  }

  delete(item: PersonalIncomeTaxNumberMaintenanceDto, isDelete: boolean) {
    this.functionUtility.snotifyConfirmDefault(() => {
      this.spinnerService.show();
      this.service.delete(item).subscribe({
        next: (res) => {
          if (res.isSuccess) this.getData(false, isDelete);
          else
            this.functionUtility.snotifySuccessError(false, 'System.Message.DeleteErrorMsg')
        },
        error: () => this.functionUtility.snotifySystemError()
      });
    });
  }
  //#endregion
  onDateChange() {
    this.param.year = this.functionUtility.isValidDate(new Date(this.param.year_Date)) ? new Date(this.param.year_Date).getFullYear().toString() : '';
  }

  pageChanged(event: any) {
    this.pagination.pageNumber = event.page;
    this.getData();
  }

  deleteProperty(name: string) {
    delete this.param[name];
  }

  validateNumber(event: any): boolean {
    const numberRegex = /^[0-9]+$/;
    return numberRegex.test(event.key);
  }
}
