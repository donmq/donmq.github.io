import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { UploadDataHpA15Service } from '../../../_core/_services/upload-data-hp-a15.service';
import { AlertifyService } from '../../../_core/_services/alertify.service';

@Component({
  selector: 'app-upload-data-hp-a15',
  templateUrl: './upload-data-hp-a15.component.html',
  styleUrls: ['./upload-data-hp-a15.component.scss']
})
export class UploadDataHpA15Component implements OnInit {
  @ViewChild('inputRef') inputRef: ElementRef<HTMLInputElement>;
  extensions: string = '.xls, .xlsm, .xlsx';
  title: string = '';

  constructor(
    private _spinnerService: NgxSpinnerService,
    private _service: UploadDataHpA15Service,
    private _alertifyService: AlertifyService,) {
    
  }

  ngOnInit(): void {
  }

  import(event: any) {
    this._spinnerService.show();

    if (event.target.files && event.target.files[0]) {
      const fileNameExtension = event.target.files[0].name.split('.').pop();
      if (!this.extensions.includes(fileNameExtension)) {
        return this._alertifyService.error('Please select a file excel');
      } else {
        this._spinnerService.show();
        this._service
          .uploadExcel(event.target.files[0])
          .subscribe({
            next: (res) => {
              if (res.success) {
                event.target.value = '';
                this._alertifyService.success(res.message);
              } else {
                this._alertifyService.error(res.message);
              }
              this._spinnerService.hide();
              this.inputRef.nativeElement.value = '';
            },
            error: () => {
              event.target.value = '';
              this._alertifyService.error('Oops! Sorry, an error occurred while processing your request');
            },
          })
          .add(() => this._spinnerService.hide());
      }
    }
  }
}
