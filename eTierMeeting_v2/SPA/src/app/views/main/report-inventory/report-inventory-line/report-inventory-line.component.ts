import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { takeUntil, tap } from 'rxjs/operators';
import { DetailInventory, ResultAllInventory } from '../../../../_core/_dtos/detail-inventory-params';
import { ReportHistoryInventoryParams } from '../../../../_core/_dtos/ReportHistoryInventoryParams';
import { Building } from '../../../../_core/_models/building';
import { Cell } from '../../../../_core/_models/cell';
import { InventoryLine } from '../../../../_core/_models/inventoryLine';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { ReportInventoryService } from '../../../../_core/_services/report-inventory.service';
import { FunctionUtility } from '../../../../_core/_utility/function-utility';
import { DestroyService } from '../../../../_core/_services/destroy.service';

@Component({
  selector: 'app-report-inventory-line',
  templateUrl: './report-inventory-line.component.html',
  styleUrls: ['./report-inventory-line.component.scss']
})
export class ReportInventoryLineComponent implements OnInit {

  listBuilding: Building = {
    buildingID: null,
    buildingCode: null,
    buildingName: null,
    visible: null,
    code: null
  };

  listBuildingOrther: Cell = {
    buildingID: null,
    cellName: null,
    cellID: null,
    cellCode: null,
    code: null,
    pdcid: null,
    updateBy: null,
    updateTime: null,
    visible: null,
    buildingCode: null,
    buildingName: null,
    pdcName: null
  };

  listPlnoHistoryInventory: InventoryLine[] = [];
  listReportDetailHistoryInventory: ResultAllInventory = {
    listDetail: [],
    listResult: []
  };
  dateSearch: string = '';
  dataSearch: Date = null;
  idBuilding: number;
  isCheckLoad: string = '';
  listView: boolean = false;
  lang: string = '';

  dateSoKiem: string;
  datePhucKiem: string;
  dateRutKiem: string;

  detailSokiem: DetailInventory;
  detailPhuckiem: DetailInventory;
  detailRutkiem: DetailInventory;

  // date: Date = new Date();
  // dateNow: string = '';

  constructor(
    private reportInventoryService: ReportInventoryService,
    private alertifyService: AlertifyService,
    private router: Router,
    private spinnerService: NgxSpinnerService,
    private functionUtility: FunctionUtility,
    private translate: TranslateService,
    private localeService: BsLocaleService,
    private destroyService: DestroyService
  ) { }

  ngOnInit() {
    this.reportInventoryService.currentBuilding
      .pipe(takeUntil(this.destroyService.destroys$))
      .subscribe((buildingObject: any) => {
        if (buildingObject.buildingID === undefined) {
          this.backBuilding();
        } else {
          this.listBuilding = buildingObject;
          this.listBuildingOrther = buildingObject;
          this.getListHistory();
        }
      });
    this.dateLanguage();
  }

  dateLanguage() {
    this.lang = localStorage.getItem('lang');
    this.localeService.use(this.lang.substring(0, 2));
  }

  getListHistory() {
    this.spinnerService.show();
    let isCheck = 1;
    if (this.listBuilding.code !== 'building') {
      isCheck = 2;
      this.listBuilding.buildingID = this.listBuildingOrther.cellID;
    }
    this.dateSearch = this.functionUtility.getDateFormat(this.dataSearch);
    this.reportInventoryService.getListHistory(this.dateSearch, this.listBuilding.buildingID, isCheck)
      .pipe(
        tap(res => {
          res.map(item => {
            if (this.dataSearch === null) {
              const oneMonthsInMilliseconds = 1 * 30 * 24 * 60 * 60 * 1000;
              let compareDateSoKiem = 0;
              let compareDatePhucKiem = 0;
              let compareDateRutKiem = 0;

              compareDateSoKiem = new Date().getTime() - new Date(item.timeSoKiem).getTime();
              item.isTimeSoKiemValid = compareDateSoKiem > oneMonthsInMilliseconds ? false : true;

              compareDatePhucKiem = new Date().getTime() - new Date(item.timePhucKiem).getTime();
              item.isTimePhucKiemValid = compareDatePhucKiem > oneMonthsInMilliseconds ? false : true;

              compareDateRutKiem = new Date().getTime() - new Date(item.timeRutKiem).getTime();
              item.isTimeRutKiemValid = compareDateRutKiem > oneMonthsInMilliseconds ? false : true;
            } else {
              item.isTimeSoKiemValid = true;
              item.isTimePhucKiemValid = true;
              item.isTimeRutKiemValid = true;
            }
          });
        }))
      .subscribe(res => {
        this.listPlnoHistoryInventory = res;
        this.spinnerService.hide();
      });
  }

  backBuilding() {
    this.router.navigate(['/reportinventory']);
  }

  backline() {
    this.listView = false;
  }

  viewDetail(listDetail: InventoryLine) {
    this.spinnerService.show();
    this.lang = localStorage.getItem('lang');
    this.dateSoKiem = listDetail.timeSoKiem === null ? '' : this.functionUtility.getDateFormat(new Date(listDetail.timeSoKiem));
    this.datePhucKiem = listDetail.timePhucKiem === null ? '' : this.functionUtility.getDateFormat(new Date(listDetail.timePhucKiem));
    this.dateRutKiem = listDetail.timeRutKiem === null ? '' : this.functionUtility.getDateFormat(new Date(listDetail.timeRutKiem));
    this.reportInventoryService
      .GetListDetailHistoryInventory(listDetail.plnoId, this.dateSoKiem, this.datePhucKiem, this.dateRutKiem, this.lang)
      .pipe(
        tap(res => {
          res.listDetail.map(item => {
            if (this.dataSearch == null) {
              const oneMonthsInMilliseconds = 1 * 30 * 24 * 60 * 60 * 1000;
              let createTime = 0;

              createTime = new Date().getTime() - new Date(item.createTime).getTime();
              item.isCreateTime = createTime > oneMonthsInMilliseconds ? false : true;
            } else {
              item.isCreateTime = true;
            }
          });
        }))
      .subscribe(res => {
        this.listReportDetailHistoryInventory = res;
        this.detailSokiem = this.listReportDetailHistoryInventory.listDetail.find(x => x.typeInventory === 1 && x.isCreateTime === true);
        this.detailPhuckiem = this.listReportDetailHistoryInventory.listDetail.find(x => x.typeInventory === 2 && x.isCreateTime === true);
        this.detailRutkiem = this.listReportDetailHistoryInventory.listDetail.find(x => x.typeInventory === 3 && x.isCreateTime === true);
        this.listReportDetailHistoryInventory.listDetail.map(i => {
          if (i.typeInventory === 1 && i.isCreateTime === false) {
            this.listReportDetailHistoryInventory.listResult.map(i2 => {
              i2.statusNameSoKiem = null;
              i2.statusSoKiem = null;
            });
          } else if (i.typeInventory === 2 && i.isCreateTime === false) {
            this.listReportDetailHistoryInventory.listResult.map(i2 => {
              i2.statusNamePhucKiem = null;
              i2.statusPhucKiem = null;
            });
          } else if (i.typeInventory === 3 && i.isCreateTime === false) {
            this.listReportDetailHistoryInventory.listResult.map(i2 => {
              i2.statusNameRutKiem = null;
              i2.statusRutKiem = null;
            });
          }
        });
        this.spinnerService.hide();
        this.listView = true;
      }, error => {
        this.spinnerService.hide();
        this.alertifyService.error(this.translate.instant('error.system_error'));
      });
  }

  convertClass(status) {
    return status == null ? '' : status === 1 ? ' badge badge-success' : status === -1 ? 'badge badge-warning' : 'badge badge-danger';
  }

  exportPDF(reportHistory: ReportHistoryInventoryParams) {
    this.listPlnoHistoryInventory.map(i => {
      if (i.isTimeSoKiemValid === false) {
        i.timeSoKiem = null;
      }
      if (i.isTimePhucKiemValid === false) {
        i.timePhucKiem = null;
      }
      if (i.isTimeRutKiemValid === false) {
        i.timeRutKiem = null;
      }
    });

    reportHistory.lang = localStorage.getItem('lang');
    const dataPDF = { ...reportHistory };
    dataPDF.timeSoKiem = dataPDF.timeSoKiem === null ? '' : this.functionUtility
      .getDateFormat(new Date(dataPDF.timeSoKiem));
    dataPDF.timePhucKiem = dataPDF.timePhucKiem === null ? '' : this.functionUtility
      .getDateFormat(new Date(dataPDF.timePhucKiem));
    dataPDF.timeRutKiem = dataPDF.timeRutKiem === null ? '' : this.functionUtility
      .getDateFormat(new Date(dataPDF.timeRutKiem));
    dataPDF.typeFile = 'pdf';
    this.reportInventoryService.exportPDF(dataPDF);
  }

  exportExcel(reportHistory: ReportHistoryInventoryParams) {
    this.listPlnoHistoryInventory.map(i => {
      if (i.isTimeSoKiemValid === false) {
        i.timeSoKiem = null;
      }
      if (i.isTimePhucKiemValid === false) {
        i.timePhucKiem = null;
      }
      if (i.isTimeRutKiemValid === false) {
        i.timeRutKiem = null;
      }
    });
    reportHistory.lang = localStorage.getItem('lang');
    const dataExcel = { ...reportHistory };
    dataExcel.timeSoKiem = dataExcel.timeSoKiem === null ? '' : this.functionUtility
      .getDateFormat(new Date(dataExcel.timeSoKiem));
    dataExcel.timePhucKiem = dataExcel.timePhucKiem === null ? '' : this.functionUtility
      .getDateFormat(new Date(dataExcel.timePhucKiem));
    dataExcel.timeRutKiem = dataExcel.timeRutKiem === null ? '' : this.functionUtility
      .getDateFormat(new Date(dataExcel.timeRutKiem));
    dataExcel.typeFile = 'excel';
    this.reportInventoryService.exportExcel(dataExcel);
  }

}
