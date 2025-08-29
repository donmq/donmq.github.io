import {
  AfterViewInit,
  Component,
  ElementRef,
  input,
  OnDestroy,
  ViewChild
} from '@angular/core';
import { ClassButton, IconButton } from '@constants/common.constants';
import {
  EmployeeRewardAndPenaltyRecords_ModalInputModel,
  EmployeeRewardPenaltyRecordsReportDownloadFileModel,
  EmployeeRewardPenaltyRecordsReportFileModel
} from '@models/reward-and-penalty-maintenance/8_1_2_employee-reward-and-penalty-records';
import { ModalService } from '@services/modal.service';
import { S_8_1_2_EmployeeRewardAndPenaltyRecordsService } from '@services/reward-and-penalty-maintenance/s_8_1_2_employee-reward-and-penalty-records.service';
import { InjectBase } from '@utilities/inject-base-app';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrl: './modal.component.scss'
})
export class ModalComponent extends InjectBase implements AfterViewInit, OnDestroy {
  @ViewChild('modal', { static: false }) directive?: ModalDirective;
  @ViewChild('inputRef') inputRef: ElementRef<HTMLInputElement>;
  id = input<string>(this.modalService.defaultModal);
  iconButton = IconButton;
  classButton = ClassButton;
  action: any;
  filesPassingSubscribe: Subscription = null;
  data: EmployeeRewardAndPenaltyRecords_ModalInputModel = <EmployeeRewardAndPenaltyRecords_ModalInputModel>{ file_List: [] }
  isSave: boolean = false;
  constructor(
    private service: S_8_1_2_EmployeeRewardAndPenaltyRecordsService,
    private modalService: ModalService
  ) {
    super();
  }
  ngOnDestroy(): void {
    this.modalService.remove(this.id());
  }
  ngAfterViewInit(): void { this.modalService.add(this); }
  open(data: EmployeeRewardAndPenaltyRecords_ModalInputModel): void {
    console.log('Before open, paramSearch:', this.service.paramSearch());
    this.data = structuredClone(data);
    this.isSave = false
    this.directive.show()
  }
  onHide = () => this.modalService.onHide.emit({ isSave: this.isSave, data: this.data })
  upload(event: any) {
    const files = Array.from(event.target.files as File[])
    event.target.value = ''
    const errorFiles = files.filter(file => file.size > 2097152).map(x => x.name)
    if (errorFiles.length > 0)
      return this.snotifyService.error(
        `${this.translateService.instant('File size must be 2MB or smaller')} : \n${errorFiles.join('\n')}`,
        this.translateService.instant('System.Caption.Error')
      );

    const fileNames = this.data.file_List.map(x => x.name).concat(files.map(x => x.name));

    const lookup = fileNames.reduce((a, e) => {
      a[e] = ++a[e] || 0;
      return a;
    }, {});
    const duplicateFiles = [...new Set(fileNames.filter(e => lookup[e]))]
    if (duplicateFiles.length > 0)
      return this.snotifyService.error(
        `${this.translateService.instant('System.Message.ExistedFile')} : \n${duplicateFiles.join('\n')}`,
        this.translateService.instant('System.Caption.Error')
      );
    files.forEach(file => {
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => {
        const newFile: EmployeeRewardPenaltyRecordsReportFileModel = {
          id: 0,
          name: file.name,
          size: file.size,
          content: reader.result as string
        };
        this.data.file_List.push(newFile);
        this.calculateId()
      }
    })
  }
  calculateId() {
    this.data.file_List.map((val, ind) => {
      val.id = ind + 1
    })
  }
  remove(item: EmployeeRewardPenaltyRecordsReportFileModel) {
    this.data.file_List = this.data.file_List.filter(x => x.name != item.name)
    this.calculateId()
  }
  save() {
    this.isSave = true
    this.directive.hide();
  }
  download(item: EmployeeRewardPenaltyRecordsReportFileModel) {
    if (item.content && item.content.includes('base64')) {
      var link = document.createElement('a');
      document.body.appendChild(link);
      link.setAttribute("href", item.content);
      link.setAttribute("download", item.name);
      link.click();
    } else {
      this.spinnerService.show();
      let data: EmployeeRewardPenaltyRecordsReportDownloadFileModel = <EmployeeRewardPenaltyRecordsReportDownloadFileModel>{
        division: this.data.division,
        factory: this.data.factory,
        serNum: this.data.serNum,
        employee_ID: this.data.employee_ID,
        file_Name: item.name
      }
      this.service.downloadFile(data)
        .subscribe({
          next: (res) => {
            this.spinnerService.hide();
            if (res.isSuccess) {
              var link = document.createElement('a');
              document.body.appendChild(link);
              link.setAttribute("href", res.data.content);
              link.setAttribute("download", res.data.name);
              link.click();
            }
            else this.functionUtility.snotifySuccessError(false, `RewardandPenaltyMaintenance.RewardAndPenaltyRecords.${res.error}`)
          },
          error: () => this.functionUtility.snotifySystemError()
        });
    }
  }
  close() {
    this.directive.hide()
  }
}
