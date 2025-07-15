import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { SearchMachineParams } from '../../../_core/_dtos/search-machine-params';
import { MachineExcel } from '../../../_core/_models/machine-excel';
import { Pagination } from '../../../_core/_models/pagination';
import { SearchMachine } from '../../../_core/_models/search-machine';
import { AlertifyService } from '../../../_core/_services/alertify.service';
import { BuildingService } from '../../../_core/_services/building.service';
import { CategoryService } from '../../../_core/_services/category.service';
import { CellService } from '../../../_core/_services/cell.service';
import { CellPlnoService } from '../../../_core/_services/cellplno.service';
import { PdcService } from '../../../_core/_services/pdc.service';
import { SearchMachineService } from '../../../_core/_services/searchmachine.service';
import { KeyValuePair } from '../../../_core/_utility/key-value-pair';

@Component({
  templateUrl: 'dashboard.component.html',
  styleUrls: ['./dashboard.component.scss']
})
export class DashboardComponent implements OnInit {
  searchMachines: SearchMachine[] = [];
  machineExcels: MachineExcel[] = [];
  categorys: KeyValuePair[] = [];
  pdcs: KeyValuePair[] = [];
  buildings: KeyValuePair[] = [];
  cells: KeyValuePair[] = [];
  cellPlnos: KeyValuePair[] = [];

  cateID: string = 'all';
  cellPlnoID: string = 'all';
  cellID: string = 'all';
  pdcID: string = 'all';
  buildingID: string = 'all';

  pdcIDTem: string = '';
  buildingIDTem: string = '';

  checkLang: string;

  searchMachineParams: SearchMachineParams = {
    buildingCode: '0',
    buildingId: 0,
    category: 'all',
    cellCode: '0',
    isPaging: true,
    machineId: '',
    pdcId: 0,
    positionCode: '',
    sort: ''
  };
  pagination: Pagination = {
    currentPage: 1,
    pageSize: 10,
    totalCount: 0,
    totalPage: 0,
  };

  constructor(
    private spinnerService: NgxSpinnerService,
    private searchMachineService: SearchMachineService,
    private _alertifyService: AlertifyService,
    private categoryService: CategoryService,
    private pdcService: PdcService,
    private buildingService: BuildingService,
    private cellService: CellService,
    private cellPlnoService: CellPlnoService,
    private translate: TranslateService,
    private cdr: ChangeDetectorRef
  ) { }


  ngOnInit(): void {
    this.checkLang = localStorage.getItem('lang');
    this.getAllCategory();
    this.getAllPdc();
    this.getAllCell();
    this.getAllBuilding();
    this.getAllCellPlno();

    this.loadData();

    let a = document.getElementsByClassName('ajs-footer');
    if (a) {
      a[0]?.childNodes[1]?.childNodes[1]?.remove();
    }
  }


  loadData() {
    this.spinnerService.show();
    this.searchMachineParams.category = this.cateID === 'all' ? '' : this.cateID;
    this.searchMachineParams.cellCode = this.cellID === 'all' ? '0' : this.cellID;
    this.searchMachineParams.positionCode = this.cellPlnoID === 'all' ? '' : this.cellPlnoID;
    this.searchMachineParams.pdcId = this.pdcID === 'all' ? 0 : +this.pdcID;
    this.searchMachineParams.buildingId = this.buildingID === 'all' ? 0 : +this.buildingID;
    this.searchMachineService.searchMachine(this.pagination, this.searchMachineParams)
      .subscribe(res => {
        this.searchMachines = res.result;
        this.pagination = res.pagination;
        this.spinnerService.hide();
      }, error => {
        this.spinnerService.hide();
        this._alertifyService.error(this.translate.instant('error.system_error'));
      });
  }

  onChangeCate() {
    this.searchMachine();
  }

  getAllCategory() {
    this.categoryService.getCategory().subscribe(res => {
      this.categorys = res.map(item => {
        return { key: item.askid, value: item.askid + '-' + item.kinen_CN };
      });
      this.categorys.unshift({ key: 'all', value: this.translate.instant('homepage.all') });
    });
  }

  getAllPdc() {
    this.pdcID = 'all';
    this.pdcService.getAllPdc().subscribe(res => {
      this.pdcs = res.map(item => {
        return { key: item.pdcid.toString(), value: item.pdcName };
      });
      this.pdcs.unshift({ key: 'all', value: this.translate.instant('homepage.all') });
    });
  }

  getAllBuilding() {
    this.buildingID = 'all';
    this.buildingService.getAllBuilding().subscribe(res => {
      this.buildings = res.map(item => {
        return { key: item.buildingID.toString(), value: item.buildingName };
      });
      this.buildings.unshift({ key: 'all', value: this.translate.instant('homepage.all') });
    });
  }

  getAllCell() {
    this.cellID = 'all';
    this.cellService.getAllCell().subscribe(res => {
      this.cells = res.map(item => {
        return { key: item.cellCode, value: item.cellCode + '-' + item.cellName };
      });
      this.cells.unshift({ key: 'all', value: this.translate.instant('homepage.all') });
    });
  }

  getAllCellPlno() {
    this.cellPlnoID = 'all';
    this.cellPlnoService.getAllCellPlno().subscribe(res => {
      this.cellPlnos = res.map(item => {
        return { key: item.plno, value: item.place };
      });
      this.cellPlnos.unshift({ key: 'all', value: this.translate.instant('homepage.all') });
    });
  }


  getListCellByPdcID() {
    if (this.pdcID !== 'all') {
      this.cellService.getListCellByPdcID(+this.pdcID).subscribe(res => {
        this.cells = res.map(item => {
          return { key: item.cellCode, value: item.cellName };
        });
        this.cells.unshift({ key: 'all', value: this.translate.instant('homepage.all') });
      });
      this.getAllBuildingByPdcID();
      this.getListPlnoByPdcID();
      this.pdcIDTem = this.pdcID;
    } else {
      this.getAllCell();
      this.getAllBuilding();
      this.getAllCellPlno();
    }
    this.searchMachine();
  }

  getAllBuildingByPdcID() {
    this.cellID = 'all';
    if (this.pdcID !== 'all') {
      this.buildingService.getBuildingByPdcID(+this.pdcID).subscribe(res => {
        this.buildings = res.map(item => {
          return { key: item.buildingID.toString(), value: item.buildingName };
        });
        this.buildings.unshift({ key: 'all', value: this.translate.instant('homepage.all') });
      });
    }
  }

  // Get listbuilding
  getListBuildingByCell() {
    if (this.cellID !== 'all') {
      this.buildingService.getBuildingByCellCodeAndPDC(this.cellID)
        .subscribe(res => {
          this.buildings = res.map(item => {
            return { key: item.buildingID.toString(), value: item.buildingName };
          });
          this.buildings.unshift({ key: 'all', value: this.translate.instant('homepage.all') });
        });
      this.getListPlnoByCellID();
      this.getAllPdc();

    }
    this.searchMachine();
  }

  getListPlnoByPdcID() {
    if (this.pdcID !== 'all') {
      this.cellPlnoService.getListPlnoByPDCID(+this.pdcID).subscribe(res => {
        this.cellPlnos = res.map(item => {
          return { key: item.plno, value: item.place };
        });
        this.cellPlnos.unshift({ key: 'all', value: this.translate.instant('homepage.all') });
      });
    }
  }

  getListPlnoByCellID() {
    if (this.cellID !== 'all') {
      this.cellPlnoService.getListPlnoByCellID(this.cellID).subscribe(res => {
        this.cellPlnos = res.map(item => {
          return { key: item.plno, value: item.place };
        });
        this.cellPlnos.unshift({ key: 'all', value: this.translate.instant('homepage.all') });
      });
    } else {
      this.getAllBuilding();
      this.getAllCell();
      this.getAllCellPlno();
    }
    this.searchMachine();
  }


  getListPlnoByBuildingID() {
    if (this.buildingID !== 'all') {
      this.cellPlnoService.getListPlnoByBuildingID(+this.buildingID).subscribe(res => {
        this.cellPlnos = res.map(item => {
          return { key: item.plno, value: item.place };
        });
        this.cellPlnos.unshift({ key: 'all', value: this.translate.instant('homepage.all') });
        this.pdcIDTem = 'all';
      });
      this.getAllCell();
    } else if (this.pdcIDTem === 'all' && this.cellID === 'all') {
      this.getAllBuilding();
      if (this.cellPlnoID === 'all') {
        this.getAllCellPlno();
      }
    }
    this.searchMachine();
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page === undefined ? 1 : event.page;
    this.loadData();
  }

  searchMachine() {
    this.pagination.currentPage = 1;
    this.loadData();
  }

  getPlano(event: any) {
    if (event !== 'all') {
      this.getAllBuilding();
    }

    this.searchMachine();
  }

  exportExcelData() {
    this.searchMachineService.exportExcelData(this.searchMachineParams);
  }

  ngAfterContentChecked() {
    this.cdr.detectChanges();
    // call or add here your code
  }

}
