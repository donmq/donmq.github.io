import { Component, ElementRef, OnInit, TemplateRef, ViewChild } from '@angular/core';
import { Pagination } from './../../../../_core/_models/pagination';
import { AssetsLendMaintainDto, AssetsLendMaintainParam } from '../../../../_core/_models/assets-lend-maintain';
import { AssetsLendMaintainService } from '../../../../_core/_services/assets-lend-maintain.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { KeyValue } from '@angular/common';
import { KeyValuePair } from '../../../../_core/_utility/key-value-pair';
import { FunctionUtility } from '../../../../_core/_utility/function-utility';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { EditComponent } from '../edit/edit.component';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit {
  param: AssetsLendMaintainParam = {
    machineID: null,
    lendDate: '',
    lendTo: null,
    return: 'all'
  }
  pagination: Pagination = {
    currentPage: 1,
    pageSize: 10,
    totalCount: 0,
    totalPage: 0,
  };

  lendDate: Date = null
  listLendTo: KeyValuePair[] = [];
  data: AssetsLendMaintainDto[] = []
  @ViewChild('inputRef') inputRef: ElementRef<HTMLInputElement>;
  extensions: string = '.xls, .xlsm, .xlsx';
  title: string = '';
  modalRef?: BsModalRef;
  constructor(private service: AssetsLendMaintainService,
    private spinner: NgxSpinnerService,
    private functionUtility: FunctionUtility,
    private _alertifyService: AlertifyService,
    private modalService: BsModalService) { }

  ngOnInit() {
    this.getListLendTo();
  }
  getData() {
    this.spinner.show();
    this.param.lendDate = this.lendDate != null ? this.functionUtility.getDateFormat(this.lendDate) : '';
    this.service.getData(this.pagination, this.param).subscribe({
      next: (res) => {
        this.data = res.result;
        this.pagination = res.pagination;
        this.spinner.hide();
      }, error: () => {
        this.spinner.hide();
      }
    });
  }
  dateCheck() { }
  search() {
    this.pagination.currentPage = 1;
    this.getData();
  }
  openEditModal(item: AssetsLendMaintainDto) {
    const initState: ModalOptions = {
      initialState: {
        data: item,
        type: 'edit'
      } as any // Bỏ qua kiểm tra kiểu dữ liệu
    };

    this.modalRef = this.modalService.show(EditComponent, initState);

  }
  download() {
    // this.checkDate()
    this.spinner.show();
    this.service.downloadExcel(this.param).subscribe({
      next: (result: Blob) => {
        this.functionUtility.exportExcel(result, 'AssetsLendMaintainReport');
      },
      error: () => {
        this.spinner.hide()
      },
    })
  }
  upload(event: any) {
    this.spinner.show();

    if (event.target.files && event.target.files[0]) {
      const fileNameExtension = event.target.files[0].name.split('.').pop();
      if (!this.extensions.includes(fileNameExtension)) {
        this.spinner.hide();
        return this._alertifyService.error('Please select a file excel');
      } else {
        this.spinner.show();
        this.service.uploadExcel(event.target.files[0]).subscribe({
            next: (res) => {
              if (res.success) {
                event.target.value = '';
                this._alertifyService.success(res.message);
              } else {
                this._alertifyService.error(res.message);
              }
              this.spinner.hide();
              this.inputRef.nativeElement.value = '';
            },
            error: () => {
              event.target.value = '';
              this._alertifyService.error('Oops! Sorry, an error occurred while processing your request');
            },
          })
          .add(() => this.spinner.hide());
      }
    }
  }
  getListLendTo() {
    // this.service.getListLendTo().subscribe(res => {
    //   this.listLendTo = res;
    this.listLendTo.push({ key: '1', value: 'SHC' });
    this.listLendTo.push({ key: '2', value: 'CB' });
    this.listLendTo.push({ key: '3', value: 'TSH' });
    // });
  }

}
