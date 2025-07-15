import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { ListHpa15, PreliminaryList } from '../../../../_core/_models/preliminary-list';
import { UserRoles } from '../../../../_core/_models/userRoles';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { InventoryService } from '../../../../_core/_services/inventory.service';
import { PreliminaryListService } from '../../../../_core/_services/preliminary-list.service';
import { UserRolesService } from '../../../../_core/_services/user-roles.service';

@Component({
  selector: 'app-inventory-building',
  templateUrl: './inventory-building.component.html',
  styleUrls: ['./inventory-building.component.scss']
})
export class InventoryBuildingComponent implements OnInit, OnDestroy {

  listBuilding: any[] = [];
  listPlno: ListHpa15[];
  listBuildingOrther: any = [];
  optonsBuilding: number;
  preliminaryList: PreliminaryList;
  listUserRoles: UserRoles[];
  listRoles: number[] = [];

  constructor(
    private _inventoryService: InventoryService,
    private _router: Router,
    private _alertifyService: AlertifyService,
    private translate: TranslateService,
    private _spinnerService: NgxSpinnerService,
    private preliminaryListService: PreliminaryListService,
    private userRolesService: UserRolesService
  ) { }

  ngOnDestroy(): void {
    this.listBuilding = [];
    this.listBuildingOrther = [];
    localStorage.removeItem('CheckInventory');
  }

  ngOnInit(): void {
    this.getUserRoles();
    this.getPreliminaryPlnos();
    this.optonsBuilding = +localStorage.getItem('CheckInventory');
  }

  getUserRoles() {
    const empNumber = JSON.parse(localStorage.getItem('user'));
    this.userRolesService.getRoleByUser(empNumber.userName).subscribe(res => {
      this.listUserRoles = res;
    });
  }

  checkOption(value: number) {
    this.optonsBuilding = value;
  }

  getPreliminaryPlnos() {
    const user = JSON.parse(localStorage.getItem('user'));
    user.listRoles.map(x => {
      this.listRoles.push(x.roles);
    });
    if (this.listRoles.includes(3)) {
      this.preliminaryListService.getPreliminaryPlnos(user.userName).subscribe(res => {
        this.preliminaryList = res;
        this.listBuilding = this.preliminaryList.listBuilding.filter(x => x.buildingID !== 100);
        this.listBuildingOrther = this.preliminaryList.listCell;
        this.listPlno = this.preliminaryList.listHpA15;
      });
    } else if (this.listRoles.includes(4) || this.listRoles.includes(5) || this.listRoles.includes(2)) {
      this._inventoryService.listPlno.next(null);
      this.getBuilding();
      this.getBuildingOrther();
    }
  }

  getBuilding(): void {
    this._spinnerService.show();
    this._inventoryService.getBuilding().subscribe(res => {
      this.listBuilding = res;
      this._spinnerService.hide();
    });
  }

  getBuildingOrther(): void {
    this._spinnerService.show();
    this._inventoryService.getBuildingOther(100).subscribe(res => {
      this.listBuildingOrther = res;
      this._spinnerService.hide();
    });
  }

  reditToLine(item): void {
    if (this.optonsBuilding == null || this.optonsBuilding === 0) {
      return this._alertifyService.error(this.translate.instant('alert.alert_select_inventory_type'));
    }

    if (item.buildingCode != null) {
      if (this.listPlno !== undefined && this.listPlno !== []) {
        const list = this.listPlno.filter(x => x.buildingID === item.buildingID);
        this._inventoryService.listPlno.next(list);
      }

      this._inventoryService.getCodeName(item.buildingCode);
      this._router.navigate(['/inventoryv2/line', item.buildingID, 'building', this.optonsBuilding]);
    } else {
      if (item.cellCode != null) {
        if (this.listPlno !== undefined && this.listPlno !== []) {
          this._inventoryService.listPlno.next(this.listPlno.filter(x => x.buildingID === 100));
        }
      }
      this._inventoryService.getCodeName(item.cellCode);
      this._router.navigate(['/inventoryv2/line', item.cellCode, 'other', this.optonsBuilding]);
    }
  }
}
