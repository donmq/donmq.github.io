import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { CheckMachineHisstoryParams } from '../../../../_core/_dtos/search-check-machine-history-params';
import { DataHistoryCheckMachine } from '../../../../_core/_models/data-history-check-machine';
import { HistoryCheckMachine } from '../../../../_core/_models/list-history-check-machine';
import { Pagination } from '../../../../_core/_models/pagination';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { CheckMachineHistoryService } from '../../../../_core/_services/check-machine-history.service';
import { FunctionUtility } from '../../../../_core/_utility/function-utility';

@Component({
  selector: 'app-history-check-machine-home',
  templateUrl: './history-check-machine-home.component.html',
  styleUrls: ['./history-check-machine-home.component.scss']
})
export class HistoryCheckMachineHomeComponent implements OnInit {
  listCheckMachineHisstory: HistoryCheckMachine[] = [];
  listViewCheckMachineHisstory: any = [];
  fromDate: Date = null;
  toDate: Date = null;
  userName: string = '';
  listView: boolean = false;
  historyCheckMachineID: number;
  lang: string;

  checkMachineHisstoryParams: CheckMachineHisstoryParams = {
    userName: '',
    fromDateTime: null,
    toDateTime: null,
  };
  pagination: Pagination = {
    currentPage: 1,
    pageSize: 10,
    totalCount: 0,
    totalPage: 0,
  };

  constructor(
    private _checkMachineHistoryService: CheckMachineHistoryService,
    private _spinnerService: NgxSpinnerService,
    private _functionUtility: FunctionUtility,
    private _alertifyService: AlertifyService,
    private translate: TranslateService,
    private localeService: BsLocaleService) { }

  ngOnInit() {
    this.loadData();
    this.dateLanguage();
  }

  dateLanguage() {
    this.lang = localStorage.getItem('lang');
    this.localeService.use(this.lang.substring(0, 2));
  }
  loadData() {
    this._spinnerService.show();
    this.checkMachineHisstoryParams.fromDateTime = this._functionUtility.getDateFormat(this.fromDate);
    this.checkMachineHisstoryParams.toDateTime = this._functionUtility.getDateFormat(this.toDate);
    this.checkMachineHisstoryParams.userName = this.userName;
    this._checkMachineHistoryService.searchHistoryCheckMachine(this.checkMachineHisstoryParams, this.pagination).subscribe(res => {
      this.listCheckMachineHisstory = res.result;
      this.pagination = res.pagination;
      this._spinnerService.hide();
    }, error => {
      this._spinnerService.hide();
      this._alertifyService.error(this.translate.instant('error.system_error'));
    });
  }

  searchMachine() {
    this.pagination.currentPage = 1;
    this.loadData();
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page === undefined ? 1 : event.page;
    this.loadData();
  }

  viewDetail(dataHistoryCheckMachine: DataHistoryCheckMachine) {
    this._spinnerService.show();
    this._checkMachineHistoryService.getDetailHistoryCheckMachine(dataHistoryCheckMachine.historyCheckMachineID).subscribe(res => {
      this.listViewCheckMachineHisstory = res;
      this._spinnerService.hide();
      this.listView = true;
    }, error => {
      this._spinnerService.hide();
      this._alertifyService.error(this.translate.instant('error.system_error'));
    });
  }

  listViewShow(value: boolean) {
    this.listView = value;
  }

  exportPDF(historyCheckMachine: HistoryCheckMachine) {
    historyCheckMachine.typeFile = 'pdf';
    this._checkMachineHistoryService.exportPDF(historyCheckMachine);
  }

  exportExcel(historyCheckMachine: HistoryCheckMachine) {
    historyCheckMachine.typeFile = 'excel';
    this._checkMachineHistoryService.exportExcel(historyCheckMachine);
  }
}
