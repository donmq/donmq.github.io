import { Component, OnInit } from '@angular/core';
import { T5ExternalUploadService } from '@services/t5-external-upload.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { NgSnotifyService } from '@services/ng-snotify.service';
import { Pagination } from '@utilities/pagination-helper';
import { eTM_HP_Efficiency_Data_External } from '@models/eTM_HP_Efficiency_Data_External';
import { CaptionConstants, MessageConstants } from '@constants/system.constants';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';
// import { FunctionUtility } from '@utilities/function-utility';
import { InjectBase } from 'src/app/_core/_utilities/inject-base-app';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent extends InjectBase implements OnInit {

  data: eTM_HP_Efficiency_Data_External[] = [];
  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 10
  }
  constructor(private t5ExternalUploadService: T5ExternalUploadService) {
    super();
  }

  ngOnInit() {
  }

  getData() {
    this.spinnerService.show();
    this.t5ExternalUploadService.getData(this.pagination).subscribe({
      next: res => {
        this.data = res.result;
        this.pagination = res.pagination;
        this.spinnerService.hide();
      },
      error: () => {
        this.spinnerService.hide();
        this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR);
      }
    })
  }

  getTemplate() {
    this.spinnerService.show();
    this.t5ExternalUploadService.getTemplate().subscribe({
      next: res => {
        this.functionUtility.exportExcel(res, "T5_External_Upload");
        this.spinnerService.hide();
      },
      error: () => {
        this.spinnerService.hide();
      }
    })
  }

  clearFile() {
    (<HTMLInputElement>document.getElementById("upload_File")).value = null;
  }

  uploadExcel(event: any) {
    if (event.target.files[0] && event.target.files[0].name.includes('xlsx')) {
      const form = new FormData();
      form.append('file', event.target.files[0]);
      this.spinnerService.show();
      this.t5ExternalUploadService.uploadExcel(form).subscribe({
        next: (res) => {
          if (res.success) {
            this.snotifyService.success(MessageConstants.UPLOAD_OK_MSG, CaptionConstants.SUCCESS);
            this.pagination.pageNumber = 1;
            this.getData();
          } else {
            this.snotifyService.error(res.message, CaptionConstants.ERROR);
          }
          this.spinnerService.hide();
          this.clearFile();
        },
        error: () => {
          this.clearFile();
          this.spinnerService.hide();
          this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR);
        }
      })
    }
  }

  pageChanged(e: PageChangedEvent) {
    this.pagination.pageNumber = e.page;
    this.getData();
  }

}
