import { ChangeDetectorRef, Component, EventEmitter, OnInit, Output } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { PreliminaryPlnoAdd } from '../../../../_core/_models/preliminary-add';
import { User } from '../../../../_core/_models/user';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { BuildingService } from '../../../../_core/_services/building.service';
import { CellService } from '../../../../_core/_services/cell.service';
import { CellPlnoService } from '../../../../_core/_services/cellplno.service';
import { PreliminaryListService } from '../../../../_core/_services/preliminary-list.service';
import { SweetAlertService } from '../../../../_core/_services/sweet-alert.service';
import { UserService } from '../../../../_core/_services/user.service';

@Component({
  selector: 'app-preliminary-add',
  templateUrl: './preliminary-add.component.html',
  styleUrls: ['./preliminary-add.component.scss'],
})
export class PreliminaryAddComponent implements OnInit {
  @Output() keyword = new EventEmitter<string>();
  listCells: Array<Select2OptionData>;
  listBuildings: Array<Select2OptionData>;
  listPlnos: Array<Select2OptionData>;
  listUser: Array<Select2OptionData>;
  cellVisible: boolean = false;
  listCellsAdd: string[] = [];
  listBuildingsAdd: string[] = [];
  listPlnosAdd: string[] = [];
  user: User = {} as User;
  userName: string = '';
  buildingID: number;
  cellID: number;
  lang: string;
  roleList: any = [];
  ///Add 2 check 
  is_Manager = false;
  is_Preliminary = false;
  ///
  constructor(
    private _spinner: NgxSpinnerService,
    private _cellService: CellService,
    private _cellplnoService: CellPlnoService,
    private _buildingService: BuildingService,
    private _userService: UserService,
    private _preliminaryListService: PreliminaryListService,
    private _translate: TranslateService,
    private _alertifyService: AlertifyService,
    private _sweetAlert: SweetAlertService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit() {
    this.getAllBuilding();
    this.getAllUser();

  }

  ngAfterContentChecked() {
    this.cdr.detectChanges();
    // call or add here your code
  }

  getAllUser() {
    this._userService.getAllUserPreliminary().subscribe((res) => {
      this.listUser = res.map((item) => {
        return { id: item.userName, text: item.userName };
      });
    });
  }
  setUserName(event) {
    console.log(event);
  }
  getUserName(userName: string) {
    this._userService.getUserName(userName).subscribe((res) => {
      if (res != null) {
        this.user = res;
      } else {
        this.user = {} as User;
      }
    });
  }
  clearAllBuiding(event) {
    this.listBuildingsAdd = [];
    this.listCellsAdd = [];
    this.listPlnosAdd = [];

  }

  ClearAllCell() {
    this.listBuildingsAdd = [];
    this.listCellsAdd = [];
    this.listPlnosAdd = [];
  }
  ClearAllPlno() {
    this.listBuildingsAdd = [];
    this.listCellsAdd = [];
    this.listPlnosAdd = [];
  }
  getAllBuilding() {
    this._buildingService.getAllBuilding().subscribe((res) => {
      this.listBuildings = res.map((item) => {
        return {
          id: item.buildingID.toString(),
          text: item.buildingName + ' (' + item.buildingCode + ')',
        };
      });
    });
  }
  unSelectBuildingList(event) {
    const listFilter = this.listPlnosAdd;
    if (event.value === '100') {
      this.listCellsAdd = [];
      this.listCells = [];
      this.listPlnos = [];
      const plnoAdd = this.listPlnosAdd.filter((f) => f.split('_').length < 3);
      this.listPlnosAdd = plnoAdd;
    } else if (this.listBuildingsAdd.length > 0) {
      const buildingAdd = listFilter.filter((f) => f.split('_').length < 3);
      const cellAdd = listFilter.filter((f) => f.split('_').length > 2);
      this.listPlnosAdd = cellAdd.concat(
        buildingAdd.filter((f) => f.split('_')[1].toString() !== event.value)
      );
    } else {
      this.listPlnosAdd = [];
    }

    this.getPlnosAllList();
  }

  getListPlnoByBuildingID(event) {
    if (event === '100') {
      this.getCellsList();
    } else {
      this.listBuildingsAdd.forEach((item) => {
        this.buildingID = +item;
      });
    }
    this.getPlnosAllList();
  }

  unSelectCellsList(event) {
    let plnoAdd = this.listPlnosAdd.filter((f) => f.split('_').length < 3);
    const listFilter = this.listPlnosAdd.filter((f) => f.split('_').length > 2);
    const listPlnoExits = listFilter.filter(
      (f) => f.split('_')[3].toString() !== event.value
    );
    if (plnoAdd.length > 0) {
      plnoAdd = plnoAdd.concat(listPlnoExits);
    } else {
      plnoAdd = listPlnoExits;
    }
    this.listPlnosAdd = plnoAdd;
    this.getPlnosAllList();
  }

  getPlnosBuildingOrderList() {
    this._cellplnoService
      .getListPlnoByMultipleBuildingID(this.listBuildingsAdd)
      .subscribe((res) => {
        this.listPlnos = res.map((item) => {
          return { id: item.plno, text: item.place };
        });
      });
  }

  getPlnosAllList() {
    const listAll = {
      id: 'string',
      listCell: this.listCellsAdd,
      listBuilding: this.listBuildingsAdd,
    };
    this._cellplnoService.getListPlnoByMultipleID(listAll).subscribe((res) => {
      this.listPlnos = res
        .sort((a, b) => {
          const nameA = a.buildingCode.toUpperCase();
          const nameB = b.buildingCode.toUpperCase();
          if (nameA < nameB) {
            return -1;
          }
          if (nameA > nameB) {
            return 1;
          }
          return 0;
        })
        .map((item) => {
          return { id: item.plno, text: item.place };
        });
    });
    this.cellVisible =
      this.listBuildingsAdd.filter((f) => f === '100').length > 0
        ? true
        : false;
  }

  getCellsList() {
    this._cellService.getListCellExistPlnoByBuildingID(100).subscribe((res) => {
      this.listCells = res.map((item) => {
        return {
          id: item.cellCode.toString(),
          text: item.cellName + ' (' + item.cellCode + ')',
        };
      });
    });
    this.cellVisible =
      this.listBuildingsAdd.filter((f) => f === '100').length > 0
        ? true
        : false;
  }

  getPlnosListWithCells() {
    this._cellplnoService
      .getListPlnoByMultipleCellID(this.listCellsAdd)
      .subscribe((res) => {
        this.listPlnos = res.map((item) => {
          return { id: item.plno, text: item.place };
        });
      });
  }

  getPlnosList() {
    this._cellplnoService
      .getListPlnoByMultipleBuildingID(this.listBuildingsAdd)
      .subscribe((res) => {
        this.listPlnos = res.map((item) => {
          return { id: item.plno, text: item.place };
        });
      });
  }

  handleRoleSelect() {
    this.roleList = [];
    const listRoles = this.listPlnosAdd.filter((f) => f.split('_').length < 3);
    const listRolesOrder = this.listPlnosAdd.filter(
      (f) => f.split('_').length > 2
    );

    listRoles.map((x) => {
      const role = {
        buildingID: x.split('_')[1],
        cell: '',
        plno: x.split('_')[0],
        is_Manager: this.is_Manager,
        is_Preliminary: this.is_Preliminary
      };
      this.roleList.push(role);
    });

    listRolesOrder.map((x) => {
      const role = {
        buildingID: x.split('_')[1],
        cell: x.split('_')[2],
        plno: x.split('_')[0],
        is_Manager: this.is_Manager,
        is_Preliminary: this.is_Preliminary
      };
      this.roleList.push(role);
    });
  }

  addPreliminary() {
    this.lang = localStorage.getItem('lang');
    const currenLoginUser = JSON.parse(localStorage.getItem('user'));
    this.handleRoleSelect();
    const preliminaryAdd: PreliminaryPlnoAdd = {
      empName: this.user.empName,
      empNumber: this.userName,
      updateBy: currenLoginUser.userName,
      roleList: this.roleList,

    };


    if (preliminaryAdd.empNumber === '') {
      return this._sweetAlert.error(
        this._translate.instant('alert.admin.please_enter_user_code')
      );
    }
    if (preliminaryAdd.roleList.length === 0) {
      return this._sweetAlert.error(
        this._translate.instant('alert.admin.please_select_a_location')
      );
    }
    this._spinner.show();
    this._preliminaryListService
      .addPreliminary(preliminaryAdd, this.lang)
      .subscribe(
        (res) => {
          if (res) {
            this._sweetAlert.success('Success', 'Thành công');
            // call về home
            this.keyword.emit(preliminaryAdd.empNumber);
            this.clearFormAdd();
            this.cellVisible = false;
            this._spinner.hide();
          } else {
            this._spinner.hide();
            this._sweetAlert.error('Error', '');
          }
        },
        (error) => {
          console.log(error);
          this._spinner.hide();
          this._alertifyService.error(
            this._translate.instant('error.system_error')
          );
        }
      );
    console.log("AddPreliminary", preliminaryAdd)
  }

  clearFormAdd() {
    this.listCells = [];
    this.listPlnos = [];
    this.listBuildingsAdd = [];
    this.listCellsAdd = [];
    this.listPlnosAdd = [];
    this.is_Manager = false;
    this.is_Preliminary = false;
    this.getAllUser();
  }
}
