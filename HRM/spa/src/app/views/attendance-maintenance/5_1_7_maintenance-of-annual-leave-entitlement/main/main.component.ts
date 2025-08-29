import { Component, OnDestroy, OnInit } from '@angular/core';
import { InjectBase } from '@utilities/inject-base-app';
import { S_5_1_7_MaintenanceOfAnnualLeaveEntitlementService } from '@services/attendance-maintenance/s_5_1_7_maintenance-of-annual-leave-entitlement.service';
import {
  MaintenanceOfAnnualLeaveEntitlement,
  MaintenanceOfAnnualLeaveEntitlementMemory,
  MaintenanceOfAnnualLeaveEntitlementParam
} from '@models/attendance-maintenance/5_1_7_maintenance_of_annual_leave_entitlement';
import { Pagination } from '@utilities/pagination-utility';
import { ClassButton, IconButton } from '@constants/common.constants';
import { KeyValuePair } from '@utilities/key-value-pair';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { CaptionConstants } from '@constants/message.enum';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { ModalService } from '@services/modal.service';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent extends InjectBase implements OnInit, OnDestroy {
  title: string = '';
  programCode: string = '';
  params: MaintenanceOfAnnualLeaveEntitlementParam = <MaintenanceOfAnnualLeaveEntitlementParam>{};
  pagination: Pagination = <Pagination>{};
  datas: MaintenanceOfAnnualLeaveEntitlement[] = [];

  availableRange_Start: Date = null;
  availableRange_End: Date = null;

  iconButton = IconButton;
  classButton = ClassButton;

  factories: KeyValuePair[] = [];
  departments: KeyValuePair[] = [];
  leaveCodes: KeyValuePair[] = [];

  bsConfig: Partial<BsDatepickerConfig> = {
    isAnimated: true,
    dateInputFormat: 'YYYY/MM/DD',
  };

  colSpan: number = 0;

  annualLeaveClone: MaintenanceOfAnnualLeaveEntitlement = <MaintenanceOfAnnualLeaveEntitlement>{};

  accept = '.xls, .xlsx, .xlsm';


  constructor(
    private _service: S_5_1_7_MaintenanceOfAnnualLeaveEntitlementService,
    private modalService: ModalService
  ) {
    super();
    this.programCode = this.route.snapshot.data['program'];
    this.modalService.onHide.pipe(takeUntilDestroyed()).subscribe((res: any) => {
      if (res.isSave && this.functionUtility.checkFunction('Search') && this.checkRequiredParams())
        this.query(false);
    })
    this.translateService.onLangChange.pipe(takeUntilDestroyed()).subscribe(() => {
      this.title = this.functionUtility.getTitle(this.route.snapshot.data['program'])
      this.processData()
    });
  }

  ngOnInit(): void {
    this.title = this.functionUtility.getTitle(this.route.snapshot.data['program'])
    this.getSource()
  }
  getSource() {
    this.params = this._service.paramSearch().params;
    this.pagination = this._service.paramSearch().pagination;
    this.datas = this._service.paramSearch().datas;
    this.processData()
  }
  processData() {
    if (this.datas.length > 0) {
      if (this.functionUtility.checkFunction('Search') && this.checkRequiredParams())
        this.query(false)
      else
        this.clear()
    }
    this.getDropDownList()
  }
  getDropDownList() {
    this.getListFactory();
    this.getListLeaveCode();
    if (this.params.factory)
      this.getListDepartment();
  }
  ngOnDestroy(): void {
    let data: MaintenanceOfAnnualLeaveEntitlementMemory = <MaintenanceOfAnnualLeaveEntitlementMemory>{
      params: this.params,
      pagination: this.pagination,
      datas: this.datas
    }
    this._service.setParamSearch(data);
  }

  checkRequiredParams(): boolean {
    var result = !this.functionUtility.checkEmpty(this.params.factory)
    return result;
  }

  getListFactory() {
    this.spinnerService.show();
    this._service.getListFactory()
      .subscribe({
        next: (res) => {
          this.factories = res;;
          this.spinnerService.hide();
        },
        error: () => this.functionUtility.snotifySystemError()
      });
  }

  changeFactory() {
    this.departments = [];
    this.deleteProperty('department')
    if (this.params.factory)
      this.getListDepartment();
  }

  getListDepartment() {
    this.spinnerService.show();
    this._service.getListDepartment(this.params.factory)
      .subscribe({
        next: (res) => {
          this.departments = res
          this.spinnerService.hide();
        },
        error: () => this.functionUtility.snotifySystemError()
      });
  }

  getListLeaveCode() {
    this.spinnerService.show();
    this._service.getListLeaveCode()
      .subscribe({
        next: (res) => {
          this.leaveCodes = res;
          this.spinnerService.hide();
        },
        error: () => this.functionUtility.snotifySystemError()
      });
  }

  //#region query data
  query = (isSearch: boolean) => {
    return new Promise<void>((resolve, reject) => {
      this.spinnerService.show();
      if (this.availableRange_Start && !this.availableRange_End || !this.availableRange_Start && this.availableRange_End) {
        this.spinnerService.hide();
        this.snotifyService.warning('Please enter Available Range Start and End', CaptionConstants.WARNING);
        reject()
      }
      else {
        this.params.availableRange_Start = this.availableRange_Start ? new Date(this.availableRange_Start).toStringDate() : '';
        this.params.availableRange_End = this.availableRange_End ? new Date(this.availableRange_End).toStringDate() : '';
        this._service.query(this.pagination, this.params)
          .subscribe({
            next: (res) => {
              this.datas = res.result;
              this.datas.map(x => {
                x.annual_Start_Date = new Date(x.annual_Start);
                x.annual_End_Date = new Date(x.annual_End);
              })
              this.pagination = res.pagination;
              if (isSearch)
                this.functionUtility.snotifySuccessError(true, 'System.Message.QuerySuccess')
              this.spinnerService.hide();
              resolve()
            },
            error: () => {
              this.functionUtility.snotifySystemError()
              reject()
            }
          });
      }
    })
  };
  search = (isSearch?: boolean) => {
    this.pagination.pageNumber == 1 ? this.query(isSearch ?? true) : this.pagination.pageNumber = 1;
  };

  pageChanged(event: any) {
    this.pagination.pageNumber = event.page;
    this.query(false);
  }
  //#endregion

  //#region add
  add() {
    const data = {
      model: <MaintenanceOfAnnualLeaveEntitlement>{
        previous_Hours: "0",
        year_Hours: "0",
        total_Hours: 0,
        total_Days: 0
      },
      isEdit: false,
    }
    this.modalService.open(data);
  }
  //#endregion

  //#region edit
  edit(item: MaintenanceOfAnnualLeaveEntitlement) {
    this._service.checkExistedData(item)
      .subscribe(result => {
        if (result.isSuccess) {
          const data = {
            model: <MaintenanceOfAnnualLeaveEntitlement>{ ...item },
            isEdit: true
          }
          this.modalService.open(data);
        } else {
          this.functionUtility.snotifySuccessError(false, 'AttendanceMaintenance.MaintenanceOfAnnualLeaveEntitlement.NotExitedData')
        }
      })
  }

  changeLeaveCodeItem(item: MaintenanceOfAnnualLeaveEntitlement) {
    item.leave_Code_Name = this.leaveCodes.find(x => x.key == item.leave_Code).value;
  }
  //#endregion

  //#region delete
  delete(item: MaintenanceOfAnnualLeaveEntitlement) {
    this.snotifyService.confirm(
      this.translateService.instant('System.Message.ConfirmDelete'),
      this.translateService.instant('System.Caption.Confirm'),
      () => {
        this.spinnerService.show();
        this._service.delete(item).subscribe({
          next: res => {
            if (res.isSuccess) {
              this.snotifyService.success(
                this.translateService.instant('System.Message.DeleteOKMsg'),
                this.translateService.instant('System.Caption.Success')
              );
              if (this.pagination.pageNumber > 1 && (this.pagination.pageNumber - 1) * this.pagination.pageSize >= this.pagination.totalCount - 1)
                this.pagination.pageNumber -= 1;
              this.query(false);
            } else {
              this.snotifyService.error(
                res.error ??
                this.translateService.instant('System.Message.DeleteErrorMsg'),
                this.translateService.instant('System.Caption.Error')
              );
            }
            this.spinnerService.hide();
          },
          error: () => this.functionUtility.snotifySystemError()
        });
      });
  }
  //#endregion

  //#region upload
  upload(event: any) {
    if (event.target.files && event.target.files[0]) {
      this.spinnerService.show();
      const fileNameExtension = event.target.files[0].name.split('.').pop();
      if (!this.accept.includes(fileNameExtension.toLowerCase())) {
        event.target.value = '';
        this.spinnerService.hide();
        return this.snotifyService.warning(
          'System.Message.AllowExcelFile',
          'System.Caption.Error'
        );
      }
      this._service.uploadExcel(event.target.files[0],).subscribe({
        next: (res) => {
          event.target.value = '';
          this.spinnerService.hide();
          if (res.isSuccess) {
            this.snotifyService.success(
              this.translateService.instant('System.Message.UploadOKMsg'),
              this.translateService.instant('System.Caption.Success')
            );
            this.functionUtility.checkFunction('Search') ? this.search(false) : this.clear();
          } else {
            if (!this.functionUtility.checkEmpty(res.data)) {
              const fileName = this.functionUtility.getFileNameExport(this.programCode, 'Report')
              this.functionUtility.exportExcel(res.data, this.functionUtility.getFileName(fileName));
            }
            this.snotifyService.error(
              res.error ?? this.translateService.instant('System.Message.UploadErrorMsg'),
              this.translateService.instant('System.Caption.Error')
            );
          }
        },
        error: () => {
          event.target.value = '';
          this.spinnerService.hide();
          this.functionUtility.snotifySystemError()
        }
      });
    }
  }
  //#endregion

  //#region download
  exportExcel() {
    this.spinnerService.show();
    this._service.exportExcel().subscribe({
      next: (result: Blob) => {
        this.spinnerService.hide();
        const fileName = this.functionUtility.getFileNameExport(this.programCode, 'Template')
        this.functionUtility.exportExcel(result, fileName)
      },
      error: () => this.functionUtility.snotifySystemError(),
    });
  }
  download() {
    return new Promise<void>((resolve, reject) => {
      this.spinnerService.show();
      if (this.availableRange_Start && !this.availableRange_End || !this.availableRange_Start && this.availableRange_End) {
        this.spinnerService.hide();
        this.snotifyService.warning('Please enter Available Range Start and End', CaptionConstants.WARNING);
        reject()
      }
      else {
        this.params.availableRange_Start = this.availableRange_Start ? new Date(this.availableRange_Start).toStringDate() : '';
        this.params.availableRange_End = this.availableRange_End ? new Date(this.availableRange_End).toStringDate() : '';
        this._service
          .downloadExcel(this.params)
          .subscribe({
            next: (result) => {
              this.spinnerService.hide();
              const fileName = this.functionUtility.getFileNameExport(this.programCode, 'Download')
              this.functionUtility.exportExcel(result.data, fileName);
            },
            error: () => this.functionUtility.snotifySystemError()
          });
      }
    })
  }
  //#endregion

  setQueryDate() {
    if (this.params.availableRange_Start)
      this.availableRange_Start = new Date(this.params.availableRange_Start);
    if (this.params.availableRange_End)
      this.availableRange_End = new Date(this.params.availableRange_End);
  }

  //#region clear
  clear() {
    this.params = <MaintenanceOfAnnualLeaveEntitlementParam>{ language: localStorage.getItem(LocalStorageConstants.LANG) }
    this.availableRange_Start = null;
    this.availableRange_End = null;
    this.departments = [];
    this.datas = [];
    this.pagination = <Pagination>{
      pageNumber: 1,
      pageSize: 10,
      totalCount: 0,
      totalPage: 0
    };
  }
  //#endregion

  //#region disable
  disable(item: MaintenanceOfAnnualLeaveEntitlement) {
    item.isDisabled = !item.annual_Start || !item.annual_End || !item.employee_ID || !item.leave_Code || item.previous_Hours == null || item.year_Hours == null;
  }
  //#endregion
  deleteProperty(name: string) {
    delete this.params[name]
  }
}
