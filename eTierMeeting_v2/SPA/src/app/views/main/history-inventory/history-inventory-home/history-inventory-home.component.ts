import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Select2OptionData } from 'ng-select2';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { HistoryInventoryParams } from '../../../../_core/_dtos/history-inventory-params';
import { HistoryInventory } from '../../../../_core/_models/history-inventory';
import { Pagination } from '../../../../_core/_models/pagination';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { BuildingService } from '../../../../_core/_services/building.service';
import { CellService } from '../../../../_core/_services/cell.service';
import { CellPlnoService } from '../../../../_core/_services/cellplno.service';
import { HistoryInventoryService } from '../../../../_core/_services/history-inventory.service';
import { FunctionUtility } from '../../../../_core/_utility/function-utility';

@Component({
  selector: 'app-history-inventory-home',
  templateUrl: './history-inventory-home.component.html',
  styleUrls: ['./history-inventory-home.component.scss']
})
export class HistoryInventoryHomeComponent implements OnInit {
  historyInventorys: HistoryInventory[] = [];
  dataHistoryInventorys: any = [];
  buildings: Array<Select2OptionData>;
  cells: Array<Select2OptionData>;
  cellPlnos: Array<Select2OptionData>;

  historyInventoryID: number;
  listView: boolean = false;
  isChooseDate: boolean = false;

  cellID: string = 'all';
  buildingID: string = 'all';
  idPlnos: string = 'all';
  inventoryID: string = '0';
  fromDate: Date = null;
  toDate: Date = null;
  checkDate: string;
  checkData: Date = null;

  pdcIDTem: string = '';
  lang: string;
  historyInventoryParams: HistoryInventoryParams = {
    fromDateTime: null,
    toDateTime: null,
    idInventory: 0,
    idPlno: ''
  };

  pagination: Pagination = {
    currentPage: 1,
    pageSize: 10,
    totalCount: 0,
    totalPage: 0,
  };

  constructor(
    private historyInventoryService: HistoryInventoryService,
    private cellPlnoService: CellPlnoService,
    private buildingService: BuildingService,
    private cellService: CellService,
    private alertifyService: AlertifyService,
    private spinnerService: NgxSpinnerService,
    private functionUtility: FunctionUtility,
    private translate: TranslateService,
    private localeService: BsLocaleService,
    private cdr: ChangeDetectorRef
  ) { }

  ngOnInit() {
    this.getAllBuilding();
    this.getAllCell();
    this.getListAllPlno();
    this.loadData();
    this.dateLanguage();
  }

  dateLanguage() {
    this.lang = localStorage.getItem('lang');
    this.localeService.use(this.lang.substring(0, 2));
  }

  loadData() {
    this.spinnerService.show();
    this.historyInventoryParams.fromDateTime = this.functionUtility.getDateFormat(this.fromDate);
    this.historyInventoryParams.toDateTime = this.functionUtility.getDateFormat(this.toDate);
    this.historyInventoryParams.idInventory = this.inventoryID === '0' ? 0 : +this.inventoryID;
    this.historyInventoryParams.idPlno = this.idPlnos === 'all' ? '' : this.idPlnos;
    this.historyInventoryService.searchHistoryInventory(this.pagination, this.historyInventoryParams)
      .subscribe(res => {
        this.historyInventorys = res.result;
        this.pagination = res.pagination;
        this.spinnerService.hide();
      }, error => {
        this.spinnerService.hide();
        this.alertifyService.error(this.translate.instant('error.system_error'));
      });
  }

  getListAllPlno() {
    this.idPlnos = 'all';
    this.cellPlnoService.getAllCellPlno().subscribe(res => {
      this.cellPlnos = res.map(item => {
        return { id: item.plno, text: item.place };
      });
      this.cellPlnos.unshift({ id: 'all', text: this.translate.instant('historyinventory.all_position') });
    });
  }

  getAllBuilding() {
    this.buildingID = 'all';
    this.buildingService.getAllBuilding().subscribe(res => {
      this.buildings = res.map(item => {
        return { id: item.buildingID.toString(), text: item.buildingName };
      });
      this.buildings.unshift({ id: 'all', text: this.translate.instant('historyinventory.all_building') });
    });
  }

  getAllCell() {
    this.cellID = 'all';
    this.cellService.getAllCell().subscribe(res => {
      this.cells = res.map(item => {
        return { id: item.cellCode, text: item.cellCode + '-' + item.cellName };
      });
      this.cells.unshift({ id: 'all', text: this.translate.instant('history.cell') });
    });
  }

  // Get listbuilding
  getListBuildingByCell() {
    if (this.cellID !== 'all') {
      this.buildingService.getBuildingByCellCodeAndPDC(this.cellID)
        .subscribe(res => {
          this.buildings = res.map(item => {
            return { id: item.buildingID.toString(), text: item.buildingName };
          });
          this.buildings.unshift({ id: 'all', text: this.translate.instant('historyinventory.all_building') });
        });
      this.getListPlnoByCellID();
    } else if (this.pdcIDTem === 'all') {
      this.getAllBuilding();
      this.getListAllPlno();
    }
    // this.searchHistoryInventory();
  }

  getListPlnoByCellID() {
    if (this.cellID !== 'all') {
      this.cellPlnoService.getListPlnoByCellID(this.cellID).subscribe(res => {
        this.cellPlnos = res.map(item => {
          return { id: item.plno, text: item.place };
        });
        this.cellPlnos.unshift({ id: 'all', text: this.translate.instant('historyinventory.all_position') });
      });
    } else {
      this.getAllBuilding();
      this.getAllCell();
      this.getListAllPlno();
    }
    // this.searchHistoryInventory();
  }

  getListPlnoByBuildingID() {
    if (this.buildingID !== 'all') {
      this.cellPlnoService.getListPlnoByBuildingID(+this.buildingID).subscribe(res => {
        this.cellPlnos = res.map(item => {
          return { id: item.plno, text: item.place };
        });
        this.cellPlnos.unshift({ id: 'all', text: this.translate.instant('historyinventory.all_position') });
        this.pdcIDTem = 'all';
      });
      this.getAllCell();
    } else if (this.pdcIDTem === 'all' && this.cellID === 'all') {
      this.getAllBuilding();
      if (this.idPlnos === 'all') {

        this.getListAllPlno();
      }
    }
    this.searchHistoryInventory();
  }

  getPlano(event: any) {
    if (event !== 'all') {
      this.getAllBuilding();
    }

    this.searchHistoryInventory();
  }

  listViewShow(value: boolean) {
    this.listView = value;
  }

  convertInventoryType(InventoryType) {
    return InventoryType === 1 ? this.translate.instant('historyinventory.so_kiem') : InventoryType === 2 ? this.translate.instant('historyinventory.phuc_kiem') : this.translate.instant('historyinventory.rut_kiem');
  }

  searchHistoryInventory() {
    this.pagination.currentPage = 1;
    this.loadData();
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page === undefined ? 1 : event.page;
    this.loadData();
  }

  viewDetail(historyInventoryID) {
    this.spinnerService.show();
    this.historyInventoryService.getDetailHistoryInventory(historyInventoryID).subscribe(res => {
      this.dataHistoryInventorys = res;
      this.spinnerService.hide();
      this.listView = true;
    }, error => {
      this.spinnerService.hide();
      this.alertifyService.error(this.translate.instant('error.system_error'));
    });
  }

  exportPDF(historyInventory: HistoryInventory) {
    historyInventory.typeFile = 'pdf';
    this.historyInventoryService.exportPDF(historyInventory);
  }

  exportExcel(historyInventory: HistoryInventory) {
    historyInventory.typeFile = 'excel';
    this.historyInventoryService.exportExcel(historyInventory);
  }

  exportPdfByDay(date: string) {
    date = this.functionUtility.getDateFormat(this.checkData);
    this.historyInventoryService.exportPdfByDay(date);
  }

  dateCheck() {
    this.checkDate = this.functionUtility.getDateFormat(this.checkData);
    this.historyInventoryService.dateCheck(this.checkDate).subscribe(res => {
      if (res) {
        this.isChooseDate = true;
      } else {
        this.isChooseDate = false;
        this.alertifyService.error(this.translate.instant('alert.alert_report_date_invalid'));
      }
    }, error => {
      this.spinnerService.hide();
      this.alertifyService.error(this.translate.instant('error.system_error'));
    });
  }

  ngAfterContentChecked() {
    this.cdr.detectChanges();
    // call or add here your code
  }
}
