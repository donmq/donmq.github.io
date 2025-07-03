import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { T2MeetingTimeSettingService } from '@services/t2-meeting-time-setting.service';
import { CaptionConstants, MessageConstants } from '@constants/system.constants';
import { BsDaterangepickerConfig } from 'ngx-bootstrap/datepicker';
import { eTM_T2_Meeting_Seeting } from '@models/eTM_T2_Meeting_Seeting';
import { OperationResult } from '@utilities/operation-result';
import { KeyValuePair } from '@utilities/key-value-pair';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-add',
  templateUrl: './add.component.html',
  styleUrls: ['./add.component.scss']
})
export class AddComponent extends InjectBase implements OnInit {
  @Output() evenModal = new EventEmitter<OperationResult>();
  listBuildingOrGroup: KeyValuePair[] = [];
  bsConfig: BsDaterangepickerConfig = <BsDaterangepickerConfig>{
    isAnimated: true,
    dateInputFormat: 'YYYY/MM/DD',
  };
  newItem: eTM_T2_Meeting_Seeting = <eTM_T2_Meeting_Seeting>{
    tU_ID: null
  };
  meetingDate: Date = null;
  startTime: Date = null;
  endTime: Date = null;
  constructor(private t2MeetingTimeSettingService: T2MeetingTimeSettingService,
    public bsModalRef: BsModalRef) {
    super();
  }

  ngOnInit() {
    this.getListBuildingOrGroup();
  }

  invalidTime(): boolean { 
    if(this.startTime.getHours() < this.endTime.getHours()
      || (this.startTime.getHours() == this.endTime.getHours() && this.startTime.getMinutes() < this.endTime.getMinutes()))
      return false;
    return true;
  }

  saveForm() {
    if(this.invalidTime()) { 
      return this.snotifyService.error("Invalid time !", CaptionConstants.ERROR);
    }
    this.newItem.meeting_Date = this.meetingDate != null ? this.functionUtility.getDateFormat(this.meetingDate) : '';
    this.newItem.start_Time = this.startTime != null ? this.startTime.toDate().toStringTime() : '';
    this.newItem.end_Time = this.endTime != null ? this.endTime.toDate().toStringTime() : '';
    this.spinnerService.show(); 
    this.t2MeetingTimeSettingService.add(this.newItem).subscribe({ 
      next: res => { 
        if(res.success) { 
          this.snotifyService.success(MessageConstants.CREATED_OK_MSG, CaptionConstants.SUCCESS);
          this.closeModal(res.success);
        }
        else { 
          if(!res.success && res.message == null) {
            this.snotifyService.error(MessageConstants.CREATED_ERROR_MSG, CaptionConstants.ERROR); 
          }else { 
            this.snotifyService.error(res.message, CaptionConstants.ERROR);
          }
        }
        this.spinnerService.hide();
      },
      error: () => { 
        this.spinnerService.hide();
        this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR);
      }
    })
  }

  closeModal(isFlat: boolean) {
    this.evenModal.emit(<OperationResult>{ caption: '', data: '', message: '', success: isFlat });
    this.bsModalRef.hide();
  }

  resetForm() {
    this.newItem = <eTM_T2_Meeting_Seeting>{
      tU_ID: '',
      meeting_Date: '',
      end_Time: '',
      start_Time: ''
    };
    this.startTime = null;
    this.endTime = null;
    this.meetingDate = null;

  }


  getListBuildingOrGroup() {
    this.spinnerService.show();
    this.t2MeetingTimeSettingService.getListBuildingOrGroup().subscribe({
      next: res => {
        this.listBuildingOrGroup = res;
        this.spinnerService.hide();
      },
      error: () => {
        this.spinnerService.hide();
        this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR);
      },
    })
  }

}
