import { Component, OnInit, TemplateRef } from "@angular/core";
import { BsDatepickerConfig } from "ngx-bootstrap/datepicker";
import { BsModalRef, BsModalService } from "ngx-bootstrap/modal";
import { takeUntil } from "rxjs/operators";
import { environment } from "@environments/environment";
import { ActionConstants, CaptionConstants, MessageConstants } from "@constants/system.constants";
import { PaginatedResult, Pagination } from "@models/pagination";
import { eTM_Video } from "@models/t1uploadVideo/eTM_Video";
import { batchDeleteParam } from "@models/t1uploadVideo/eTM_Video_Param";
import { UnitInfo } from "@models/t1uploadVideo/unit-Info";
import { UploadVideoT1Service } from "@services/uploadVideoT1.service";
import { KeyValuePair } from "@utilities/key-value-pair";
import { InjectBase } from "@utilities/inject-base-app";

@Component({
  selector: "app-batch-delete",
  templateUrl: "./batch-delete.component.html",
  styleUrls: ["./batch-delete.component.scss"],
})
export class BatchDeleteComponent extends InjectBase implements OnInit {
  videoKind: string = null;
  videoKindList: KeyValuePair[];
  centerList: KeyValuePair[];
  center: string = null;
  tierList: KeyValuePair[];
  tier: string = null;
  sectionList: KeyValuePair[];
  section: string = null;
  unitList: UnitInfo[] = [];
  unit: string = null;
  optionsCenter ={
    placeholder: "Select Center...",
    allowClear: true,
    width: "100%"
  };

  optionsVideoKind ={
    placeholder: "Select video Kind...",
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

  time_start: string = "";
  time_end: string = "";
  bsConfig: Partial<BsDatepickerConfig>;
  pagination: Pagination;

  dataTmVideos: eTM_Video[] = [];
  dataTmVideoAll: eTM_Video[] = [];
  url: any = environment.baseUrl;
  searchParam: any;

  modalRef: BsModalRef | null;
  showAllUnit: boolean = true;
  unitNameCheck: string = '';
  unitListCheck: string[] = [];
  constructor(
    private uploadVideoT1Service: UploadVideoT1Service,
    private modalService: BsModalService
  ) {
    super();
  }

  ngOnInit() {
    this.pagination = {
      currentPage: 1,
      itemsPerPage: 50,
      totalItems: 0,
      totalPages: 0
    }
    this.getListVideoKind();
    this.getCenters();
    this.getTiers();
    this.getSections();
    this.getUnits();
  }
// ====================================================Get more List Data====================================================//
  getListVideoKind() {
    this.uploadVideoT1Service
      .getListVideoKind()
      .pipe(takeUntil(this.destroyService.destroyed$))
      .subscribe((res) => {
        this.videoKindList = res.map((item) => {
          return { key: item, value: item };
        });
      });
  }
  getCenters() {
    this.uploadVideoT1Service.getCenters().subscribe(res => {
      this.centerList = res.map(item => {
        return {key: item, value: item};
      });
    });
  }
  changedCenter(e: any) {
    this.center = e;
  }
  getTiers() {
    this.uploadVideoT1Service.getTiers().subscribe(res => {
      this.tierList = res.map(item => {
        return {key: item, value: item};
      });
    });
  }
  changedTier(e: any) {
    this.tier = e;
  }
  getSections() {
    this.uploadVideoT1Service.getSections().subscribe(res => {
      this.sectionList = res.map(item => {
        return {key: item, value: item};
      });
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
    let checkUnit = this.unitList.some(x => x.isCheck);
    if(!checkUnit) {
      this.snotifyService.error("Please check unit!", CaptionConstants.ERROR);
    } else {
      let unitsCheck = this.unitList.filter(x => x.isCheck);
      this.unitListCheck = unitsCheck.map(x => x.key);
      this.unitNameCheck = unitsCheck.map(x => x.value).join(',');
      this.snotifyService.success('Choose Unit successfully!', CaptionConstants.SUCCESS)
      this.modalRef.hide();
    }
  }
  // =====================================================================End Modal Unit=====================================================================//
  
  // ===============================================================*********===============================================================//

  getDataTable() {
    if(this.functionUtility.checkEmpty(this.videoKind)) {
      return this.snotifyService.error('Please option Video Kind!', CaptionConstants.ERROR);
    }
    if(this.functionUtility.checkEmpty(this.unitNameCheck)) {
      return this.snotifyService.error('Please option Dept/Cell and Confirm!', CaptionConstants.ERROR);
    }
    if(this.functionUtility.checkEmpty(this.time_start) || this.functionUtility.checkEmpty(this.time_end)) {
      return this.snotifyService.error(MessageConstants.SELECT_DATE, CaptionConstants.ERROR);
    }

    let from_Date = this.functionUtility.getDateFormat(new Date(this.time_start));
    let to_Date = this.functionUtility.getDateFormat(new Date(this.time_end));
    if(from_Date > to_Date) {
      return this.snotifyService.error(MessageConstants.COMPARE_DATE, CaptionConstants.ERROR);
    }

    this.spinnerService.show();
    let param: batchDeleteParam = {
      videoKind: this.videoKind,
      units: this.unitListCheck,
      center: this.center,
      tier: this.tier,
      section: this.section,
      from_Date: from_Date,
      to_Date : to_Date,
    }
     
    this.uploadVideoT1Service.searchOfUser(this.pagination.currentPage, this.pagination.itemsPerPage, param).pipe(takeUntil(this.destroyService.destroyed$))
    .subscribe((res: PaginatedResult<eTM_Video[]>) => {
      this.dataTmVideoAll = res.result;
      this.dataTmVideos = this.dataTmVideoAll.slice((this.pagination.currentPage - 1) * this.pagination.itemsPerPage, this.pagination.itemsPerPage * this.pagination.currentPage);
      this.pagination = res.pagination;  
      if(this.dataTmVideoAll.length === 0) {
        this.snotifyService.error(MessageConstants.NO_DATA);
      }

      this.searchParam = param;
      this.spinnerService.hide();
    }, error => {
      this.snotifyService.error(MessageConstants.QUERY_ERROR, CaptionConstants.ERROR);
      this.spinnerService.hide();
    });
  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.dataTmVideos = this.dataTmVideoAll.slice((this.pagination.currentPage - 1) * this.pagination.itemsPerPage, this.pagination.itemsPerPage * this.pagination.currentPage);
  }

  search() {
    this.pagination.currentPage = 1;
    this.getDataTable();
  }

  deleteAll() {
    if(this.searchParam !== undefined) {
      if(this.dataTmVideoAll.length === 0) {
        this.snotifyService.error('No data of delete', CaptionConstants.ERROR);
      } else {
        this.snotifyService.confirm(MessageConstants.CONFIRM_DELETE,ActionConstants.DELETE, () => {
          this.spinnerService.show();
          this.uploadVideoT1Service.deleteAllBySearch(this.dataTmVideoAll).subscribe(res => {
            if(res) {
              this.snotifyService.success(MessageConstants.DELETED_OK_MSG, CaptionConstants.SUCCESS);
              this.clearHeaderContent();
              this.spinnerService.hide();
            }
          }, error => {
            this.snotifyService.error(MessageConstants.DELETED_ERROR_MSG, CaptionConstants.ERROR);
              this.spinnerService.hide();
          });
        });
      }
    } else {
      this.snotifyService.error(MessageConstants.QUERY_SEARCH, CaptionConstants.ERROR);
    }
  }

  changedVideoKind(e: any): void {
    this.videoKind = e;
  }

  clearHeaderContent() {
    [this.videoKind,this.time_start,this.time_end] = ['','',''];
    this.searchParam = undefined;
    this.dataTmVideos.length = 0;
    this.pagination.currentPage = 1;
  }
}
