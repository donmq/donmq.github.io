import { Component, ElementRef, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { IconButton, Placeholder } from '@constants/common.constants';
import { SalaryAdditionsAndDeductionsInput_Basic, SalaryAdditionsAndDeductionsInput_Param, SalaryAdditionsAndDeductionsInputDto } from '@models/salary-maintenance/7_1_19_salary-additions-and-deductions-input';
import { S_7_1_19_SalaryAdditionsAndDeductionsInputService } from '@services/salary-maintenance/s_7_1_19_salary-additions-and-deductions-input.service';
import { InjectBase } from '@utilities/inject-base-app';
import { KeyValuePair } from '@utilities/key-value-pair';
import { Pagination } from '@utilities/pagination-utility';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent extends InjectBase implements OnInit, OnDestroy {
  @ViewChild('inputRef') inputRef: ElementRef<HTMLInputElement>;
  title: string = '';
  programCode: string = '';
  param: SalaryAdditionsAndDeductionsInput_Param = <SalaryAdditionsAndDeductionsInput_Param>{};
  data: SalaryAdditionsAndDeductionsInputDto[] = [];
  source: SalaryAdditionsAndDeductionsInputDto;
  pagination: Pagination = <Pagination>{};
  iconButton = IconButton;
  placeholder = Placeholder
  listFactory: KeyValuePair[] = [];
  listAddDedType: KeyValuePair[] = [];
  listAddDedItem: KeyValuePair[] = [];
  listDepartment: KeyValuePair[] = [];
  dataTypeaHead: string[];
  sal_Month_Value: Date;
  extensions: string = '.xls, .xlsm, .xlsx';

  constructor(
    private service: S_7_1_19_SalaryAdditionsAndDeductionsInputService,
  ) {
    super();
    this.programCode = this.route.snapshot.data['program'];
    this.translateService.onLangChange.pipe(takeUntilDestroyed()).subscribe(() => {
      this.title = this.functionUtility.getTitle(this.route.snapshot.data['program'])
      this.loadDropdownList()
      this.processData()
    });
  }

  ngOnInit() {
    this.title = this.functionUtility.getTitle(this.route.snapshot.data['program'])
    this.getSource()
  }
  getSource() {
    this.param = this.service.paramSource().param;
    this.pagination = this.service.paramSource().pagination;
    this.data = this.service.paramSource().data;
    if (this.functionUtility.isValidDate(new Date(this.param.sal_Month)))
      this.sal_Month_Value = this.param.sal_Month.toDate();
    this.loadDropdownList()
    this.processData()
  }

  processData() {
    if (this.data.length > 0) {
      if (this.functionUtility.checkFunction('Search') && this.checkRequiredParams()) {
        this.getData(false)
      }
      else
        this.clear()
    }
  }
  loadDropdownList() {
    this.getListFactory();
    this.getListAddDedType();
    this.getListAddDedItem();
    this.getListDepartment();
    if (!this.functionUtility.checkEmpty(this.param.factory)) {
      this.getListDepartment();
    }
  }
  getListFactory() {
    this.spinnerService.show();
    this.service.getListFactory().subscribe({
      next: (res) => {
        this.listFactory = res
        this.spinnerService.hide();
      },
      error: () => this.functionUtility.snotifySystemError(),
    });
  }

  getListAddDedType() {
    this.spinnerService.show();
    this.service.getListAddDedType().subscribe({
      next: (res) => {
        this.listAddDedType = res
        this.spinnerService.hide();
      },
      error: () => this.functionUtility.snotifySystemError(),
    });
  }

  getListAddDedItem() {
    this.spinnerService.show();
    this.service.getListAddDedItem().subscribe({
      next: (res) => {
        this.listAddDedItem = res
        this.spinnerService.hide();
      },
      error: () => this.functionUtility.snotifySystemError(),
    });
  }

  getListDepartment() {
    this.spinnerService.show();
    this.service.getListDepartment(this.param.factory).subscribe({
      next: (res) => {
        this.listDepartment = res
        this.spinnerService.hide();
      },
      error: () => this.functionUtility.snotifySystemError(),
    });
  }

  ngOnDestroy(): void {
    this.service.setSource(<SalaryAdditionsAndDeductionsInput_Basic>{
      selectedData: this.source,
      param: this.param,
      data: this.data,
      pagination: this.pagination,
    });
  }

  getData(isSearch?: boolean) {
    if (this.sal_Month_Value.toString() == 'Invalid Date' || this.sal_Month_Value.toString() == 'NaN/NaN')
      return this.functionUtility.snotifySuccessError(false, 'SalaryMaintenance.SalaryAdditionsAndDeductionsInput.InvalidSalMonth')

    this.spinnerService.show();
    this.service.getData(this.pagination, this.param).subscribe({
      next: (res) => {
        this.spinnerService.hide();
        this.data = res.result;
        this.pagination = res.pagination;
        if (isSearch)
          this.functionUtility.snotifySuccessError(true, 'System.Message.QuerySuccess')
      },
      error: () => this.functionUtility.snotifySystemError(),
    });
  }

  search(isSearch: boolean) {
    this.pagination.pageNumber === 1 ? this.getData(isSearch) : this.pagination.pageNumber = 1;
  }

  pageChanged(event: any) {
    this.pagination.pageNumber = event.page;
    this.getData();
  }

  clear() {
    this.param = <SalaryAdditionsAndDeductionsInput_Param>{};
    this.sal_Month_Value = null;
    this.pagination.pageNumber = 1;
    this.pagination.totalCount = 0;
    this.data = [];
  }
  deleteProperty = (name: string) => delete this.param[name]

  onForm(item: SalaryAdditionsAndDeductionsInputDto = null) {
    this.source = item
    this.router.navigate([`${this.router.routerState.snapshot.url}/${item != null ? 'edit' : 'add'}`]);
  }
  delete(item: SalaryAdditionsAndDeductionsInputDto) {
    this.snotifyService.confirm(
      this.translateService.instant('System.Message.ConfirmDelete'),
      this.translateService.instant('System.Action.Delete'),
      () => {
        this.spinnerService.show();
        this.service.delete(item).subscribe({
          next: (result) => {
            if (result.isSuccess) {
              this.functionUtility.snotifySuccessError(result.isSuccess, result.error)
              this.getData(false);
            }
            else this.functionUtility.snotifySuccessError(result.isSuccess, result.error)
          },
          error: () => this.functionUtility.snotifySystemError(),
          complete: () => this.spinnerService.hide()
        });
      }
    );
  }

  checkRequiredParams() {
    return this.param.factory != null && this.sal_Month_Value != null
  }

  download() {
    this.spinnerService.show();
    this.service.download(this.param).subscribe({
      next: (result) => {
        this.spinnerService.hide();
        const fileName = this.functionUtility.getFileNameExport(this.programCode, 'Download')
        result.isSuccess
          ? this.functionUtility.exportExcel(result.data, fileName)
          : this.functionUtility.snotifySuccessError(result.isSuccess, result.error)
      },
      error: () => this.functionUtility.snotifySystemError(),
    });
  }
  downloadExcelTemplate() {
    this.spinnerService.show();
    this.service.downloadTemplate().subscribe({
      next: (result) => {
        this.spinnerService.hide();
        const fileName = this.functionUtility.getFileNameExport(this.programCode, 'Template')
        this.functionUtility.exportExcel(result.data, fileName)
      },
      error: () => this.functionUtility.snotifySystemError(),
    });
  }

  upload(event: any) {
    if (event.target.files && event.target.files[0]) {
      this.spinnerService.show();
      const fileNameExtension = event.target.files[0].name.split('.').pop();
      if (!this.extensions.includes(fileNameExtension.toLowerCase())) {
        event.target.value = '';
        this.spinnerService.hide();
        return this.snotifyService.warning(
          this.translateService.instant('System.Message.AllowExcelFile'),
          this.translateService.instant('System.Caption.Warning')
        );
      }
      const formData = new FormData();
      formData.append('file', event.target.files[0]);
      this.service.upload(formData).subscribe({
        next: (res) => {
          event.target.value = '';
          if (res.isSuccess) {
            if (!this.functionUtility.checkFunction('Search'))
              this.clear();
            else
              this.getData();
            this.functionUtility.snotifySuccessError(true, 'System.Message.UploadOKMsg');
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

  onChangeSalMonth() {
    this.param.sal_Month = this.functionUtility.isValidDate(this.sal_Month_Value)
      ? this.functionUtility.getDateFormat(this.sal_Month_Value)
      : "";
  }
  onFactoryChange() {
    this.getListDepartment()
  }
}
