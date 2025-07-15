import { Component, OnInit, ViewChild } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Select2OptionData } from 'ng-select2';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { CellPlno } from '../../../../_core/_models/cell-plno';
import { OperationResult } from '../../../../_core/_models/operation-result';
import { Pagination, PaginationResult } from '../../../../_core/_models/pagination';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { CellService } from '../../../../_core/_services/cell.service';
import { CellPlnoService } from '../../../../_core/_services/cellplno.service';
import { SweetAlertService } from '../../../../_core/_services/sweet-alert.service';
import { ChangeDetectorRef } from '@angular/core';
@Component({
  selector: 'app-cellplno-home',
  templateUrl: './cellplno-home.component.html',
  styleUrls: ['./cellplno-home.component.scss']
})
export class CellplnoHomeComponent implements OnInit {
  @ViewChild('childModal', { static: false }) childModal: ModalDirective;
  listCellPlnos: CellPlno[];

  cells: Array<Select2OptionData>;
  cell_Plnos: Array<Select2OptionData>;

  keyword: string = '';
  lang: string;
  cellPlnoUpdate: any = [];

  pagination: Pagination = {
    currentPage: 1,
    pageSize: 5,
    totalCount: 0,
    totalPage: 0,
  };

  constructor(
    private cellPlnoService: CellPlnoService,
    private cellService: CellService,
    private spinnerService: NgxSpinnerService,
    private sweetAlertifyService: SweetAlertService,
    private translate: TranslateService,
    private alertifyService: AlertifyService,
    private cdref: ChangeDetectorRef
  ) { }

  ngAfterContentChecked() {
    this.cdref.detectChanges();
  }
  ngOnInit() {
    this.getAllCell();
    this.getAllCellPlno();
    this.loadData();
  }

  loadData() {
    this.spinnerService.show();
    this.cellPlnoService.searchCellPlno(this.keyword, this.pagination).subscribe((res: PaginationResult<CellPlno>) => {
      this.listCellPlnos = res.result;
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

  searchCellPlno() {
    this.pagination.currentPage = 1;
    this.loadData();
  }

  clearSearch() {
    this.pagination.currentPage = 1;
    this.keyword = '';
    this.loadData();
  }

  getAllCell() {
    this.cellService.getAllCellAdmin().subscribe(res => {
      this.cells = res.map(item => {
        return { id: item.cellID.toString(), text: item.cellCode + '-' + item.cellName + '-' + item.buildingCode };
      });
      this.cells.unshift({ id: '0', text: this.translate.instant('admin.admincellplno.all_unit') });
    });
  }

  getAllCellPlno() {
    this.cellPlnoService.getAllCellPlno().subscribe(res => {
      this.cell_Plnos = res.map(item => {
        return { id: item.plno, text: item.plno + '-' + item.place };
      });
      this.cell_Plnos.unshift({ id: 'all', text: this.translate.instant('admin.admincellplno.all_position') });
    });
  }

  keywordAdd(value: string) {
    this.keyword = value;
    this.searchCellPlno();
  }

  cancel() {
    this.searchCellPlno();
  }

  removeCellPlno(cellPlno: CellPlno) {
    this.lang = localStorage.getItem('lang');
    this.sweetAlertifyService.confirm('Delete', this.translate.instant('alert.admin.are_you_sure_to_delete_this_cell_plno'), () => {
      this.cellPlnoService.removeCellPlno(cellPlno, this.lang).subscribe((res: OperationResult) => {
        if (res.success) {
          this.sweetAlertifyService.success('Success', res.message);
          this.loadData();
          this.keyword = '';
        } else {
          this.sweetAlertifyService.error('Failed', res.message);
        }
      });
    });
  }

  updateCellPlno(cellPlno: CellPlno) {
    this.lang = localStorage.getItem('lang');
    if (this.cellPlnoUpdate.cellID == 0) {
      return this.sweetAlertifyService.warning('Error', this.translate.instant('alert.admin.please_select_the_unit'));
    }
    if (this.cellPlnoUpdate.plno == 'all') {
      return this.sweetAlertifyService.warning('Error', this.translate.instant('alert.admin.please_select_a_location'));
    }

    this.cellPlnoUpdate.cellID = +this.cellPlnoUpdate.cellID;
    this.cellPlnoUpdate.plno = this.cellPlnoUpdate.plno;

    this.cellPlnoService.updateCellPlno(cellPlno, this.lang).subscribe((res: OperationResult) => {
      if (res.success) {
        this.sweetAlertifyService.success('Success', res.message);
        this.childModal.hide();
        this.keyword = this.cellPlnoUpdate.plno;
        this.cancel();
      } else {
        this.sweetAlertifyService.error('Error', res.message);
      }
    });
  }

  showChildModal(cellPlno: CellPlno): void {
    const cellPlnoObject = {
      id: cellPlno.id,
      cellID: cellPlno.cellID,
      plno: cellPlno.plno
    };
    this.cellPlnoUpdate = cellPlnoObject;
    this.childModal.show();
  }

  hideChildModal(): void {
    this.childModal.hide();
  }

  exportExcelData() {
    this.cellPlnoService.exportExcelData();
  }

}
