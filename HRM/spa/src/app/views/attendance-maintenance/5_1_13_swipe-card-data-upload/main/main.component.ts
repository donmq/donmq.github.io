import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { IconButton } from '@constants/common.constants';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { HRMS_Att_Swipe_Card_Upload } from '@models/attendance-maintenance/5_1_13_swipe-card-data-upload';
import { S_5_1_13_SwipeCardDataUploadService } from '@services/attendance-maintenance/s_5_1_13_swipe-card-data-upload.service';
import { InjectBase } from '@utilities/inject-base-app';
import { KeyValuePair } from '@utilities/key-value-pair';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrl: './main.component.scss'
})
export class MainComponent extends InjectBase implements OnInit {
  @ViewChild('fileInput') fileInput!: ElementRef;
  currentUser = JSON.parse(localStorage.getItem(LocalStorageConstants.USER));

  //#region Data
  factories: KeyValuePair[] = [];
  //#endregion

  //#region object
  param: HRMS_Att_Swipe_Card_Upload = <HRMS_Att_Swipe_Card_Upload>{
    factory: '',
    fileUpload: null
  }
  //#endregion

  //#region Vaiables
  accept: string = '.txt';
  title: string = '';
  processedRecords: number = null;
  iconButton = IconButton;
  //#endregion

  constructor(private _services: S_5_1_13_SwipeCardDataUploadService) {
    super();
    // Load lại dữ liệu khi thay đổi ngôn ngữ
    this.translateService.onLangChange.pipe(takeUntilDestroyed()).subscribe(res => {
      this.title = this.functionUtility.getTitle(this.route.snapshot.data['program'])
      this.getFactories();
    });
  }

  ngOnInit(): void {
    this.title = this.functionUtility.getTitle(this.route.snapshot.data['program'])
    this.getFactories();
  }

  //#region Methods
  getFactories() {
    this.spinnerService.show();
    this._services.getFactories().subscribe({
      next: result => {
        this.spinnerService.hide();
        this.factories = result;
      },
      error: () => this.functionUtility.snotifySystemError(),
      complete: () => this.spinnerService.hide()
    })
  }
  //#endregion
  //#endregion

  //#region Events
  onSelectFile(event: any) {
    if (this.functionUtility.checkEmpty(this.param.factory)) {
      event.target.value = '';
      this.snotifyService.warning(`Please choose ${this.translateService.instant('AttendanceMaintenance.SwipeCardDataUpload.Factory')}`, this.translateService.instant('System.Caption.Warning'))
    }
    else {
      this.snotifyService.confirm(
        this.translateService.instant('AttendanceMaintenance.SwipeCardDataUpload.ConfirmExecution'),
        this.translateService.instant('System.Action.Confirm'),
        () => {
          if (event.target.files && event.target.files[0]) {
            this.spinnerService.show();
            const fileFormat = event.target.files[0].name.split('.').pop();
            if (!this.accept.includes(fileFormat)) {
              event.target.value = '';
              this.spinnerService.hide();
              this.snotifyService.error(
                this.translateService.instant('System.Message.InvalidFile'),
                this.translateService.instant('System.Caption.Error')
              );
            } else {
              this.param.fileUpload = event.target.files[0]
              this._services.excute(this.param).subscribe({
                next: result => {
                  event.target.value = '';
                  this.spinnerService.hide();
                  if (result.isSuccess) {
                    this.functionUtility.snotifySuccessError(result.isSuccess, this.translateService.instant('System.Message.UploadOKMsg'))
                    this.processedRecords = result.data;
                  }
                  else {
                    this.processedRecords = null;
                    if (result.data != null) {
                      const fileName = this.functionUtility.getFileName('SwipeCardDataReport_Report')
                      this.functionUtility.exportExcel(result.data, fileName);
                    }
                    else this.functionUtility.snotifySuccessError(result.isSuccess, result.error)
                  }
                },
                error: () => {
                  event.target.value = '';
                  this.functionUtility.snotifySystemError()
                }
              })
            }
          }
        },
        () => { event.target.value = ''; }
      )
    }
  }
  //#endregion
}
