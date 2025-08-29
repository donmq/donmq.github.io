import {
  Component,
  effect,
  EventEmitter,
  OnDestroy,
  OnInit,
} from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ClassButton, IconButton } from '@constants/common.constants';
import {
  D_8_1_2_EmployeeRewardPenaltyRecordsData,
  D_8_1_2_EmployeeRewardPenaltyRecordsParam,
  EmployeeRewardAndPenaltyRecords_ModalInputModel,
  EmployeeRewardAndPenaltyRecords_Memory,
  D_8_1_2_EmployeeRewardPenaltyRecordsSubParam,
} from '@models/reward-and-penalty-maintenance/8_1_2_employee-reward-and-penalty-records';
import { LangChangeEvent } from '@ngx-translate/core';
import { S_8_1_2_EmployeeRewardAndPenaltyRecordsService } from '@services/reward-and-penalty-maintenance/s_8_1_2_employee-reward-and-penalty-records.service';
import { InjectBase } from '@utilities/inject-base-app';
import { KeyValuePair } from '@utilities/key-value-pair';
import { Pagination } from '@utilities/pagination-utility';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss',
})
export class MainComponent extends InjectBase implements OnInit, OnDestroy {
  filesPassingEmitter: EventEmitter<EmployeeRewardAndPenaltyRecords_ModalInputModel> = new EventEmitter<EmployeeRewardAndPenaltyRecords_ModalInputModel>();

  acceptFormat: string = '.xls, .xlsx, .xlsm';
  selectedMquo: D_8_1_2_EmployeeRewardPenaltyRecordsSubParam;
  factoryList: KeyValuePair[] = []
  listDepartment: KeyValuePair[] = [];
  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 10,
    totalCount: 0,
  };
  bsConfig: Partial<BsDatepickerConfig> = <Partial<BsDatepickerConfig>>{
    dateInputFormat: 'YYYY/MM',
    minMode: 'month'
  };
  iconButton = IconButton;
  classButton = ClassButton;
  param: D_8_1_2_EmployeeRewardPenaltyRecordsParam = <D_8_1_2_EmployeeRewardPenaltyRecordsParam>{};
  selectedData: D_8_1_2_EmployeeRewardPenaltyRecordsSubParam = <D_8_1_2_EmployeeRewardPenaltyRecordsSubParam>{};
  data: D_8_1_2_EmployeeRewardPenaltyRecordsData[] = [];
  title: string = '';
  programCode: string = '';
  constructor(
    private service: S_8_1_2_EmployeeRewardAndPenaltyRecordsService,
  ) {
    super();
    this.programCode = this.route.snapshot.data['program'];
    this.translateService.onLangChange.pipe(takeUntilDestroyed()).subscribe(() => {
      this.title = this.functionUtility.getTitle(this.route.snapshot.data['program'])
      this.loadDropDownList()
      this.processData()
    });
  }

  ngOnInit(): void {
    this.title = this.functionUtility.getTitle(this.route.snapshot.data['program'])
    this.getSource();
  }

  getSource() {
    this.param = this.service.paramSearch().param;
    this.pagination = this.service.paramSearch().pagination;
    this.data = this.service.paramSearch().data;
    this.loadDropDownList();
    this.processData()
  }
  processData() {
    if (this.data.length > 0) {
      if (this.functionUtility.checkFunction('Search') && this.param.factory) {
        this.getData(false)
      }
      else
        this.clear()
    }
  }
  ngOnDestroy(): void {
    this.service.setParamSearch(<EmployeeRewardAndPenaltyRecords_Memory>{
      param: this.param,
      pagination: this.pagination,
      selectedData: this.selectedData,
      data: this.data,
    });
  }

  search = () => {
    this.pagination.pageNumber == 1 ? this.getData(true) : (this.pagination.pageNumber = 1);
  };

  getData(isSearch: boolean = false) {
    this.spinnerService.show();
    this.service.getSearch(this.pagination, this.param).subscribe({
      next: (res) => {
        this.spinnerService.hide();
        this.data = res.result;
        this.pagination = res.pagination;
        if (isSearch)
          this.functionUtility.snotifySuccessError(true, 'System.Message.QuerySuccess')
      },
      error: () => this.functionUtility.snotifySystemError()
    });
  }

  loadDropDownList() {
    this.getListFactory();
    if (this.param.factory)
      this.getListDepartment();
  }

  onDateChange(name: string) {
    this.param[`${name}_Str`] = this.param[name] ? this.functionUtility.getDateFormat(new Date(this.param[name])) : '';
  }
  upload(event: any) {
    if (event.target.files && event.target.files[0]) {
      this.spinnerService.show();
      const fileNameExtension = event.target.files[0].name.split('.').pop();
      if (!this.acceptFormat.includes(fileNameExtension.toLowerCase())) {
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
              this.getData();
            this.functionUtility.snotifySuccessError(true, 'System.Message.UploadOKMsg')
          } else {
            if (!this.functionUtility.checkEmpty(res.data)) {
              const fileName = this.functionUtility.getFileNameExport(this.programCode, 'Report')
              this.functionUtility.exportExcel(res.data, fileName);
            }
            this.functionUtility.snotifySuccessError(res.isSuccess, res.error)
          }
        },
        error: () => {
          event.target.value = '';
          this.functionUtility.snotifySystemError();
        }
      }).add(() => this.spinnerService.hide());
    }
  }
  exceltemp() {
    this.spinnerService.show();
    this.service.downloadTemplate().subscribe({
      next: (result) => {
        console.log(result);
        this.spinnerService.hide();
        const base64 = result.data.split(',')[1];
        const fileName = this.functionUtility.getFileNameExport(this.programCode, 'Template')
        this.functionUtility.exportExcel(base64, fileName)
      },
      error: () => this.functionUtility.snotifySystemError(),
    });
  }
  deleteProperty(name: string) {
    delete this.param[name]
  }
  checkRequiredParams(): boolean {
    return !this.functionUtility.checkEmpty(this.param.factory)
  }

  clear() {
    this.param = <D_8_1_2_EmployeeRewardPenaltyRecordsParam>{}
    this.pagination.pageNumber = 1;
    this.pagination.totalCount = 0;
    this.data = [];
  }
  remove(item: D_8_1_2_EmployeeRewardPenaltyRecordsData) {
    this.snotifyService.confirm(
      this.translateService.instant('System.Message.ConfirmDelete'),
      this.translateService.instant('System.Action.Delete'),
      () => {
        this.spinnerService.show();
        this.service.deleteData(item).subscribe({
          next: (result) => {
            if (result.isSuccess) {
              this.getData();
              this.functionUtility.snotifySuccessError(true, 'System.Message.DeleteOKMsg')
            }
            else this.functionUtility.snotifySuccessError(result.isSuccess, result.error)
          },
          error: () => this.functionUtility.snotifySystemError(),
          complete: () => this.spinnerService.hide()
        });
      }
    );
  }
  getListFactory() {
    this.service.GetListFactory().subscribe({
      next: res => {
        this.factoryList = res
      }, error: () => this.functionUtility.snotifySystemError()
    })
  }
  getListDepartment() {
    this.service.GetListDepartment(this.param.factory).subscribe({
      next: res => {
        this.listDepartment = res
      }, error: () => this.functionUtility.snotifySystemError()
    })
  }
  onFactoryChange() {
    this.getListDepartment();
  }
  add() {
    this.router.navigate([`${this.router.routerState.snapshot.url}/add`]);
  }
  query(item: D_8_1_2_EmployeeRewardPenaltyRecordsData) {
    this.service.setParamForm(item.history_GUID);
    this.router.navigate([`${this.router.routerState.snapshot.url}/query`]);
  }
  edit(item: D_8_1_2_EmployeeRewardPenaltyRecordsData) {
    this.service.setParamForm(item.history_GUID);
    this.router.navigate([`${this.router.routerState.snapshot.url}/edit`]);
  }
  changePage = (e: PageChangedEvent) => {
    this.pagination.pageNumber = e.page;
    this.getData();
  };
}
