import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Select2OptionData } from 'ng-select2';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { SearchHistoryParams } from '../../../_core/_dtos/search-history-params';
import { Pagination } from '../../../_core/_models/pagination';
import { SearchHistory } from '../../../_core/_models/search-history';
import { AlertifyService } from '../../../_core/_services/alertify.service';
import { BuildingService } from '../../../_core/_services/building.service';
import { CellService } from '../../../_core/_services/cell.service';
import { CellPlnoService } from '../../../_core/_services/cellplno.service';
import { PdcService } from '../../../_core/_services/pdc.service';
import { SearchHistoryService } from '../../../_core/_services/search-history.service';
import { FunctionUtility } from '../../../_core/_utility/function-utility';

@Component({
  selector: 'app-history-move',
  templateUrl: './history-move.component.html',
  styleUrls: ['./history-move.component.scss']
})
export class HistoryMoveComponent implements OnInit {
  searchHistorys: SearchHistory[] = [];

  pdcs: Array<Select2OptionData>;
  buildings: Array<Select2OptionData>;
  cells: Array<Select2OptionData>;
  cellPlnos: Array<Select2OptionData>;

  cateID: string = 'all';
  cellPlnoID: string = 'all';
  cellID: string = 'all';
  pdcID: string = 'all';
  buildingID: string = 'all';

  fromDate: Date = null;
  toDate: Date = null;

  pdcIDTem: string = '';
  buildingIDTem: string = '';

  searchHistoryParams: SearchHistoryParams = {
    machineId: '',
    pdcId: 0,
    buildingCode: 0,
    cellName: 0,
    positionCode: '',
    timeStart: '',
    timeEnd: '',
    sort: '',
    isPaging: true
  };

  pagination: Pagination = {
    currentPage: 1,
    pageSize: 10,
    totalCount: 0,
    totalPage: 0,
  };
  lang: string;
  constructor(
    private searchHistoryService: SearchHistoryService,
    private alertifyService: AlertifyService,
    private spinnerService: NgxSpinnerService,
    private pdcService: PdcService,
    private buildingService: BuildingService,
    private cellService: CellService,
    private cellPlnoService: CellPlnoService,
    private functionUtility: FunctionUtility,
    private translate: TranslateService,
    private localeService: BsLocaleService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit() {
    this.getAllBuilding();
    this.getAllCell();
    this.getAllPdc();
    this.getAllCellPlno();
    this.loadHistory();
    this.dateLanguage();
  }

  dateLanguage() {
    this.lang = localStorage.getItem('lang');
    this.localeService.use(this.lang.substring(0, 2));
  }

  loadHistory() {
    this.spinnerService.show();
    this.searchHistoryParams.timeStart = this.functionUtility.getDateFormat(this.fromDate);
    this.searchHistoryParams.timeEnd = this.functionUtility.getDateFormat(this.toDate);
    this.searchHistoryParams.cellName = this.cellID === 'all' ? 0 : +this.cellID;
    this.searchHistoryParams.positionCode = this.cellPlnoID === 'all' ? '' : this.cellPlnoID;
    this.searchHistoryParams.pdcId = this.pdcID === 'all' ? 0 : +this.pdcID;
    this.searchHistoryParams.buildingCode = this.buildingID === 'all' ? 0 : +this.buildingID;
    this.searchHistoryService.searchHistory(this.pagination, this.searchHistoryParams)
      .subscribe(res => {
        this.searchHistorys = res.result;
        this.pagination = res.pagination;
        this.spinnerService.hide();
      }, error => {
        this.spinnerService.hide();
        this.alertifyService.error(this.translate.instant('error.system_error'));
      });
  }

  getAllPdc() {
    this.pdcID = 'all';
    this.pdcService.getAllPdc().subscribe(res => {
      this.pdcs = res.map(item => {
        return { id: item.pdcid.toString(), text: item.pdcName };
      });
      this.pdcs.unshift({ id: 'all', text: this.translate.instant('history.all_set') });
    });
  }

  getAllBuilding() {
    this.buildingID = 'all';
    this.buildingService.getAllBuilding().subscribe(res => {
      this.buildings = res.map(item => {
        return { id: item.buildingID.toString(), text: item.buildingName };
      });
      this.buildings.unshift({ id: 'all', text: this.translate.instant('history.all_building') });
    });
  }

  getAllCell() {
    this.cellID = 'all';
    this.cellService.getAllCell().subscribe(res => {
      this.cells = res.map(item => {
        return { id: item.cellID.toString(), text: item.cellCode + '-' + item.cellName };
      });
      this.cells.unshift({ id: 'all', text: this.translate.instant('history.all_unit') });
    });
  }

  getAllCellPlno() {
    this.cellPlnoID = 'all';
    this.cellPlnoService.getAllCellPlno().subscribe(res => {
      this.cellPlnos = res.map(item => {
        return { id: item.plno, text: item.place };
      });
      this.cellPlnos.unshift({ id: 'all', text: this.translate.instant('history.all_position') });
    });
  }

  getListBuildingByPdcID() {
    if (this.pdcID !== 'all') {
      this.buildingService.getBuildingByPdcID(+this.pdcID).subscribe(res => {
        this.buildings = res.map(item => {
          return { id: item.buildingID.toString(), text: item.buildingName };
        });
        this.buildings.unshift({ id: 'all', text: this.translate.instant('history.all_building') });
      });
      this.getListPlnoByPdcID();
      this.getListCellByPdcID();
      this.pdcIDTem = this.pdcID;
    } else {
      this.getAllBuilding();
      this.getAllCell();
      this.getAllCellPlno();
    }
  }

  getListCellByPdcID() {
    if (this.pdcID !== 'all') {
      this.cellService.getListCellByPdcID(+this.pdcID).subscribe(res => {
        this.cells = res.map(item => {
          return { id: item.cellCode, text: item.cellName };
        });
        this.cells.unshift({ id: 'all', text: this.translate.instant('history.all_unit') });
      });
    }
  }

  getListPlnoByPdcID() {
    if (this.pdcID !== 'all') {
      this.cellPlnoService.getListPlnoByPDCID(+this.pdcID).subscribe(res => {
        this.cellPlnos = res.map(item => {
          return { id: item.plno, text: item.place };
        });
        this.cellPlnos.unshift({ id: 'all', text: this.translate.instant('history.all_position') });
      });
    }
  }

  // Get ListCell
  getListCellByBuildingID() {
    if (this.buildingID !== 'all') {
      this.cellService.getListCellByBuildingID(+this.buildingID)
        .subscribe(res => {
          this.cells = res.map(item => {
            return { id: item.cellID.toString(), text: item.cellName };
          });
          this.cells.unshift({ id: 'all', text: this.translate.instant('history.all_unit') });
        });
      this.getListPlnoByBuildingID();
      this.getAllPdc();
      this.buildingIDTem = this.buildingID;
    } else if (this.pdcIDTem === 'all') {
      this.getAllBuilding();
      // this.getAllCell();
      this.getAllCellPlno();
    }
    this.searchHistory();
  }

  // Get ListPlno
  getListPlnoByBuildingID() {
    if (this.buildingID !== 'all') {
      this.cellPlnoService.getListPlnoByBuildingID(+this.buildingID).subscribe(res => {
        this.cellPlnos = res.map(item => {
          return { id: item.plno, text: item.place };
        });
        this.cellPlnos.unshift({ id: 'all', text: this.translate.instant('history.all_position') });
        this.pdcIDTem = 'all';
      });
    } else {
      this.getAllCellPlno();
    }
    this.searchHistory();
  }

  getListPlnoByCellID() {
    if (this.cellID !== 'all') {
      this.cellPlnoService.getListPlnoByCellID(this.cellID).subscribe(res => {
        this.cellPlnos = res.map(item => {
          return { id: item.plno, text: item.place };
        });
        this.cellPlnos.unshift({ id: 'all', text: this.translate.instant('history.all_position') });
        this.pdcIDTem = 'all';
        this.buildingIDTem = 'all';
      });
      this.getAllPdc();
      this.getAllBuilding();
    } else if (this.pdcIDTem === 'all' && this.buildingID === 'all') {
      this.getAllCell();
      if (this.cellPlnoID === 'all') {
        this.getAllCellPlno();
      }
    }
    this.searchHistory();
  }

  getPlano(event: any) {
    if (event !== 'all') {
      this.getAllCell();
    }
    this.searchHistory();
  }

  searchHistory() {
    this.pagination.currentPage = 1;
    this.loadHistory();
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page === undefined ? 1 : event.page;
    this.loadHistory();
  }

  exportExcelData() {
    this.searchHistoryService.exportExcelData(this.searchHistoryParams);
  }

  ngAfterContentChecked() {
    this.cdr.detectChanges();
    // call or add here your code
  }
}
