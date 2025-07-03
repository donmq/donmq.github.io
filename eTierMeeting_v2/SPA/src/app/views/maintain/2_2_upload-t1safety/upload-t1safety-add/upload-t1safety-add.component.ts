import { Component, OnInit, TemplateRef } from "@angular/core";
import { BsDatepickerConfig } from "ngx-bootstrap/datepicker";
import { takeUntil } from "rxjs/operators";
import { UploadT1Model, UploadVideoParam } from "@models/upload-t1-model";
import { DestroyService } from "@services/destroy.service";
import { UploadVideoT1Service } from "@services/uploadVideoT1.service";
import { ActionConstants, CaptionConstants, MessageConstants } from '@constants/system.constants';
import { BsModalRef, BsModalService } from "ngx-bootstrap/modal";
import { UnitInfo } from "@models/t1uploadVideo/unit-Info";
import { KeyValuePair } from "@utilities/key-value-pair";
import { InjectBase } from "@utilities/inject-base-app";

@Component({
  selector: "app-upload-t1safety-add",
  templateUrl: "./upload-t1safety-add.component.html",
  styleUrls: ["./upload-t1safety-add.component.scss"],
  providers: [DestroyService]
})
export class UploadT1safetyAddComponent extends InjectBase implements OnInit {
  bsConfig: Partial<BsDatepickerConfig>;
  time_start: string = "";
  time_end: string = "";
  videoKind: string= null;
  videoKindList: KeyValuePair[] = [
    { key: 'SAFETY', value: 'SAFETY' },
    { key: 'KAIZEN', value: 'KAIZEN' },
    { key: 'MPREP', value: 'MPREP' },
  ];
  optionsVideoKind = {
    placeholder: "Select video Kind...",
    allowClear: true,
    width: "100%",
  };

  centerList: KeyValuePair[];
  center: string = null;
  tierList: KeyValuePair[];
  tier: string = null;
  sectionList: KeyValuePair[];
  section: string = null;
  unitList: UnitInfo[] = [];
  optionsCenter = {
    placeholder: "Select Center...",
    allowClear: true,
    width: "100%"
  };

  optionsTier = {
    placeholder: "Select Tier...",
    allowClear: true,
    width: "100%"
  };
  optionsSections = {
    placeholder: "Select Sections...",
    allowClear: true,
    width: "100%"
  };

  showBodyInformationUpload: boolean = false;
  cell: string;
  title_ENG: string;
  title_LCL: string;
  title_CHT: string;
  remark: string;
  video: any;
  iconVideo: any;
  urlIcon: any;

  dataUpload: UploadT1Model[] = [];
  maxSize = 31457280;

  modalRef: BsModalRef | null;
  showAllUnit: boolean = true;
  unitNameCheck: string = '';
  unitsCheck: UnitInfo[] = [];
  uploadVideoParam: UploadVideoParam = <UploadVideoParam>{};
  constructor(
    private uploadVideoT1Service: UploadVideoT1Service,
    private modalService: BsModalService
  ) {
    super();
  }

  ngOnInit(): void {
    this.videoKind = this.videoKindList[0].key;
    this.getCenters();
    this.getTiers();
    this.getSections();
  }

  //====================================================Get more List Data====================================================//
  changedVideoKind(e: any): void {
    this.videoKind = e;
  }
  getCenters() {
    this.uploadVideoT1Service.getCenters().subscribe(res => {
      this.centerList = res.map(item => {
        return {key: item, value: item};
      });
    }, error => {
      this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR);
    });
  }
  changedCenter(e: any) {
    this.center = e;
    this.getUnits();
  }
  getTiers() {
    this.uploadVideoT1Service.getTiers().subscribe(res => {
      this.tierList = res.map(item => {
        return {key: item, value: item};
      });
    }, error => {
      this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR);
    });
  }
  changedTier(e: any) {
    this.tier = e;
    this.getUnits();
  }
  getSections() {
    this.uploadVideoT1Service.getSections().subscribe(res => {
      this.sectionList = res.map(item => {
        return {key: item, value: item};
      });
    }, error => {
      this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR);
    });
  }
  changedSection(e: any) {
    this.section = e;
  }
  getUnits() {
    if(this.checkEnableChooseUnit()) {
      this.uploadVideoT1Service.getUnitsInParam(this.center, this.tier, this.section).subscribe(res => {
        this.unitList = res.map(item => {
          item.isCheck = true;
          return item;
        });
        
      }, error => {
        this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR);
      });
    }
  }
  checkEnableChooseUnit(): boolean {
    if(this.functionUtility.checkEmpty(this.center) || this.functionUtility.checkEmpty(this.tier) || this.functionUtility.checkEmpty(this.section)) {
      return false;
    }
    return true;
  }
  
  // ===================================================================== Start Modal Unit=====================================================================//
  // Mở modal Unit
  openModalChooseUnit(template: TemplateRef<any>) {
    this.showAllUnit = true;
    this.modalRef = this.modalService.show(template, { class: "modal-fluid" });
    this.getUnits();
  }
  clickCheckAllUnit() {
    this.unitList = this.unitList.map(item => {
      item.isCheck = this.showAllUnit;
      return item;
    })
  }
  clickCheckItemUnit(model: UnitInfo) {
    if(!model.isCheck) {
      this.showAllUnit = false;
    } else {
      this.showAllUnit = this.unitList.every(x => x.isCheck);
    }
  }
  confirmModalUnit() {
    let checkUnit = this.unitList.filter(x => x.isCheck);
    if(checkUnit.length === 0) {
      this.snotifyService.error("Please check unit!", CaptionConstants.ERROR);
    } else {
      this.unitsCheck = checkUnit;
      this.unitNameCheck = checkUnit.map(x => x.value).join(',');
      this.snotifyService.success('Choose Unit successfully!', CaptionConstants.SUCCESS)
      this.modalRef.hide();
    }
  }
  // =====================================================================End Modal Unit=====================================================================//

  chooseVideo(event: any) {
    if (event.target.files[0]?.name.includes("mp4")) {
      let fileZise = event.target.files[0].size;
      if (fileZise <= this.maxSize) {
        const reader = new FileReader();
        reader.readAsDataURL(event.target.files[0]); // read file as data url
        reader.onload = (e) => {
          this.video = e.target.result.toString();
        };
      } else {
        (<HTMLInputElement>document.getElementById('upload_File_video')).value = '';
        this.snotifyService.error(MessageConstants.FILE_VIDEO_SIZE, CaptionConstants.ERROR);
      }

    }
  }
  chooseIcon(event: any) {
    let files = ["jpg", "JPG", "jpeg", "JPEG", "png", "PNG"];
    const fileNameExtension = event.target.files[0].name.split(".").pop();
    if (files.includes(fileNameExtension)) {
      let fileZise = event.target.files[0].size;
      if (fileZise <= this.maxSize) {
        const reader = new FileReader();
        reader.readAsDataURL(event.target.files[0]); // read file as data url
        reader.onload = (e) => {
          this.iconVideo = e.target.result.toString();
          this.urlIcon = e.target.result.toString();
        };
      } else {
        (<HTMLInputElement>document.getElementById('upload_File_icon')).value = '';
        this.snotifyService.error(MessageConstants.FILE_IMAGE_SIZE, CaptionConstants.ERROR);
      }
    }
  }

  //------------------------- Show content để nhập dữ liệu,upload video + icon-------------------------------//
  validate(): string[] {
    if (this.functionUtility.checkEmpty(this.videoKind)) {
      this.snotifyService.error('Please option Video Kind!', CaptionConstants.ERROR);
      return;
    }
    if (this.functionUtility.checkEmpty(this.unitNameCheck)) {
      this.snotifyService.error('Please option Unit!', CaptionConstants.ERROR);
      return;
    }
    if (this.functionUtility.checkEmpty(this.time_start) || this.functionUtility.checkEmpty(this.time_end)) {
      this.snotifyService.error(MessageConstants.SELECT_DATE, CaptionConstants.ERROR);
      return;
    }
    let from_Date = this.functionUtility.getDateFormat(new Date(this.time_start));
    let to_Date = this.functionUtility.getDateFormat(new Date(this.time_end));
    if (from_Date > to_Date) {
      this.snotifyService.error(MessageConstants.COMPARE_DATE, CaptionConstants.ERROR);
    } else {
      return [from_Date, to_Date];
    }
  }
  showBodyUpload() {
    let checkValidate = this.validate();
    if(checkValidate.length === 2) {
      this.showBodyInformationUpload = true;
    }
  }

  //--------------------------------Add Data vào table dưới cùng --------------------------------------------//
  addDataTable() {
    let checkValidate = this.validate();
    if(checkValidate.length === 2) {
      if (this.video === undefined) {
        return this.snotifyService.error('Please upload the video in mp4 format!', CaptionConstants.ERROR);
      }
  
      if (this.functionUtility.checkEmpty(this.title_ENG)) {
        return this.snotifyService.error('Please enter title ENG!', CaptionConstants.ERROR);
      }
  
      if (this.functionUtility.checkEmpty(this.title_LCL)) {
        return this.snotifyService.error('Please enter title LCL!', CaptionConstants.ERROR);
      }
      if (this.functionUtility.checkEmpty(this.title_CHT)) {
        return this.snotifyService.error('Please enter title CHT!', CaptionConstants.ERROR);
      }
      if (this.iconVideo === undefined) {
        return this.snotifyService.error('Please option icon!', CaptionConstants.ERROR);
      }
      if (this.functionUtility.checkEmpty(this.remark)) {
        return this.snotifyService.error('Please enter Remark!', CaptionConstants.ERROR);
      }
  
      this.unitsCheck.forEach((unitItem, i) => {
        let dataItem: UploadT1Model = <UploadT1Model> {
          unitId: unitItem.key,
          unitName: unitItem.value,
          video_Kind: this.videoKind,
          video_Title_ENG: this.title_ENG,
          vIdeo_Title_LCL: this.title_LCL,
          video_Title_CHT: this.title_CHT,
          urllIcon: this.urlIcon,
          video_Remark: this.remark,
        };
        this.dataUpload.push(dataItem);
      });
  
      this.uploadVideoParam.unitIds = this.unitsCheck.map(x => x.key);
      this.uploadVideoParam.video_Kind = this.videoKind,
        this.uploadVideoParam.video_Title_ENG = this.title_ENG,
        this.uploadVideoParam.vIdeo_Title_LCL = this.title_LCL,
        this.uploadVideoParam.video_Title_CHT = this.title_CHT,
        this.uploadVideoParam.video = this.video,
        this.uploadVideoParam.video_Icon = this.iconVideo,
        this.uploadVideoParam.video_Remark = this.remark,
        this.uploadVideoParam.to_Date = checkValidate[1],
        this.uploadVideoParam.from_Date = checkValidate[0]
      this.showBodyInformationUpload = false;
      this.clearDataForm();
    }
  }

  //---------------------------------------Xóa Item của bảng-------------------------------------------//
  deleteItemtable(i: number) {
    this.snotifyService.confirm(MessageConstants.CONFIRM_DELETE, ActionConstants.DELETE, () => {
      this.dataUpload.splice(i, 1);
    });
  }

  //--------------------------------------- Add data in database ---------------------------------------//
  uploadVideo() {
    this.spinnerService.show();
    this.uploadVideoT1Service.uploadVideo(this.uploadVideoParam).pipe(takeUntil(this.destroyService.destroyed$)).subscribe(res => {
      if (res) {
        this.snotifyService.success(MessageConstants.UPLOAD_OK_MSG, CaptionConstants.SUCCESS);
        this.dataUpload.length = 0;
        this.clearHeaderContent();
        this.spinnerService.hide();
      } else {
        this.spinnerService.hide();
        this.snotifyService.error(MessageConstants.UPDATED_ERROR_MSG, CaptionConstants.ERROR);
      }
    }, error => {
      this.snotifyService.error(MessageConstants.UPDATED_ERROR_MSG, CaptionConstants.ERROR);
    })

  }

  clearHeaderContent() {
    this.videoKind = '';
    this.time_start = '';
    this.time_end = '';
  }
  clearDataForm() {
    this.title_ENG = '';
    this.title_LCL = '';
    this.title_CHT = '';
    this.remark = '';
    this.iconVideo = undefined;
    this.video = undefined;
    this.urlIcon = undefined;
  }
}
