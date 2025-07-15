import { Component, OnInit, ViewChild } from '@angular/core';
import { UntypedFormControl } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { Select2OptionData } from 'ng-select2';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { debounceTime, startWith, switchMap, tap } from 'rxjs/operators';
import { Employee } from '../../../../_core/_models/employee';
import { EmployAdmin } from '../../../../_core/_models/employee-admin';
import { OperationResult } from '../../../../_core/_models/operation-result';
import { Pagination, PaginationResult } from '../../../../_core/_models/pagination';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { CellPlnoService } from '../../../../_core/_services/cellplno.service';
import { EmployeeService } from '../../../../_core/_services/employee.service';
import { SweetAlertService } from '../../../../_core/_services/sweet-alert.service';

@Component({
  selector: 'app-employyee-home',
  templateUrl: './employyee-home.component.html',
  styleUrls: ['./employyee-home.component.scss']
})
export class EmployyeeHomeComponent implements OnInit {
  @ViewChild('childModal', { static: false }) childModal: ModalDirective;
  listPlnos: Array<Select2OptionData>;
  listEmploy: EmployAdmin[] = [];
  lang: string;
  items = [];
  keyword: UntypedFormControl = new UntypedFormControl("");
  currentPage: number = 0;
  employeeUpdate: Employee = {
    iD: 0,
    empName: '',
    empNumber: '',
    visible: true,
    listPlnoEmploy: [],
  };

  listValueUpdate: string[] = [];

  pagination: Pagination = {
    currentPage: 1,
    pageSize: 5,
    totalCount: 0,
    totalPage: 0,
  };
  constructor(private _employeeService: EmployeeService,
    private _cellPlnoService: CellPlnoService,
    private _sweetAlert: SweetAlertService,
    private translate: TranslateService,
    private _alertifyService: AlertifyService,
    private _spinner: NgxSpinnerService
  ) { }

  ngOnInit() {
    this.searchDelay();
    this.getAllPlno();
  }

  getAllPlno() {
    this._cellPlnoService.getAllCellPlno().subscribe(res => {
      this.listPlnos = res.map(item => {
        return { id: item.plno.trim() + '-' + item.place.trim(), text: item.plno.trim() + '-' + item.place.trim() };
      });
    });
  }

  searchDelay() {
    this.keyword.valueChanges.pipe(
      tap(() => {
        this._spinner.show("from-list");
        this.pagination.currentPage = 1;
      }),
      debounceTime(500),
      startWith(""),
      switchMap(res => {
        return this._employeeService.searchEmployee(res, this.pagination);
      }),
    ).subscribe(val => {
      this.listEmploy = val.result;
      this.pagination = val.pagination;
      this.currentPage = this.pagination.currentPage;
      this._spinner.hide("from-list");
    })
  }

  pageChanged(event: any): void {
    this.keyword.patchValue(this.keyword.value);
  }

  showChildModal(item: EmployAdmin): void {
    this.employeeUpdate.empName = item.empName;
    this.employeeUpdate.empNumber = item.empNumber;
    this.employeeUpdate.iD = item.iD;
    this.employeeUpdate.visible = item.visible;
    this.listValueUpdate = item.listPlnoEmploy.map(items => {
      return items.name.trim();
    });
    this.childModal.show();
  }

  hideChildModal() {
    this.childModal.hide();
  }

  updateEmployee() {
    this.lang = localStorage.getItem('lang');
    this.modifyListPlanoUpdate(this.listValueUpdate);
    if (this.employeeUpdate.empName === '') {
      return this._sweetAlert.error(this.translate.instant('alert.admin.please_enter_a_manager_name'));
    }
    if (this.listValueUpdate.length === 0) {
      return this._sweetAlert.error(this.translate.instant('alert.admin.please_select_a_management_position'));
    }
    this._employeeService.updateEmployee(this.employeeUpdate, this.lang).subscribe(res => {
      if (res.success) {
        this._sweetAlert.success('Success', res.message);
        this.keyword.patchValue(this.employeeUpdate.empNumber);
      } else {
        this._sweetAlert.error('Error', res.message);
      }
    }, error => {
      this._alertifyService.error(this.translate.instant('error.system_error'));
    });
    this.hideChildModal();
  }

  removeEmployee(empNumber: string) {
    this.lang = localStorage.getItem('lang');
    this._sweetAlert.confirm('Delete', this.translate.instant('alert.admin.are_you_sure_you_want_to_delete_this_manager'), () => {
      this._employeeService.removeEmployee(empNumber, this.lang).subscribe((res: OperationResult) => {
        if (res.success) {
          this._sweetAlert.success('Success', res.message);
          this.keyword.patchValue("");

        } else {
          this._sweetAlert.error('Failed', res.message);
        }
      });
    });
  }

  clearSearch() {
    this.pagination.currentPage = 1;
    this.keyword.patchValue("");
  }

  modifyListPlanoUpdate(listValue: string[]) {
    this.employeeUpdate.listPlnoEmploy = [];
    listValue.map(item => {
      const itemPlnoEmploy = item.split('-');
      this.employeeUpdate.listPlnoEmploy.push({ name: item, plnoID: itemPlnoEmploy[0].trim(), place: itemPlnoEmploy[1].trim() });
    });
  }

  exportExcel() {
    this._employeeService.exportExcel();
  }

  keywordAdd(value: string) {
    this.keyword.setValue(value);
  }
}
