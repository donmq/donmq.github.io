import { Component, OnInit } from '@angular/core';
import { ClassButton, IconButton } from '@constants/common.constants';
import { UploadResultDto } from '@models/employee-maintenance/4_1_1_employee-basic-information-maintenance';
import { S_4_1_1_EmployeeBasicInformationMaintenanceService } from '@services/employee-maintenance/s_4_1_1_employee-basic-information-maintenance.service';
import { S_4_1_2_EmployeeEmergencyContactsService } from '@services/employee-maintenance/s_4_1_2_employee-emergency-contacts.service';
import { InjectBase } from '@utilities/inject-base-app';
import { OperationResult } from '@utilities/operation-result';
import { Observable } from 'rxjs';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ModalService } from '@services/modal.service';

@Component({
  selector: 'app-upload-form-4-1-1',
  templateUrl: './upload-form.component.html',
  styleUrl: './upload-form.component.scss'
})
export class UploadFormComponent411 extends InjectBase implements OnInit {
  title: string = '';
  title2: string = '';
  programCode: string = '';
  url: string = '';
  action: string = '';
  iconButton = IconButton;
  classButton = ClassButton;
  accept = '.xls, .xlsx, .xlsm';
  dataRes: UploadResultDto = <UploadResultDto>{};

  constructor(
    public basicService: S_4_1_1_EmployeeBasicInformationMaintenanceService,
    public emergencyService: S_4_1_2_EmployeeEmergencyContactsService,
    private modalService: ModalService
  ) {
    super();
    this.programCode = this.route.snapshot.data['program'];
    this.translateService.onLangChange.pipe(takeUntilDestroyed()).subscribe(() => {
      this.title = this.functionUtility.getTitle(this.route.snapshot.data['program'])
      this.title2 = this.functionUtility.getTitle('4.1.2');
    });
  }

  ngOnInit(): void {
    this.title = this.functionUtility.getTitle(this.route.snapshot.data['program']);
    this.url = this.functionUtility.getRootUrl(this.router.routerState.snapshot.url);
    this.title2 = this.functionUtility.getTitle('4.1.2');

    this.route.data.subscribe(res =>
      this.action = `System.Action.${res.title}`
    );
  }

  downloadTemplate(service: string) {
    this.spinnerService.show();
    let downloadService: Observable<OperationResult>;
    let fileName: string;

    if (service === 'basic') {
      downloadService = this.basicService.downloadExcelTemplate();
      fileName = this.functionUtility.getFileNameExport(this.programCode, 'Template');
    } else if (service === 'emergency') {
      downloadService = this.emergencyService.downloadExcelTemplate();
      fileName = this.functionUtility.getFileNameExport('4.1.2', 'Template');
    }

    downloadService.subscribe({
      next: (result: any) => {
        this.spinnerService.hide();
        this.functionUtility.exportExcel(result.data, fileName);
      },
      error: () => this.functionUtility.snotifySystemError()
    });
  }

  upload(event: any, type: 'basic' | 'emergency') {
    if (event.target.files && event.target.files[0]) {
      this.spinnerService.show();
      const fileNameExtension = event.target.files[0].name.split('.').pop();
      if (!this.accept.includes(fileNameExtension.toLowerCase())) {
        event.target.value = '';
        this.spinnerService.hide();
        return this.snotifyService.warning(
          this.translateService.instant('System.Message.AllowExcelFile'),
          this.translateService.instant('System.Caption.Error')
        );
      }
      const formData = new FormData();
      formData.append('file', event.target.files[0]);
      let service: Observable<OperationResult> = type === 'basic'
        ? this.basicService.uploadExcel(formData)
        : this.emergencyService.uploadExcel(formData);
      service.subscribe({
        next: (res) => {
          event.target.value = '';
          this.spinnerService.hide();
          if (res.data != null) {
            this.modalService.open(<UploadResultDto>{
              total: res.data.total,
              success: res.data.success,
              error: res.data.error
            },'modal-4-1-1')
          }
          if (res.isSuccess)
            this.functionUtility.snotifySuccessError(
              res.isSuccess,
              this.translateService.instant('System.Message.UploadOKMsg'))
          else {
            if (res.data?.errorReport) {
              const fileName = type === 'basic'
                ? this.functionUtility.getFileNameExport(this.programCode, 'Report')
                : this.functionUtility.getFileNameExport('4.1.2', 'Report')
              this.functionUtility.exportExcel(res.data.errorReport, fileName);
            }

            this.functionUtility.snotifySuccessError(res.isSuccess, res.error)
          }
        },
        error: () => {
          event.target.value = '';
          this.functionUtility.snotifySystemError();
        }
      });
    }
  }

  back = () => this.router.navigate([this.url]);
}
