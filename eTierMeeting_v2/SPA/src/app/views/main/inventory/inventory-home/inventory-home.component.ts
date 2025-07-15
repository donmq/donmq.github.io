import { DatePipe } from '@angular/common';
import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { InventoryParams } from '../../../../_core/_dtos/inventory-params';
import { InventoryLine } from '../../../../_core/_models/inventoryLine';
import { User, UserForLoginDto } from '../../../../_core/_models/user';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { InventoryService } from '../../../../_core/_services/inventory.service';
import { SweetAlertService } from '../../../../_core/_services/sweet-alert.service';
import { ScanModalComponent } from '../../scan-modal/scan-modal.component';
import { InventoryHomeLoginComponent } from './inventory-home-login/inventory-home-login.component';

@Component({
  selector: 'app-inventory-home',
  templateUrl: './inventory-home.component.html',
  styleUrls: ['./inventory-home.component.scss']
})
export class InventoryHomeComponent implements OnInit {
  isCheckEnterValue: boolean = false;
  qrResultString: string;
  machineCode: string;
  checkInternet: string = '1';
  loadingData: boolean = false;
  listDataInventory: any = [];
  getMachine: boolean = false;
  isScan: true;
  idPlnos: string = 'all';
  inventoryID: string = '0';
  optonsInventory: number = 0;
  lang: string;
  lineInventory: InventoryLine = {
    plnoName: '',
    plnoId: '',
    place: '',
    timeSoKiem: '',
    timePhucKiem: '',
    timeRutKiem: '',
    pecenMatchPhucKiem: '',
    pecenMatchSoKiem: '',
    pecenMatchRutKiem: '',
    isTimeSoKiemValid: true,
    isTimePhucKiemValid: true,
    isTimeRutKiemValid: true,
  };
  listMachineInventory: any = [];

  inventoryParams: InventoryParams = {
    idInventory: 0,
    idPlno: '',
    fromDateTime: '',
    toDateTime: '',
    erorr: 0,
    inventoryID: 0,
    typeFile: '',
    listMachineInventory: this.listDataInventory,
  };

  showLogin: boolean = true;
  dataUser: User;
  user: UserForLoginDto = {
    password: '',
    username: '',
  };
  checkLogin: boolean = false;
  index = '';
  login: any = [];
  @ViewChild('inventoryHomeLogin') inventoryHomeLogin: InventoryHomeLoginComponent;
  @ViewChild('modelScan') modelScan: ScanModalComponent;

  fromDate: Date;

  constructor(private _inventoryService: InventoryService,
    private _alertifyService: AlertifyService,
    private _sweetAlertService: SweetAlertService,
    private _spinnerService: NgxSpinnerService,
    private translate: TranslateService,
    private _router: Router,
    private _datePipe: DatePipe,
    private _activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    this.fromDate = new Date();
    this._activatedRoute.paramMap.subscribe(res => {
      const optonsUrl = res.get('optonsLine');
      this.optonsInventory = +optonsUrl;
    });
    const data = JSON.parse(localStorage.getItem('listDataInventory'));
    if (data !== null) {
      this.listDataInventory = data;
    }
    this.checkGetMachineData();
    this._inventoryService.positionInventory.asObservable().subscribe(res => {
      if (res != null) {
        this.lineInventory = res;
      } else {
        this._router.navigateByUrl('/inventoryv2');
      }
    });
  }

  // Get Machine Online
  getDataMachine() {
    this.machineCode = this.machineCode.toUpperCase();
    this._spinnerService.show();
    this.lang = localStorage.getItem('lang');
    if (localStorage.getItem('factory') === 'TSH' && this.machineCode.substr(0, 1) !== 'U') {
      this.machineCode = 'U' + this.machineCode;
    }
    const machine = {
      machineID: this.machineCode.trim()
        .substr(1, this.machineCode.length), ownerFty: this.machineCode.trim().substr(0, 1), flag: 1
    };
    console.log(machine);

    if (this.checkInternet === '2' && this.machineCode !== '') {
      if (this.checkMachineExits(machine)) {
        this.getMachine = true;
        this.listDataInventory.unshift(machine);
        const listDataInventoryJson = JSON.stringify(this.listDataInventory);
        localStorage.setItem('listDataInventory', listDataInventoryJson);
        this.machineCode = '';
      }
      this.loadingData = false;
      this._spinnerService.hide();
    } else {
      this._inventoryService.getMachine(machine.ownerFty + machine.machineID, this.lang).subscribe(
        val => {
          if (val.isNull === false) {
            if (this.checkMachineExits(machine)) {
              if (val.machineID === null) {
                val.machineID = this.machineCode;
              }
              this.listDataInventory.unshift(val);
              this.machineCode = '';
              // save local storage
              localStorage.setItem('listDataInventory', JSON.stringify(this.listDataInventory));
            }
          } else {
            this._spinnerService.hide();
            this._alertifyService.error(this.translate.instant('alert.alert_not_found_data'));
          }
        }
      );
      this._spinnerService.hide();
    }
  }

  // Get Machine Offline
  getDataMachineOffline() {
    this.lang = localStorage.getItem('lang');
    this._spinnerService.show();
    if (navigator.onLine === true) {
      this.listDataInventory.filter(item => {
        if (item.flag === 1) {
          this._inventoryService.getMachine(item.ownerFty + item.machineID, this.lang).subscribe(res => {
            if (res.isNull === false) {
              const data = this.listDataInventory.findIndex(x => x.machineID === item.machineID);
              this.listDataInventory.splice(data, 1);
              this.listDataInventory.unshift(res);
              const listDataInventoryJson = JSON.stringify(this.listDataInventory);
              localStorage.setItem('listDataInventory', listDataInventoryJson);
              this._alertifyService.success(this.translate.instant('inventory.alert_get_machine'));
              this.loadingData = false;
              this.checkGetMachineData();
              this._spinnerService.hide();
            } else {
              this._spinnerService.hide();
              this._alertifyService.error(this.translate.instant('inventory.machine_found') + ' :' + item.ownerFty + item.machineID);
            }
          });
        }
      }, error => {
        this._spinnerService.hide();
        this._alertifyService.error(this.translate.instant('error.system_error'));
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

  SubmitInvenroty() {
    this._spinnerService.show();
    const listModel = this.listDataInventory.filter(item => {
      if (!('flag' in item)) {
        return item;
      }
    });

    this.inventoryParams.fromDateTime = this._datePipe.transform(this.fromDate, 'yyyy-MM-dd HH:mm:ss');
    this.inventoryParams.toDateTime = this._datePipe.transform(new Date(), 'yyyy-MM-dd HH:mm:ss');
    this.inventoryParams.idInventory = this.optonsInventory;
    this.inventoryParams.idPlno = this.lineInventory.plnoId;
    this.inventoryParams.listMachineInventory = listModel;
    this._inventoryService.submitInventory(this.inventoryParams).subscribe(res => {
      if (res.erorr !== 1) {
        this.loadingData = true;
        this.listMachineInventory = res;
        localStorage.removeItem('listDataInventory');
        this._spinnerService.hide();
      }
    }, error => {
      this._spinnerService.hide();
      this._alertifyService.error(this.translate.instant('error.system_error'));
    });
  }

  checkMachineExits(itemMachine: any): boolean {
    if (itemMachine === '') {
      this._alertifyService.error(this.translate.instant('inventory.Please_enter_the_machine_code'));
      return false;
    }
    const item = this.listDataInventory
      .find(x => x.machineID.trim() === itemMachine.machineID.trim() && x.ownerFty.trim() === itemMachine.ownerFty.trim());
    if (item) {
      this._alertifyService.error(this.translate.instant('alert.alert_data_already_exists_scan_list'));
      this.machineCode = '';
      return false;
    } else {
      return true;
    }
  }

  clearData() {
    this._spinnerService.show();
    localStorage.removeItem('listDataInventory');
    this._alertifyService.success(this.translate.instant('inventory.delete_all_data'));
    setTimeout(function () {
      window.location.reload();
    }, 1000);
    this._spinnerService.hide();
  }

  refresh() {
    this._spinnerService.show();
    window.location.reload();
    this._spinnerService.hide();
  }

  deleteMachine(machineID: string) {
    this._sweetAlertService.confirm1(this.translate.instant('alert.do_you_want_delete'), this.translate.instant('alert.data_permanently_deleted'), () => {
      this.listDataInventory.splice(machineID, 1);
      localStorage.setItem('listDataInventory', JSON.stringify(this.listDataInventory));
      this.checkGetMachineData();
    });
  }

  checkGetMachineData() {
    this.getMachine = false;
    this.listDataInventory.filter(item => {
      if (('flag' in item)) {
        this.getMachine = true;
      }
    });
  }

  check(value: boolean) {
    this.checkLogin = value;
    if (this.checkLogin) {
      this.deleteMachine(this.index);
    }
  }

  loginDelete(index) {
    this.index = index;
    if (this.optonsInventory !== 1) {
      this.inventoryHomeLogin.showChildLogin();
    } else {
      this.deleteMachine(index);
    }
  }

  openScan() {
    this.modelScan.showChildModal();
    this.codeQr(this.qrResultString);
  }

  codeQr(event: string) {
    this.qrResultString = event;
    if (this.qrResultString !== undefined && this.qrResultString !== null) {
      this.machineCode = this.qrResultString.toUpperCase();
      this.getDataMachine();
      this.qrResultString = null;
    }
    // let check = true;
    // this.modelScan.qrScannerComponent.capturedQr.pipe(debounceTime(1000)).subscribe((res: string) => {
    //   if (res !== '' && check === true) {
    //     this.machineCode = res;
    //     this.getDataMachine();
    //     this.modelScan.childModal.hide();
    //     check = false;
    //   }
    // });
  }
}


