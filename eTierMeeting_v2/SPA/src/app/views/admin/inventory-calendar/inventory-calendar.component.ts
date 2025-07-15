import { Component, OnInit, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { AddDateInventory } from '../../../_core/_models/add-date-inventory';
import { DateInventory } from '../../../_core/_models/date-inventory';
import { OperationResult } from '../../../_core/_models/operation-result';
import { Pagination, PaginationResult } from '../../../_core/_models/pagination';
import { AlertifyService } from '../../../_core/_services/alertify.service';
import { DateInventoryService } from '../../../_core/_services/date-inventory.service';
import { SweetAlertService } from '../../../_core/_services/sweet-alert.service';

@Component({
  selector: 'app-inventorycalendar',
  templateUrl: './inventory-calendar.component.html',
  styleUrls: ['./inventory-calendar.component.scss']
})
export class InventoryCalendarComponent implements OnInit {
  fromDate: Date = new Date();
  toDate: Date = new Date();

  listDateInventory: DateInventory[] = [];
  pagination: Pagination = {
    currentPage: 1,
    pageSize: 4,
    totalCount: 0,
    totalPage: 0,
  };

  note: string = '';
  lang: string;

  listadd: AddDateInventory = {
    note: '',
    fromDate: null,
    toDate: null,
  };
  constructor(private _dateInventory: DateInventoryService,
    private _sweetAlert: SweetAlertService,
    private translate: TranslateService,
    private _alertifyService: AlertifyService,
    private localeService: BsLocaleService) { }

  ngOnInit() {
    this.searchDateInventory();
    this.dateLanguage();
  }

  dateLanguage() {
    this.lang = localStorage.getItem('lang');
    this.localeService.use(this.lang.substring(0, 2));
  }

  loadData() {
    this._dateInventory.GetAllDateInventories(this.pagination).subscribe((res: PaginationResult<DateInventory>) => {
      this.listDateInventory = res.result;
      this.pagination = res.pagination;
    }, error => {
      console.log(error);
    });
  }

  searchDateInventory() {
    this.pagination.currentPage = 1;
    this.loadData();
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page === undefined ? 1 : event.page;
    this.loadData();
  }

  addDateInventory() {
    this.lang = localStorage.getItem('lang');
    this.listadd.fromDate = this.fromDate;
    this.listadd.toDate = this.toDate;

    if (this.listadd.fromDate == null) {
      return this._sweetAlert.error('Error', this.translate.instant('alert.admin.please_select_date_add_to_the_inventory_calendar'));
    }

    if (this.listadd.toDate == null) {
      return this._sweetAlert.error('Error', this.translate.instant('alert.admin.please_select_date_add_to_the_inventory_calendar'));
    }

    if (this.listadd.toDate < this.listadd.fromDate) {
      return this._sweetAlert.error('Error', this.translate.instant('alert.admin.the_end_date_is_greater_than_he_start_date'));
    }

    this._dateInventory.addDateInventory(this.listadd, this.lang).subscribe(res => {
      if (res.success) {
        this._sweetAlert.success('Success', res.message);
        this.clearFormAdd();

      } else {
        this._sweetAlert.error('Error', res.message);
      }

    }, error => {
      this._alertifyService.error(this.translate.instant('error.system_error'));
    });
  }

  removeDateInventory(id: number) {
    this.lang = localStorage.getItem('lang');
    this._sweetAlert.confirm('Delete', this.translate.instant('alert.admin.delete_inventory_schedule'), () => {
      this._dateInventory.removeDateInventory(id, this.lang).subscribe((res: OperationResult) => {
        if (res.success) {
          this._sweetAlert.success('Success', res.message);
          this.searchDateInventory();
        } else {
          this._sweetAlert.error('Failed', res.message);
        }
      });
    });
  }

  clearFormAdd() {
    this.listadd.note = '';
    this.searchDateInventory();
  }
}


