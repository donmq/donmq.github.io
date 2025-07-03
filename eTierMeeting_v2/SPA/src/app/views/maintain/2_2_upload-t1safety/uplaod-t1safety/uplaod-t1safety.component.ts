import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
 
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { takeUntil } from 'rxjs/operators';
import { environment } from '@environments/environment';
import { ActionConstants, CaptionConstants, MessageConstants } from '@constants/system.constants';
import { PaginatedResult, Pagination } from '@models/pagination';
import { eTMVideoParam } from '@models/t1uploadVideo/eTM_Video_Param';
import { TMVideoDto } from '@models/t1uploadVideo/tm-video-dto';
import { DestroyService } from '@services/destroy.service';
import { UploadVideoT1Service } from '@services/uploadVideoT1.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-uplaod-t1safety',
  templateUrl: './uplaod-t1safety.component.html',
  styleUrls: ['./uplaod-t1safety.component.scss'],
  providers: [DestroyService]
})
export class UplaodT1safetyComponent extends InjectBase implements OnInit {
  bsConfig: Partial<BsDatepickerConfig>;
  productT1List: TMVideoDto[] = [];
  time_start: string = '';
  time_end: string = '';

  videoKind: string=null;
  videoKindList: KeyValuePair[];
  centerList: KeyValuePair[];
  center: string = null;
  tierList: KeyValuePair[];
  tier: string = null;
  sectionList: KeyValuePair[];
  section: string = null;
  unitList: KeyValuePair[];
  unit: string = null;

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
  optionsUnit = {
    placeholder: "Select Unit...",
    allowClear: true,
    width: "100%"
  };
  url: any = environment.baseUrl;
  pagination: Pagination;
  constructor(private uploadVideoT1Service: UploadVideoT1Service) {
    super();
  }

  ngOnInit(): void {
    this.pagination = {
      currentPage: 1,
      itemsPerPage: 10,
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
    this.uploadVideoT1Service.getListVideoKind().pipe(takeUntil(this.destroyService.destroyed$)).subscribe(res => {
      this.videoKindList = res.map(item => {
        return {key: item, value: item}
      })
      this.videoKindList.unshift({key: 'All', value: 'All'})
    });
  }
  changedVideoKind(e: any): void {
    this.videoKind = e;
  }
  getCenters() {
    this.uploadVideoT1Service.getCenters().subscribe(res => {
      this.centerList = res.map(item => {
        return {key: item, value: item};
      });
      this.centerList.unshift({key: 'All', value: 'All'});
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
      this.tierList.unshift({key: 'All', value: 'All'});
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
      this.sectionList.unshift({key: 'All', value: 'All'});
    });
  }
  changedSection(e: any) {
    this.section = e;
  }
  getUnits() {
    this.uploadVideoT1Service.getUnits().subscribe(res => {
      this.unitList = res.map(item => {
        return {key: item.key, value: item.value};
      });
      this.unitList.unshift({key: 'All', value: 'All'});
    });
  }
  changedUnit(e: any) {
    this.unit = e;
  }

  // ===============================================================*********===============================================================//

  getDataTable() {
    this.spinnerService.show();
    let param: eTMVideoParam = {
      videoKind: this.videoKind,
      center: this.center,
      tier: this.tier,
      section: this.section,
      unit: this.unit,
      from_Date: this.functionUtility.checkEmpty(this.time_start) ? '' : this.functionUtility.getDateFormat(new Date(this.time_start)),
      to_Date : this.functionUtility.checkEmpty(this.time_end) ? '' : this.functionUtility.getDateFormat(new Date(this.time_end)),
    }

    this.uploadVideoT1Service.search(this.pagination.currentPage, this.pagination.itemsPerPage, param).pipe(takeUntil(this.destroyService.destroyed$))
    .subscribe((res: PaginatedResult<TMVideoDto[]>) => {
      this.productT1List = res.result;
      this.pagination = res.pagination;
      if(this.productT1List.length === 0) {
        this.snotifyService.error(MessageConstants.NO_DATA);
      }
      this.spinnerService.hide();
    }, error => {
      this.snotifyService.error(MessageConstants.QUERY_ERROR, CaptionConstants.ERROR);
      this.spinnerService.hide();
    });
  }
  search() {
    this.pagination.currentPage = 1;
    this.getDataTable();

  }
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.getDataTable();
  }
  delete(model: TMVideoDto) {
    this.snotifyService.confirm(MessageConstants.CONFIRM_DELETE,ActionConstants.DELETE, () => {
      this.uploadVideoT1Service.delete(model).subscribe(res => {
        if(res) {
          this.snotifyService.success(MessageConstants.DELETED_OK_MSG, CaptionConstants.SUCCESS);
          this.search();
        } else {
          this.snotifyService.error(MessageConstants.DELETED_ERROR_MSG, CaptionConstants.ERROR);
        }
      });
    });
  }
  add() {
    this.router.navigate(['/uT1safety/add'])
  }
}
