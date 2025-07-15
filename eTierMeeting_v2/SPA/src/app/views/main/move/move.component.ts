import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { Employee } from '../../../_core/_models/employee';
import { AlertifyService } from '../../../_core/_services/alertify.service';
import { DateInventoryService } from '../../../_core/_services/date-inventory.service';
import { EmployeeService } from '../../../_core/_services/employee.service';
import { MachineMoveService } from '../../../_core/_services/machine-move.service';
import { ScanModalComponent } from '../scan-modal/scan-modal.component';

@Component({
  selector: 'app-move',
  templateUrl: './move.component.html',
  styleUrls: ['./move.component.scss']
})

export class MoveComponent implements OnInit {
  employee: Employee;
  qrResultString: string;
  isScan = true;
  typeScan: string;
  empNumber: string;
  empNumberFirst: string;
  empNamefirst: string;
  placeFirst: string;
  listPlnoFirst: any = [];
  empNumberLast: string;
  empNameLast: string;
  placeLast: string;
  listPlnoLast: any = [];
  idMachine: string;
  machineID: string;
  machineName: string;
  place: string;
  state: string;
  plno: string;
  fromEmploy: string;
  toEmploy: string;
  fromPlno: string;
  toPlno: string;
  lang: string;
  checkPlaceFirst: string;
  dateTime: Date = new Date();
  countDownDate: number = 0;

  @ViewChild('modelScan') modelScan: ScanModalComponent;
  constructor(
    private employeeService: EmployeeService,
    private router: Router,
    private alertifyService: AlertifyService,
    private machineMoveService: MachineMoveService,
    private translate: TranslateService,
    private dateIventory: DateInventoryService
  ) { }

  ngOnInit() {
    this.checkScheduleInventory();

  }

  checkScheduleInventory() {

    this.dateIventory.checkScheduleInventory().subscribe(res => {

      this.dateTime = res.item2;
      this.countDownDate = new Date(this.dateTime).getTime();

      const now = new Date().getTime();
      const distance = this.countDownDate - now;

      if (res.item1) {
        return this.router.navigate(['move']);
      }
      if (res.item2 && distance > 0) {
        return this.router.navigate(['Error/MoveMachineBlocked']);
      }
      else {
        this.router.navigate(['/home'])
        // this.alertifyService.error(this.translate.instant('alert.alert_error_datetime_server'));
        return this.alertifyService.confirm(this.translate.instant('alert.alert_error'),
          this.translate.instant('alert.alert_error_datetime_server'), () => true);
      }
    });
  }

  getEmployee(empNumber: string, type: string) {
    this.employeeService.getEmployee(empNumber).subscribe(res => {
      if (res.isData) {
        if (type === 'first') {
          this.empNamefirst = res.dataEmployee.empName;
          this.listPlnoFirst = res.dataPlno.map(item => {
            return { id: item.plno, name: item.place };
          });
          if (this.listPlnoFirst.length > 0) {
            this.placeFirst = this.listPlnoFirst[0].id;
          }
        } else {
          this.empNameLast = res.dataEmployee.empName;
          this.listPlnoLast = res.dataPlno.map(item => {
            return { id: item.plno, name: item.place };
          });
          if (this.listPlnoLast.length > 0) {
            this.placeLast = this.listPlnoLast[0].id;
          }
        }
      } else {
        this.alertifyService.error(this.translate.instant('alert.alert_not_found_data'));
      }
    });
  }

  getMachine() {
    this.lang = localStorage.getItem('lang');
    if (localStorage.getItem('factory') === 'TSH' && this.idMachine.substr(0, 1) !== 'U') {
      this.idMachine = 'U' + this.idMachine;
    }

    this.machineMoveService.getMachine(this.idMachine, this.lang).subscribe(res => {
      if (res) {
        if (res.plno !== this.placeFirst) {
          return this.alertifyService.warning(this.translate.instant('alert.alert-not-permision'));
        } else {
          this.idMachine = res.ownerFty + res.machineID;
          this.machineName = res.machineName;
          this.place = res.place;
          this.state = res.state;
          this.checkPlaceFirst = res.plno;
        }
      } else {
        this.alertifyService.error(this.translate.instant('alert.alert_not_found_data'));
      }
    });
  }

  submitMove() {

    if (this.empNumberFirst == null || this.empNumberFirst === '') {
      return this.alertifyService.error(this.translate.instant('alert.alert_full_input'));
    } else if (this.idMachine == null || this.idMachine === '') {
      return this.alertifyService.error(this.translate.instant('alert.alert_full_input'));
    } else if (this.empNumberLast == null || this.empNumberLast === '') {
      return this.alertifyService.error(this.translate.instant('alert.alert_full_input'));
    } else if (this.placeFirst !== this.checkPlaceFirst) {
      return this.alertifyService.warning(this.translate.instant('alert.alert-not-permision'));
    }

    this.fromEmploy = this.empNumberFirst;
    this.toEmploy = this.empNumberLast;
    this.fromPlno = this.placeFirst;
    this.toPlno = this.placeLast;

    this.dateIventory.checkScheduleInventory().subscribe(result => {
      if (!result.item1) {
        return this.alertifyService.error(this.translate.instant('movepage.machine_transfer_locked'));
      }
      this.machineMoveService.moveMachine(this.idMachine, this.fromEmploy, this.toEmploy, this.fromPlno, this.toPlno).subscribe(res => {
        if (res.status) {
          this.alertifyService.success(this.translate.instant('alert.alert-success-move'));
          this.clearFormMachine();
        } else {
          if (res.message !== undefined || res.message !== '') {
            this.alertifyService.error(res.message);
          } else {
            this.alertifyService.error(this.translate.instant('alert.alert-danger-move'));
          }
        }
      }, error => {
        this.alertifyService.error(this.translate.instant('alert.alert-danger-move'));
      });
    });

  }

  clearFormFirst() {
    this.empNumberFirst = '';
    this.empNamefirst = '';
    this.listPlnoFirst = [];
  }

  clearFormLast() {
    this.empNumberLast = '';
    this.empNameLast = '';
    this.listPlnoLast = [];
  }

  clearFormMachine() {
    this.idMachine = '';
    this.machineName = '';
    this.place = '';
    this.state = '';
  }

  openModalScan(type: string) {
    if (type === 'first') {
      this.clearFormFirst();
    }
    if (type === 'last') {
      this.clearFormLast();
    }
    if (type === 'machine') {
      this.clearFormMachine();
    }
    if (this.isScan) {
      this.typeScan = type;
      this.openModal();
      this.codeQr(this.qrResultString);
    }
  }

  openModal() {
    this.modelScan.showChildModal();
  }

  codeQr(event: string) {
    this.qrResultString = event;
    if (this.qrResultString !== undefined && this.qrResultString !== null) {
      if (this.typeScan === 'first') {
        this.empNumberFirst = this.qrResultString;
        this.getEmployee(this.empNumberFirst, 'first');
        this.qrResultString = null;
      }
      if (this.typeScan === 'machine') {
        this.idMachine = this.qrResultString;
        this.getMachine();
        this.qrResultString = null;
      }
      if (this.typeScan === 'last') {
        this.empNumberLast = this.qrResultString;
        this.getEmployee(this.empNumberLast, 'last');
        this.qrResultString = null;
      }
    }
  }
}
