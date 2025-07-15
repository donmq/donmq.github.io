import { Component, OnInit, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { OperationResult } from '../../../../_core/_models/operation-result';
import { Pagination, PaginationResult } from '../../../../_core/_models/pagination';
import { Pdc } from '../../../../_core/_models/pdc';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { PdcService } from '../../../../_core/_services/pdc.service';
import { SweetAlertService } from '../../../../_core/_services/sweet-alert.service';

@Component({
  selector: 'app-pdc-home',
  templateUrl: './pdc-home.component.html',
  styleUrls: ['./pdc-home.component.scss']
})
export class PdcHomeComponent implements OnInit {
  @ViewChild('childModal', { static: false }) childModal: ModalDirective;
  listPdcs: Pdc[];
  keyword: string = '';
  lang: string;
  pdcUpdate: any = [];

  pagination: Pagination = {
    currentPage: 1,
    pageSize: 5,
    totalCount: 0,
    totalPage: 0,
  };

  constructor(
    private pdcService: PdcService,
    private sweetAlertService: SweetAlertService,
    private spinnerService: NgxSpinnerService,
    private translate: TranslateService,
    private alertifyService: AlertifyService,
  ) { }

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    this.spinnerService.show();
    this.pdcService.searchPDC(this.keyword, this.pagination).subscribe((res: PaginationResult<Pdc>) => {
      this.listPdcs = res.result;
      this.pagination = res.pagination;
      this.spinnerService.hide();
    }, error => {
      this.alertifyService.error(this.translate.instant('error.system_error'));
      this.spinnerService.hide();
    });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page === undefined ? 1 : event.page;
    this.loadData();
  }

  searchPDC() {
    this.pagination.currentPage = 1;
    this.loadData();
  }

  clearSearch() {
    this.pagination.currentPage = 1;
    this.keyword = '';
    this.loadData();
  }

  removePDC(pdc: Pdc) {
    this.lang = localStorage.getItem('lang');
    this.sweetAlertService.confirm('Delete', this.translate.instant('alert.admin.are_you_sure_to_delete_this_pdc'), () => {
      this.pdcService.removePDC(pdc, this.lang).subscribe((res: OperationResult) => {
        if (res.success) {
          this.sweetAlertService.success('Success', res.message);
          this.loadData();
          this.keyword = '';
        } else {
          this.sweetAlertService.error('Failed', res.message);
        }
      });
    });
  }

  updatePDC() {
    this.lang = localStorage.getItem('lang');
    if (this.pdcUpdate.pdcName === null || this.pdcUpdate.pdcName.trim() === '') {
      this.sweetAlertService.warning('Error', this.translate.instant('alert.admin.please_enter_pdc_name'));
      return;
    }
    if (this.pdcUpdate.pdcCode === null || this.pdcUpdate.pdcCode.trim() === '') {
      this.sweetAlertService.warning('Error', this.translate.instant('alert.admin.please_enter_pdc_code'));
      return;
    }
    this.pdcUpdate.pdcName = this.pdcUpdate.pdcName.trim();
    this.pdcUpdate.pdcCode = this.pdcUpdate.pdcCode.trim();
    this.pdcService.updatePDC(this.pdcUpdate, this.lang).subscribe((res: OperationResult) => {
      if (res.success) {
        this.sweetAlertService.success('Success', res.message);
        this.keyword = this.pdcUpdate.pdcCode;
        this.searchPDC();
        this.childModal.hide();
      } else {
        this.sweetAlertService.error('Error', res.message);
      }
    }, error => {
      this.alertifyService.error(this.translate.instant('error.system_error'));
    });
  }

  showChildModal(pdc: Pdc): void {
    const pdcObject = {
      pdcName: pdc.pdcName,
      pdcCode: pdc.pdcCode,
      pdcID: pdc.pdcid
    };
    this.pdcUpdate = pdcObject;
    this.childModal.show();
  }

  keywordAdd(value: string) {
    this.keyword = value;
    this.searchPDC();
  }

  hideChildModal(): void {
    this.childModal.hide();
  }
}
