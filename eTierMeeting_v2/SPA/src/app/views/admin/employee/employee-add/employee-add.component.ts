import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { Employee } from '../../../../_core/_models/employee';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { CellPlnoService } from '../../../../_core/_services/cellplno.service';
import { EmployeeService } from '../../../../_core/_services/employee.service';
import { SweetAlertService } from '../../../../_core/_services/sweet-alert.service';

@Component({
  selector: 'app-employee-add',
  templateUrl: './employee-add.component.html',
  styleUrls: ['./employee-add.component.scss']
})
export class EmployeeAddComponent implements OnInit {
  @Output() keyword = new EventEmitter<string>();

  lang: string;
  listValue: string[] = [];
  listPlnos: Array<Select2OptionData>;
  employee: Employee = {
    iD: 0,
    empName: '',
    empNumber: '',
    visible: true,
    listPlnoEmploy: [],
  };
  constructor(
    private _employeeService: EmployeeService,
    private _cellPlnoService: CellPlnoService,
    private _sweetAlert: SweetAlertService,
    private translate: TranslateService,
    private _alertifyService: AlertifyService,
  ) { }

  ngOnInit() {
    this.getAllPlno();
  }

  getAllPlno() {
    this._cellPlnoService.getAllCellPlno().subscribe(res => {
      this.listPlnos = res.map(item => {
        return { id: item.plno.trim() + '-' + item.place.trim(), text: item.plno.trim() + '-' + item.place.trim() };
      });
    });
  }

  addEmployee() {
    this.lang = localStorage.getItem('lang');
    this.modifyListPlano(this.listValue);
    if (this.employee.empName === '') {
      this.listValue = [];
      this.employee.listPlnoEmploy = [];
      return this._sweetAlert.error(this.translate.instant('alert.admin.please_enter_a_manager_name'));
    }
    if (this.employee.empNumber === '') {
      this.listValue = [];
      this.employee.listPlnoEmploy = [];
      return this._sweetAlert.error(this.translate.instant('alert.admin.please_enter_the_manager_code'));
    }
    if (this.employee.listPlnoEmploy.length === 0) {
      return this._sweetAlert.error(this.translate.instant('alert.admin.please_select_a_management_position'));
    }
    this._employeeService.addEmployee(this.employee, this.lang).subscribe(res => {
      if (res.success) {
        this._sweetAlert.success('Success', res.message);
        this.keyword.emit(this.employee.empNumber);
        this.clearFormAdd();
      } else {
        this._sweetAlert.error('Error', res.message);
      }
    }, error => {
      this._alertifyService.error(this.translate.instant('error.system_error'));
    });
    this.employee.listPlnoEmploy = [];
  }

  clearFormAdd() {
    this.employee.empName = '';
    this.employee.empNumber = '';
    this.listValue = [];
    this.employee.listPlnoEmploy = [];
  }

  modifyListPlano(listValue: string[]) {
    listValue.map(item => {
      const itemPlnoEmploy = item.split('-');
      this.employee.listPlnoEmploy.push({ name: item, plnoID: itemPlnoEmploy[0].trim(), place: itemPlnoEmploy[1].trim() });
    });
  }
}
