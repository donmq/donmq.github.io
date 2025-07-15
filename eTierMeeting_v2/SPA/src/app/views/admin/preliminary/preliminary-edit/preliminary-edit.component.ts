import {
  Component,
  EventEmitter,
  OnInit,
  Output,
  ViewChild,
} from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Select2OptionData } from 'ng-select2';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { PreliminaryPlnoAdd } from '../../../../_core/_models/preliminary-add';
import { PreliminaryList } from '../../../../_core/_models/preliminary-list';
import { User } from '../../../../_core/_models/user';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { BuildingService } from '../../../../_core/_services/building.service';
import { CellService } from '../../../../_core/_services/cell.service';
import { CellPlnoService } from '../../../../_core/_services/cellplno.service';
import { PreliminaryListService } from '../../../../_core/_services/preliminary-list.service';
import { SweetAlertService } from '../../../../_core/_services/sweet-alert.service';
import { UserService } from '../../../../_core/_services/user.service';

@Component({
  selector: 'app-preliminary-edit',
  templateUrl: './preliminary-edit.component.html',
  styleUrls: ['./preliminary-edit.component.scss'],
})
export class PreliminaryEditComponent implements OnInit {
  @Output() keyword = new EventEmitter<string>();
  @ViewChild('childModal', { static: false }) childModal: ModalDirective;
  cellVisible: boolean = false;
  preliminary: PreliminaryList = {
    empName: '',
    empNumber: '',
    updateBy: '',
    updateTime: null,
    visible: true,
    listBuilding: [],
    listCell: [],
    listHpA15: [],
    roleList: [],
    //Add
    is_Manager: true,
    is_Preliminary: true,

  };
  listCells: Array<Select2OptionData>;
  listBuildings: Array<Select2OptionData>;
  listPlnos: Array<Select2OptionData>;
  listUser: Array<Select2OptionData>;
  listCellsAdd: string[] = [];
  listBuildingsAdd: string[] = [];
  listPlnosAdd: string[] = [];
  roleList: any = [];
  managerList: any = [];
  user: User = {} as User;
  userName: string = '';
  buildingID: number;
  lang: string;

  constructor(
    private _spinner: NgxSpinnerService,
    private _cellService: CellService,
    private _cellplnoService: CellPlnoService,
    private _buildingService: BuildingService,
    private _userService: UserService,
    private _translate: TranslateService,
    private _preliminaryList: PreliminaryListService,
    private _sweetAlert: SweetAlertService,
    private _preliminaryListService: PreliminaryListService,
    private _alertifyService: AlertifyService
  ) { }

  ngOnInit() {
    this.getAllBuilding();
    this.getAllCell();
    this.getAllPlno();
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

  getAllCell() {
    this._cellService.getAllCellAdmin().subscribe((res) => {
      this.listCells = res.map((item) => {
        return {
          id: item.cellID.toString(),
          text: item.cellCode + '-' + item.cellName + '-' + item.buildingCode,
        };
      });
    });
  }

  getAllPlno() {
    this._cellplnoService.getAllCellPlno().subscribe((res) => {
      this.listPlnos = res.map((item) => {
        return { id: item.plno, text: item.plno + '-' + item.place };
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
        managerList: this.managerList,
        is_Manager: this.preliminary.is_Manager,
        is_Preliminary: this.preliminary.is_Preliminary,
        plno: x.split('_')[0],
      };
      this.roleList.push(role);
    });
    listRolesOrder.map((x) => {
      const role = {
        buildingID: x.split('_')[1],
        cell: x.split('_')[2],
        plno: x.split('_')[0],
        is_Manager: this.preliminary.is_Manager,
        is_Preliminary: this.preliminary.is_Preliminary,
      };
      this.roleList.push(role);
    });
  }

  updatePreliminaryPlnos() {
    console.log("Edit List Pre ", this.preliminary)
    this.lang = localStorage.getItem('lang');
    const currenLoginUser = JSON.parse(localStorage.getItem('user'));
    this.handleRoleSelect();
    const preliminaryAdd: PreliminaryPlnoAdd = {
      empName: this.preliminary.empName,
      empNumber: this.preliminary.empNumber,
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
      .updatePreliminary(preliminaryAdd, this.lang)
      .subscribe(
        (res) => {
          if (res) {
            this._sweetAlert.success('Success', 'Thành công');
            // call về home
            this.keyword.emit(preliminaryAdd.empNumber);
            this.hideChildModal();
            this.cellVisible = false;
            this._spinner.hide();
          } else {
            this._sweetAlert.error('Error', '');
            this._spinner.hide();
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
  }

  showChildModal(item: PreliminaryList) {
    this.preliminary = item;
    const listBuilding = [];
    item.listBuilding.map((i) => {
      listBuilding.push(i.buildingID.toString());
    });

    const listCell = [];
    item.listCell.map((i) => {
      listCell.push(i.cellCode.toString());
    });

    const listPlnos = [];
    item.listHpA15.map((i) => {
      listPlnos.push(i.plnoCode);
    });
    this.listBuildingsAdd = listBuilding;

    this.listCellsAdd = listCell;
    this.listPlnosAdd = listPlnos;
    if (listBuilding.find((i) => i === '100')) { this.getCellsList(); }

    this.getPlnosAllList();
    this.childModal.show();
  }

  hideChildModal() {
    this.childModal.hide();
  }
}
