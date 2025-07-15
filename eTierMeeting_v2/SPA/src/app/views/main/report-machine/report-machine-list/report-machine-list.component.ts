import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { SearchMachineParams } from '../../../../_core/_dtos/search-machine-params';
import { Pagination } from '../../../../_core/_models/pagination';
import { ReportMachine, ReportMachineItem } from '../../../../_core/_models/report-machine';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { BuildingService } from '../../../../_core/_services/building.service';
import { CategoryService } from '../../../../_core/_services/category.service';
import { CellService } from '../../../../_core/_services/cell.service';
import { CellPlnoService } from '../../../../_core/_services/cellplno.service';
import { MachineReportService } from '../../../../_core/_services/machine-report.service';
import { PdcService } from '../../../../_core/_services/pdc.service';

@Component({
  selector: 'app-report-machine-list',
  templateUrl: './report-machine-list.component.html',
  styleUrls: ['./report-machine-list.component.scss']
})
export class ReportMachineListComponent implements OnInit {

  listMachines: ReportMachineItem[] = [];
  listCategorys: Array<Select2OptionData>;
  listPDCs: Array<Select2OptionData>;
  listCells: Array<Select2OptionData>;
  listBuildings: Array<Select2OptionData>;
  listPlnos: Array<Select2OptionData>;

  cateID: string = 'all';
  cellPlnoID: string = 'all';
  cellID: string = 'all';
  pdcID: string = 'all';
  buildingID: string = 'all';

  pdcIDTem: string = '';
  buildingIDTem: string = '';

  categorys: string = 'all';
  machineIds: string = 'all';

  dataChart: ReportMachine = {
    totalIdle: 0,
    totalInuse: 0,
    listReportMachineItem: [],
  };
  machineUsed: number;
  machineNotUsed: number;

  searchMachineParams: SearchMachineParams = <SearchMachineParams>{
    buildingCode: '0',
    category: '',
    cellCode: '0',
    machineId: '',
    pdcId: 0,
    isPaging: true,
    positionCode: '',
    sort: ''
  };

  pagination: Pagination = {
    currentPage: 1,
    pageSize: 10,
    totalCount: 0,
    totalPage: 0,
  };

  constructor(private _machineReportService: MachineReportService,
    private _categoryService: CategoryService,
    private _pdcService: PdcService,
    private _cellService: CellService,
    private _cellplnoService: CellPlnoService,
    private _buildingService: BuildingService,
    private _spinnerService: NgxSpinnerService,
    private translate: TranslateService,
    private _alertifyService: AlertifyService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit() {
    this.loadData();
    this.getAllCategory();
    this.getAllPdc();
    this.getAllCell();
    this.getAllBuilding();
    this.getAllPlno();
  }

  searchMachine() {
    this.loadData();
    this.getDataChartPie();
  }

  loadData() {
    this._spinnerService.show();
    this.searchMachineParams.category = this.cateID === 'all' ? '' : this.cateID;
    this.searchMachineParams.cellCode = this.cellID === 'all' ? '0' : this.cellID;
    this.searchMachineParams.positionCode = this.cellPlnoID === 'all' ? '' : this.cellPlnoID;
    this.searchMachineParams.pdcId = this.pdcID === 'all' ? 0 : +this.pdcID;
    this.searchMachineParams.buildingCode = this.buildingID === 'all' ? '0' : this.buildingID;
    this._machineReportService.getMachineRport(this.searchMachineParams)
      .subscribe(res => {
        this.listMachines = res.listReportMachineItem;
        this._spinnerService.hide();
      }, error => {
        this._spinnerService.hide();
        this._alertifyService.error(this.translate.instant('error.system_error'));
      });
  }

  getAllCategory() {
    this._categoryService.getCategory().subscribe(res => {
      this.listCategorys = res.map(item => {
        return { id: item.askid, text: item.askid + '-' + item.kinen_CN };
      });
      this.listCategorys.unshift({ id: 'all', text: this.translate.instant('homepage.pdc_category') });
    });
  }

  getAllPdc() {
    this.pdcID = 'all';
    this._pdcService.getAllPdc().subscribe(res => {
      this.listPDCs = res.map(item => {
        return { id: item.pdcid.toString(), text: item.pdcName };
      });
      this.listPDCs.unshift({ id: 'all', text: this.translate.instant('homepage.pdc_name') });
    });
  }

  getAllCell() {
    this.cellID = 'all';
    this._cellService.getAllCell().subscribe(res => {
      this.listCells = res.map(item => {
        return { id: item.cellCode, text: item.cellName };
      });
      this.listCells.unshift({ id: 'all', text: this.translate.instant('homepage.cell_name') });
    });
  }

  getAllBuilding() {
    this.buildingID = 'all';
    this._buildingService.getAllBuilding().subscribe(res => {
      this.listBuildings = res.map(item => {
        return { id: item.buildingID.toString(), text: item.buildingName };
      });
      this.listBuildings.unshift({ id: 'all', text: this.translate.instant('homepage.building_name') });
    });
  }

  getAllPlno() {
    this.cellPlnoID = 'all';
    this._cellplnoService.getAllCellPlno().subscribe(res => {
      this.listPlnos = res.map(item => {
        return { id: item.plno, text: item.place };
      });
      this.listPlnos.unshift({ id: 'all', text: this.translate.instant('homepage.plno_name') });
    });
  }

  // get data search
  getListCellByPdcID() {
    if (this.pdcID !== 'all') {
      this._cellService.getListCellByPdcID(+this.pdcID).subscribe(res => {
        this.listCells = res.map(item => {
          return { id: item.cellCode, text: item.cellName };
        });
        this.listCells.unshift({ id: 'all', text: this.translate.instant('homepage.cell_name') });
      });
      this.getAllBuildingByPdcID();
      this.getListPlnoByPdcID();
      this.pdcIDTem = this.pdcID;
    } else {
      this.getAllCell();
      this.getAllBuilding();
      this.getAllPlno();
    }
    this.searchMachine();
  }

  getAllBuildingByPdcID() {
    this.cellID = 'all';
    if (this.pdcID !== 'all') {
      this._buildingService.getBuildingByPdcID(+this.pdcID).subscribe(res => {
        this.listBuildings = res.map(item => {
          return { id: item.buildingID.toString(), text: item.buildingName };
        });
        this.listBuildings.unshift({ id: 'all', text: this.translate.instant('homepage.building_name') });
      });
    }
  }

  getListBuildingByCell() {
    if (this.cellID !== 'all') {
      this._buildingService.getBuildingByCellCodeAndPDC(this.cellID)
        .subscribe(res => {
          this.listBuildings = res.map(item => {
            return { id: item.buildingID.toString(), text: item.buildingName };
          });
          this.listBuildings.unshift({ id: 'all', text: this.translate.instant('homepage.building_name') });
        });
      this.getListPlnoByCellID();
      this.getAllPdc();

    }
    this.searchMachine();
  }

  getListPlnoByPdcID() {
    if (this.pdcID !== 'all') {
      this._cellplnoService.getListPlnoByPDCID(+this.pdcID).subscribe(res => {
        this.listPlnos = res.map(item => {
          return { id: item.plno, text: item.place };
        });
        this.listPlnos.unshift({ id: 'all', text: this.translate.instant('homepage.plno_name') });
      });
    }
  }

  getListPlnoByCellID() {
    if (this.cellID !== 'all') {
      this._cellplnoService.getListPlnoByCellID(this.cellID).subscribe(res => {
        this.listPlnos = res.map(item => {
          return { id: item.plno, text: item.place };
        });
        this.listPlnos.unshift({ id: 'all', text: this.translate.instant('homepage.plno_name') });
      });
    } else {
      this.getAllBuilding();
      this.getAllCell();
      this.getAllPlno();
    }
    this.searchMachine();
  }

  getListPlnoByBuildingID() {
    if (this.buildingID !== 'all') {
      this._cellplnoService.getListPlnoByBuildingID(+this.buildingID).subscribe(res => {
        this.listPlnos = res.map(item => {
          return { id: item.plno, text: item.place };
        });
        this.listPlnos.unshift({ id: 'all', text: this.translate.instant('homepage.plno_name') });
        this.pdcIDTem = 'all';
      });
      this.getAllCell();
      this.getAllPdc();
    } else if (this.pdcIDTem === 'all' && this.cellID === 'all') {
      this.getAllBuilding();
      if (this.cellPlnoID === 'all') {
        this.getAllPlno();
      }
    }
    this.searchMachine();
  }

  getPlano(event: any) {
    if (event !== 'all') {
      this.getAllBuilding();
      this.getAllPdc();
      this.getAllCell();
    }
    this.searchMachine();
  }
  // get data search End

  getDataChartPie() {
    this._machineReportService.getMachineRport(this.searchMachineParams).subscribe(res => {
      this.dataChart = res;
    });
  }

  ngAfterContentChecked() {
    this.cdr.detectChanges();
    // call or add here your code
  }
}
