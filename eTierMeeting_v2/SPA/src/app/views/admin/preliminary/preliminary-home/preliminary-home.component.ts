import { Component, OnInit, ViewChild } from '@angular/core';
import { UntypedFormControl } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { Select2OptionData } from 'ng-select2';
import { NgxSpinnerService } from 'ngx-spinner';
import { Pagination } from '../../../../_core/_models/pagination';
import { PreliminaryList } from '../../../../_core/_models/preliminary-list';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { PreliminaryListService } from '../../../../_core/_services/preliminary-list.service';
import { SweetAlertService } from '../../../../_core/_services/sweet-alert.service';
import { OperationResult } from '../../../../_core/_models/operation-result';
import { PreliminaryAddComponent } from '../preliminary-add/preliminary-add.component';
@Component({
  selector: 'app-preliminary-home',
  templateUrl: './preliminary-home.component.html',
  styleUrls: ['./preliminary-home.component.scss'],
})
export class PreliminaryHomeComponent implements OnInit {
  @ViewChild('childAdd', { static: false }) childAdd: PreliminaryAddComponent;
  listCells: Array<Select2OptionData>;
  listBuildings: Array<Select2OptionData>;
  listPlnos: Array<Select2OptionData>;
  listPreliminary: PreliminaryList[] = [];
  cellPlnoID: string = 'all';
  cellID: string = 'all';
  buildingID: string = 'all';
  search: string = '';

  keyword: UntypedFormControl = new UntypedFormControl('');
  lang: string;

  pagination: Pagination = {
    currentPage: 1,
    pageSize: 6,
    totalCount: 0,
    totalPage: 0,
  };
  constructor(
    private _spinner: NgxSpinnerService,
    private _translate: TranslateService,
    private _preliminaryList: PreliminaryListService,
    private _sweetAlert: SweetAlertService,
    private _alertifyService: AlertifyService
  ) { }

  ngOnInit() {
    this.loadData();
  }
  loadData() {
    this._preliminaryList
      .getPreliminaryList(this.pagination, this.search)
      .subscribe(
        (res) => {
          this.listPreliminary = res.result;
          this.pagination = res.pagination;
        },
        (error) => {
          this._alertifyService.error(
            this._translate.instant('error.system_error')
          );
        }
      );

  }
  searchData() {
    this.pagination.currentPage = 1;
    this.loadData();
    console.log("listPreliminary", this.listPreliminary)
  }
  clearSearch() {
    this.search = '';
    this.loadData();
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page === undefined ? 1 : event.page;
    this.loadData();
  }

  exportExcel() {
    this._preliminaryList.exportExcel(this.search);
  }

  keywordAdd(event) {
    this.search = event;
    this.loadData();
  }
  removeUserRole(empNumber) {
    this.lang = localStorage.getItem('lang');
    this._sweetAlert.confirm(
      'Delete',
      this._translate.instant('alert.do_you_want_delete'),
      () => {
        this._preliminaryList
          .removePreliminary(empNumber, this.lang)
          .subscribe((res: OperationResult) => {
            if (res.success) {
              this._sweetAlert.success('Success', res.message);
              this.childAdd.getAllUser();
              this.loadData();
            } else {
              this._sweetAlert.error('Failed', res.message);
            }
          });
      }
    );
  }
}
