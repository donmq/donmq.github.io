import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ReportKiemKeParam } from '../../../../_core/_dtos/ReportHistoryInventoryParams';
import { Building } from '../../../../_core/_models/building';
import { Cell } from '../../../../_core/_models/cell';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { ReportInventoryService } from '../../../../_core/_services/report-inventory.service';
import { FunctionUtility } from '../../../../_core/_utility/function-utility';

@Component({
  selector: 'app-report-inventory-building',
  templateUrl: './report-inventory-building.component.html',
  styleUrls: ['./report-inventory-building.component.scss']
})
export class ReportInventoryBuildingComponent implements OnInit {
  buildings: Building[] = [];
  listBuilding: any = [];
  listBuildingOrther: any = [];
  lang: string = localStorage.getItem('lang');
  fromDate: Date = null;
  toDate: Date = null;
  param: ReportKiemKeParam = <ReportKiemKeParam>{
    lang: '',
    fromDate: '',
    toDate: ''
  }
  datePickerConfig: Partial<BsDatepickerConfig> = { dateInputFormat: 'YYYY/MM/DD', showClearButton: true, clearPosition: 'right' };
  constructor(
    private reportInventoryService: ReportInventoryService,
    private spinnerService: NgxSpinnerService,
    private router: Router,
    private localeService: BsLocaleService,
    private functionUtility: FunctionUtility,
    private translate: TranslateService,
    private alertifyService: AlertifyService,
  ) { }

  ngOnInit() {
    this.getBuilding();
    this.getBuildingOrther();
    this.dateLanguage();
  }
  dateLanguage() {
    this.lang = localStorage.getItem('lang');
    this.localeService.use(this.lang.substring(0, 2));
  }

  getBuilding() {
    this.spinnerService.show();
    this.reportInventoryService.getBuilding().subscribe(res => {
      this.listBuilding = res;
      this.spinnerService.hide();
    });
  }

  getBuildingOrther() {
    this.spinnerService.show();
    this.reportInventoryService.getBuildingOther(100).subscribe(res => {
      this.listBuildingOrther = res;
      this.spinnerService.hide();
    });
  }

  selectBuilding(building: Building) {
    const buildingObject = { ...building, code: 'building' };
    this.spinnerService.show();
    this.reportInventoryService.getBuilding().subscribe(res => {
      this.reportInventoryService.changeBuilding(buildingObject);
      this.router.navigate(['/reportinventory/line']);
      this.spinnerService.hide();
    });
  }

  selectOrther(cell: Cell) {
    const buildingObject = { ...cell, code: 'buiding' };
    this.spinnerService.show();
    this.reportInventoryService.getBuildingOther(cell.cellID).subscribe(res => {
      this.reportInventoryService.changeBuilding(buildingObject);
      this.router.navigate(['/reportinventory/line']);
      this.spinnerService.hide();
    });
  }
  exportExcel() {
    if (this.validateDate())
      return this.alertifyService.error(this.translate.instant('historyinventory.please_select_date'));
    else {
      this.param.lang = this.lang
      this.param.fromDate = this.fromDate != null ? this.functionUtility.getDateFormat(this.fromDate) : '';
      this.param.toDate = this.toDate != null ? this.functionUtility.getDateFormat(this.toDate) : '';
      this.reportInventoryService.exportExcelAllInventory(this.param);
    }

  }

  validateDate(): boolean {
    if ((this.fromDate != null && this.toDate == null) || (this.fromDate == null && this.toDate != null)) {
      return true
    }
  }

}
