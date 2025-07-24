import { Component, OnInit, ViewChild } from '@angular/core';
import { BsModalRef, ModalDirective } from 'ngx-bootstrap/modal';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { Employee, EmployeeDetalParam, EmployeeRedirect } from '@models/manage/employee-manage/employee';
import { EmployExport } from '@models/manage/employee-manage/employExport';
import { LeaveData } from '@models/manage/employee-manage/leaveData';
import { EmployeeService } from '@services/manage/employee.service';
import { OperationResult } from '@utilities/operation-result';
import { Pagination } from '@utilities/pagination-utility';
import { KeyValuePair } from '@utilities/key-value-pair';
import { DestroyService } from '@services/destroy.service';
import { takeUntil } from 'rxjs';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-employee-detail',
  templateUrl: './employee-detail.component.html',
  styleUrls: ['./employee-detail.component.scss'],
  providers: [DestroyService]
})
export class EmployeeDetailComponent extends InjectBase implements OnInit {
  detailEmployee: EmployExport = <EmployExport>{
  };
  employee: Employee = <Employee>{
  };
  listCateLog: KeyValuePair[] = [];
  listYear = [];
  searchList: LeaveData[] = [];
  param: EmployeeDetalParam = <EmployeeDetalParam>{}; 
  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 20
  };
  EmployeeId: number;
  CategoryId: number = 0;
  dateIn: Date = null;
  YearTo: number = new Date().getFullYear();
  YearFrom: number = new Date().getFullYear();
  lang: string = localStorage.getItem(LocalStorageConstants.LANG).toLowerCase();
  listGroupBase = [];
  listPositionID = [];
  listPartID = [];
  listDeptID = [];
  public visibleData = [
    {
      key: true,
      value: true
    },
    {
      key: false,
      value: false
    }
  ]

  linkRedirect: string;
  returnUrl: EmployeeRedirect[] = [
    { key: 'empList', url: '/manage/employee' },
    { key: 'seahrEmpManager', url: '/seahr/emp-management' }
  ]
  public yearValue: number = new Date().getFullYear()

  constructor(
    private employeeService: EmployeeService,
  ) {
    super()
  }
  ngOnInit(): void {
    this.detailEmployee.empID = Number(this.route.snapshot.paramMap.get('id'));
    this.getLinkUrlRedirect();
    this.pushYearData();
    this.getDetail(this.detailEmployee.empID, this.lang);
    this.getListCateLog(this.lang);
    this.searchDetail();
    this.getListDeptID();
    this.getListGroupBase();
    this.getListPositionID();

    this.translateService.onLangChange.pipe(takeUntil(this.destroyService.destroys$)).subscribe({
      next: (event) => {
        this.lang = event.lang === 'zh' ? 'zh-TW' : event.lang;
        this.getDetail(this.detailEmployee.empID, this.lang)
        this.getListCateLog(this.lang);
        this.searchDetail();
      }
    });
  }

  /**
 * Trả về trang muốn quay lại trước đó / Này ??
 *
 */
  getLinkUrlRedirect() {
    let key = this.route.snapshot.paramMap.get('key');
    this.linkRedirect = this.returnUrl.filter(item => item.key == key)[0].url;
  }

  //           1. In ra Chi tiết
  getDetail(empID: number, lang: string) {
    this.spinnerService.show();
    lang = this.lang;
    this.employeeService.getDetail(empID, lang).subscribe(
      {
        next: (res) => {
          this.detailEmployee = res;
          this.getListPartID(this.detailEmployee.deptID);
          this.spinnerService.hide();
        },
        error: () => {
          this.snotifyService.error(this.translateService.instant('System.Message.LockErrorMsg'), this.translateService.instant('System.Caption.Error'));
          this.spinnerService.hide();
        }

      });
  }

  //            2.Button
  // Nút vô hiệu hóa
  get buttonTilte() {
    return this.detailEmployee.voHieu == "0" ? this.translateService.instant('System.Action.Disable') : this.translateService.instant('System.Action.Enable');
  }

  statusMultiLanguage(status: string): string {
    if (status == "Wait") return this.translateService.instant('Manage.EmployeeManage.Wait');
    if (status == "Approved") return this.translateService.instant('Manage.EmployeeManage.Approved');
    if (status == "Rejected") return this.translateService.instant('Manage.EmployeeManage.Rejected');
    if (status == "Finish") return this.translateService.instant('Manage.EmployeeManage.Finish');
  }

  disable() {
    this.employeeService.changeVisible(this.detailEmployee.empID).subscribe({
      next: (res: OperationResult) => {
        if (res.isSuccess) {
          this.snotifyService.success(this.translateService.instant('System.Message.UpdateOKMsg'), this.translateService.instant('System.Caption.Success'));
          this.getDetail(this.detailEmployee.empID, this.lang);
        }
        else {
          this.snotifyService.error(this.translateService.instant('System.Message.UpdateErrorMsg'), this.translateService.instant('System.Caption.Error'));
        }
      },
      error: () => {
        this.snotifyService.error(this.translateService.instant('System.Message.UpdateErrorMsg'), this.translateService.instant('System.Caption.Error'))
      }
    });
  }

  //Nút Xóa
  modalRef?: BsModalRef;
  delete(id: number) {
    this.spinnerService.show();
    this.employeeService.remove(id).subscribe({
      next: (res: OperationResult) => {
        if (res.isSuccess) {
          this.snotifyService.success(this.translateService.instant('System.Message.DeleteOKMsg'), this.translateService.instant('System.Caption.Success'));
          this.back();
        }
        else {
          this.snotifyService.error(this.translateService.instant('System.Message.DeleteErrorMsg'), this.translateService.instant('System.Caption.Error'));
          this.spinnerService.hide()
          this.modalRef.hide();
        }
      },
      error: () => {
        this.snotifyService.error(this.translateService.instant('System.Message.DeleteErrorMsg'), this.translateService.instant('System.Caption.Error'));
        this.spinnerService.hide()
        this.modalRef.hide();
      }
    });

  }

  deleteItem(id: number) {
    this.snotifyService.confirm('Are you sure you want to delete the data ? ', 'Delete',
      () => {
        this.delete(id)
      }), () => {
        this.modalRef.hide()
      }
  }


  parseDateString(dateString: string): Date {
    const dateParts = dateString.split('/');
    const day = parseInt(dateParts[0], 10);
    const month = parseInt(dateParts[1], 10) - 1;
    const year = parseInt(dateParts[2], 10);

    return new Date(year, month, day);
  }

  //Nút Edit
  // Mở modal edit detail
  @ViewChild('modalEdit', { static: false }) modalEdit?: ModalDirective;
  editData?: EmployExport = <EmployExport>{};
  openModal(employ: EmployExport) {
    this.editData = { ...employ };
    this.modalEdit?.show();
    this.dateIn = this.parseDateString(this.editData.dateIn)
  }

  reset() {
    this.detailEmployee = { ...this.editData };
    this.getListPartID(this.detailEmployee.deptID);
    this.dateIn = this.parseDateString(this.editData.dateIn)
  }

  //Lưu lại
  saveEditInDetail() {
    this.spinnerService.show();
    this.detailEmployee.dateIn = this.functionUtility.getDateTimeFormat(this.dateIn)
    this.employeeService.updateInDetail(this.detailEmployee).subscribe({
      next: (res) => {
        this.modalEdit?.hide();
        if (res.isSuccess === true) {
          this.getDetail(this.detailEmployee.empID, this.lang);
          this.snotifyService.success(this.translateService.instant('System.Message.UpdateOKMsg'), this.translateService.instant('System.Caption.Success'));
        }
        else this.snotifyService.error(this.translateService.instant('System.Message.UpdateErrorMsg'), this.translateService.instant('System.Caption.Error'));
      },
      error: () => {
        this.snotifyService.error(this.translateService.instant('System.Message.UpdateErrorMsg'), this.translateService.instant('System.Caption.Error'));
        this.spinnerService.hide();
      }
    });
    this.spinnerService.hide();
  }

  //3. Danh sách nghỉ phép
  //Thanh search theo catelog
  getListCateLog(lang: string) {
    this.spinnerService.show();
    this.employeeService.getListCatelog(lang).subscribe({
      next: (res) => {
        this.listCateLog = res
        this.listCateLog.unshift({
          key: 0,
          value: this.translateService.instant('Manage.EmployeeManage.LeaveCatalog')
        })
        this.spinnerService.hide();
      }
    });
  }

  //Thanh search theo năm
  pushYearData() {
    for (var i = 2017; i <= this.yearValue; i++) {
      this.listYear.push({
        key: i,
        value: i
      });
    }
  }
  //Danh sách search
  searchDetail() {
    this.param.EmployeeId = this.detailEmployee.empID
    this.param.YearTo = this.YearTo === null ? new Date().getFullYear() : this.YearTo;
    this.param.YearFrom = this.YearFrom === null ? new Date().getFullYear() : this.YearFrom;
    this.param.CategoryId = this.CategoryId === null ? 0 : this.CategoryId;
    this.param.lang = this.lang;
    this.spinnerService.show();
    this.employeeService.searchDetail(this.pagination, this.param)
      .subscribe(
        {
          next: (res) => {
            this.searchList = res.result;
            this.pagination = res.pagination;
            this.spinnerService.hide();
          },
          error: () => {
            this.snotifyService.error(this.translateService.instant('System.Message.DataNotFound'), this.translateService.instant('System.Caption.Error'));
            this.spinnerService.hide();
          }

        })
  }
  pageChanged(event: any) {
    this.pagination.pageNumber = event.page;
    this.searchDetail();
  }
  back = () => this.router.navigate([this.linkRedirect]);

  //Xử lý khi search thì danh sách bên dưới hiển thị
  changeCate(args) {
    this.CategoryId = args
    this.searchDetail();
  }
  changeYearTo(args) {
    this.YearTo = args;
    this.searchDetail();
  }
  changeYearFrom(args) {
    this.YearFrom = args;
    this.searchDetail();
  }

  ///////////////Xử lý trong Nút Edit /////////////////////
  //Lấy data trong ng-select
  getListGroupBase() {
    this.spinnerService.show();
    this.employeeService.getListGroupBase().subscribe({
      next: (res) => {
        this.listGroupBase = res.map(item => {
          return {
            key: parseInt(item.key),
            value: item.value
          }
        })
        this.spinnerService.hide();
      }
    });
  }
  getListPositionID() {
    this.spinnerService.show();
    this.employeeService.getListPositionID().subscribe({
      next: (res) => {
        this.listPositionID = res.map(item => {
          return {
            key: parseInt(item.key),
            value: item.value
          }
        })
        this.spinnerService.hide();
      }
    });
  }
  getListPartID(deptID: number) {
    this.spinnerService.show();
    this.employeeService.getListPartID(deptID).subscribe({
      next: (res) => {
        this.listPartID = res
          .map(item => {
            return {
              key: parseInt(item.key),
              value: item.value
            }
          })
        this.listPartID.unshift({ key: 0, value: '' });
        this.spinnerService.hide();
      }

    });
  }
  getListDeptID() {
    this.spinnerService.show();
    this.employeeService.getListDeptID().subscribe({
      next: (res) => {
        this.listDeptID = res
          .map(item => {
            return {
              key: parseInt(item.key),
              value: item.value
            }
          }
          )
        this.listDeptID.unshift({ key: 0, value: '' });
        this.spinnerService.hide();
      }
    });
  }
  changeDept(deptID) {
    if (this.detailEmployee.deptID == deptID) {
      this.detailEmployee.partID = 0;
      this.getListPartID(deptID);
    }
  }

}
