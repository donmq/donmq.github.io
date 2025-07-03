import { Component, OnInit } from '@angular/core';
import { T2MeetingTimeSettingService } from '@services/t2-meeting-time-setting.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CaptionConstants, MessageConstants } from '@constants/system.constants';
import { T2MeetingTimeSettingParam } from '../../../../_core/_helpers/params/t2-meeting-time-setting.param';
import { BsDaterangepickerConfig } from 'ngx-bootstrap/datepicker';
import { Pagination } from '@utilities/pagination-helper';
import { eTM_T2_Meeting_Seeting } from '@models/eTM_T2_Meeting_Seeting';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';
import { AddComponent } from '../add/add.component';
import { OperationResult } from '@utilities/operation-result';
import { KeyValuePair } from '@utilities/key-value-pair';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent extends InjectBase implements OnInit {
  listBuildingOrGroup: KeyValuePair[] = [];
  param: T2MeetingTimeSettingParam = <T2MeetingTimeSettingParam>{
    start_Date: '',
    end_Date: '',
    building_Or_Group: ''
  }
  startDate: Date = null;
  endDate: Date = null;
  bsConfig: BsDaterangepickerConfig = <BsDaterangepickerConfig>{
    isAnimated: true,
    dateInputFormat: 'YYYY/MM/DD',
  };
  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 10
  };
  data: eTM_T2_Meeting_Seeting[] = [];
  modalRef?: BsModalRef;
  constructor(
    private t2MeetingTimeSettingService: T2MeetingTimeSettingService,
    private modalService: BsModalService) {
    super();
  }

  ngOnInit() {
    this.getListBuildingOrGroup();
    this.getAllData();
  }

  getAllData() {
    this.param.start_Date = this.startDate != null ? this.functionUtility.getDateFormat(this.startDate) : "";
    this.param.end_Date = this.endDate != null ? this.functionUtility.getDateFormat(this.endDate) : "";
    this.spinnerService.show();
    this.t2MeetingTimeSettingService.getAllData(this.pagination, this.param).subscribe({
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

  openAddModal() {
    this.modalRef = this.modalService.show(AddComponent); 
    this.modalRef.content.evenModal.subscribe((e: OperationResult) => { 
      if(e.success) { 
        this.getAllData();
      }
    });
  }

  validateSearch(): boolean { 
    let valid: boolean = true; 
    if(
      (this.startDate != null && this.endDate == null) || 
      (this.startDate == null && this.endDate != null) ||
      (this.startDate == null && this.endDate == null)
    ) { 
      valid = false; 
      this.snotifyService.error("Please choose Start date and End date to search!", CaptionConstants.ERROR);
    }
    return valid;
  }

  search() {
    if(this.validateSearch()) { 
      this.pagination.pageNumber === 1 ? this.getAllData() : this.pagination.pageNumber = 1;
    }
  }

  clearForm() {
    this.param = <T2MeetingTimeSettingParam>{
      start_Date: '',
      end_Date: '',
      building_Or_Group: ''
    };
    this.startDate = null; 
    this.endDate = null;
    this.pagination.pageNumber = 1; 
    this.getAllData();
  }

  onDelete(item: eTM_T2_Meeting_Seeting) {
    this.snotifyService.confirm(MessageConstants.CONFIRM_DELETE_MSG, "Confirm!", async () => { 
      this.spinnerService.show();
      this.t2MeetingTimeSettingService.delete(item).subscribe({
        next: res => {
          if (res.success) {
            this.snotifyService.success(MessageConstants.DELETED_OK_MSG, CaptionConstants.SUCCESS);
            this.getAllData();
          } else {
            this.snotifyService.error(MessageConstants.DELETED_ERROR_MSG, CaptionConstants.ERROR);
          }
          this.spinnerService.hide();
        },
        error: () => {
          this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR);
          this.spinnerService.hide();
        }
      });
    });
  }

  pageChanged(e: PageChangedEvent) {
    this.pagination.pageNumber = e.page;
    this.getAllData();
  }

}
