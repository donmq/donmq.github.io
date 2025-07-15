import { Component, OnInit, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { HistoryCheckMachine, ResultHistoryCheckMachine } from '../../../../_core/_models/export-pdf';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { CheckMachineService } from '../../../../_core/_services/check-machine.service';
import { SweetAlertService } from '../../../../_core/_services/sweet-alert.service';
import { ScanModalComponent } from '../../scan-modal/scan-modal.component';
@Component({
  selector: 'app-check-machine-home',
  templateUrl: './check-machine-home.component.html',
  styleUrls: ['./check-machine-home.component.scss']
})
export class CheckMachineHomeComponent implements OnInit {
  checkInternet: string = '1';
  scan: boolean = true;
  loadingData: boolean = false;
  getMachine: boolean = false;
  machineName: string;
  listCheckMachine: any = [];
  listResultCheckMachine: ResultHistoryCheckMachine[] = [];
  historyCheckMachine: HistoryCheckMachine[] = [];
  dataReport: ResultHistoryCheckMachine[] = [];
  qrResultString: string;
  lang: string;

  @ViewChild('modelScan') modelScan: ScanModalComponent;

  constructor(private _checkMachineService: CheckMachineService,
    private _spinnerService: NgxSpinnerService,
    private _alertifyService: AlertifyService,
    private _sweetAlertService: SweetAlertService,
    private translate: TranslateService
  ) { }

  ngOnInit() {
    const listCheckDataParseJson = localStorage.getItem('listCheckMachine');
    if (listCheckDataParseJson !== '' && listCheckDataParseJson !== null) {
      this.listCheckMachine = JSON.parse(listCheckDataParseJson);
    }
    this.checkGetMachineData();
  }

  scanManually() {
    this.scan = !this.scan;
  }

  // Get Machine Online
  getDataMachine() {
    this._spinnerService.show();
    this.lang = localStorage.getItem('lang');
    if (localStorage.getItem('factory') === 'TSH' && this.machineName.substr(0, 1) !== 'U') {
      this.machineName = 'U' + this.machineName;
    }
    const machine = {
      machineID: this.machineName.trim().substr(1, this.machineName.length),
      ownerFty: this.machineName.trim().substr(0, 1), flag: 1
    };

    if (this.checkInternet === '2' && this.machineName !== '') {
      if (this.checkMachineExits(machine)) {
        this.getMachine = true;
        this.listCheckMachine.unshift(machine);
        const listCheckMachineJson = JSON.stringify(this.listCheckMachine);
        localStorage.setItem('listCheckMachine', listCheckMachineJson);
      }
      this._spinnerService.hide();
      this.loadingData = false;
    } else {
      this._checkMachineService.getMachine(machine.ownerFty + machine.machineID, this.lang).subscribe(res => {
        this.loadingData === false;
        if (res.isNull === false || res.isNull === true) {
          if (this.checkMachineExits(machine)) {
            this.listCheckMachine.unshift(res);
            const listCheckMachineJson = JSON.stringify(this.listCheckMachine);
            localStorage.setItem('listCheckMachine', listCheckMachineJson);
          }
        } else {
          this._spinnerService.hide();
          this._alertifyService.error(this.translate.instant('alert.alert_not_found_data'));
        }

      }, error => {
        this._spinnerService.hide();
        this._alertifyService.error(this.translate.instant('error.system_error'));
      });
      this._spinnerService.hide();
    }
    this.machineName = '';
  }

  // Get Machine Offline
  getDataMachineOffline() {
    this.lang = localStorage.getItem('lang');
    this._spinnerService.show();
    if (navigator.onLine === true) {
      this.listCheckMachine.filter(item => {
        if (item.flag === 1) {
          this._checkMachineService.getMachine(item.ownerFty + item.machineID, this.lang).subscribe(res => {
            if (res.isNull === false || res.isNull === true) {
              const data = this.listCheckMachine.findIndex(x => x.machineID === item.machineID);
              this.listCheckMachine.splice(data, 1);
              this.listCheckMachine.unshift(res);
              const listCheckMachineJson = JSON.stringify(this.listCheckMachine);
              localStorage.setItem('listCheckMachine', listCheckMachineJson);
              this._alertifyService.success(this.translate.instant('inventory.alert_get_machine'));
              this.loadingData = false;
              this.checkGetMachineData();
              this._spinnerService.hide();
            } else {
              this._spinnerService.hide();
              this._alertifyService.error(this.translate.instant('alert.alert_not_found_data'));
            }
          }, error => {
            this._spinnerService.hide();
            this._alertifyService.error(this.translate.instant('error.system_error'));
          });
        }
      });
    } else {
      this.loadingData = false;
      this._alertifyService.success('');
      setTimeout(function () {
        window.location.reload();
      }, 2000);
      this._spinnerService.hide();
    }
    this.checkGetMachineData();
    this._spinnerService.hide();
  }

  clearData() {
    this._spinnerService.show();
    localStorage.removeItem('listCheckMachine');
    this._alertifyService.success(this.translate.instant('inventory.delete_all_data'));
    setTimeout(function () {
      window.location.reload();
    }, 1000);
    this._spinnerService.hide();
  }

  refresh() {
    window.location.reload();
  }

  checkGetMachineData() {
    this.getMachine = false;
    this.listCheckMachine.filter(item => {
      if (('flag' in item)) {
        this.getMachine = true;
      }
    });
  }

  submitCheckMachineAll() {
    this._spinnerService.show();
    this._checkMachineService.submitCheckMachineAll(this.listCheckMachine).subscribe(res => {
      this.loadingData = true;
      if (res.error !== 1) {
        this.listResultCheckMachine = res.listCheckMachine;
        this.historyCheckMachine = res.historyCheckMachine;
        this.dataReport = res;
        localStorage.removeItem('listCheckMachine');
        this._spinnerService.hide();
      } else {
        this._spinnerService.hide();
        this._alertifyService.error(this.translate.instant('alert.alert_error_occurred'));
      }
    }, error => {
      this._spinnerService.hide();
      this._alertifyService.error(this.translate.instant('error.system_error'));
    });
  }

  submitCheckMachine() {
    this._spinnerService.show();
    this._checkMachineService.submitCheckMachine(this.listCheckMachine).subscribe(res => {
      this.loadingData = true;
      if (res.error !== 1) {
        this.listResultCheckMachine = res.listCheckMachine;
        this.historyCheckMachine = res.historyCheckMachine;
        this.dataReport = res;
        localStorage.removeItem('listCheckMachine');
        this._spinnerService.hide();
      } else {
        this._spinnerService.hide();
        this._alertifyService.error(this.translate.instant('alert.alert_error_occurred'));
      }
    }, error => {
      this._spinnerService.hide();
      this._alertifyService.error(this.translate.instant('error.system_error'));
    });
  }

  deleteMachine(machineID) {
    this._sweetAlertService.confirm1(this.translate.instant('alert.do_you_want_delete'), this.translate.instant('alert.data_permanently_deleted'), () => {
      this.listCheckMachine.splice(machineID, 1);
      localStorage.setItem('listCheckMachine', JSON.stringify(this.listCheckMachine));
      this.checkGetMachineData();
    });
  }

  checkMachineExits(itemMachine: any): boolean {
    if (itemMachine === '') {
      this._alertifyService.error(this.translate.instant('inventory.Please_enter_the_machine_code'));
      return false;
    }
    const item = this.listCheckMachine.find(x => x.machineID.trim() === itemMachine.machineID.trim()
      && x.ownerFty.trim() === itemMachine.ownerFty.trim());
    if (item) {
      this._alertifyService.error(this.translate.instant('alert.alert_data_already_exists_scan_list'));
      this.machineName = '';
      return false;
    } else {
      return true;
    }
  }

  openScan() {
    this.machineName = '';
    this.modelScan.showChildModal();
    this.codeQr(this.qrResultString);
  }

  codeQr(event: string) {
    this.qrResultString = event;
    if (this.qrResultString !== undefined && this.qrResultString !== null) {
      this.machineName = this.qrResultString;
      this.getDataMachine();
      this.qrResultString = null
    }
  }
}
