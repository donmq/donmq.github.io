import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';
import { takeUntil } from 'rxjs/operators';
import { CaptionConstants, MessageConstants } from '@constants/system.constants';
import { PageItemSettingParam } from '../../../../_core/_helpers/params/page-item-setting.param';
import { eTM_Page_Item_Settings } from '@models/etm-page-item-settings';
import { PageItemSettingService } from '@services/production/T2/CTB/page-item-setting.service';
import { OperationResult } from '@utilities/operation-result';
import { Pagination, PaginationResult } from '@utilities/pagination-helper';
import { AddComponent } from '../add/add.component';
import { EditComponent } from '../edit/edit.component';
import { KeyValuePair } from '@utilities/key-value-pair';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent extends InjectBase implements OnInit {
  centers: KeyValuePair[] = [];
  tiers: KeyValuePair[] = [];
  sections: KeyValuePair[] = [];
  param: PageItemSettingParam = <PageItemSettingParam>{
    center_Level: null,
    class_Level: null,
    tier_Level: null
  };
  pagination: Pagination = <Pagination>{};
  settings: eTM_Page_Item_Settings[] = [];
  bsModalRef?: BsModalRef;

  constructor(
    private pageItemSettingService: PageItemSettingService,
    private modalService: BsModalService) {
    super();
  }

  ngOnInit(): void {
    const res: PaginationResult<eTM_Page_Item_Settings> = this.route.snapshot.data['res'];
    this.pagination = res.pagination;
    this.settings = res.result;
    this.getCenterLevels();
  }

  search(): void {
    this.pagination.pageNumber != 1 ? this.pagination.pageNumber = 1 : this.getData();
  }

  getData(): void {
    this.spinnerService.show();
    this.pageItemSettingService.getAll(this.param, this.pagination).subscribe({
      next: (res) => {
        this.spinnerService.hide();
        this.pagination = res.pagination;
        this.settings = res.result;
      },
      error: () => {
        this.spinnerService.hide();
        this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR);
      }
    })
  }

  clearForm(): void {
    this.param = <PageItemSettingParam>{
      center_Level: null,
      class_Level: null,
      tier_Level: null
    };
    this.pagination = <Pagination>{
      pageNumber: 1,
      pageSize: 10
    };
    this.tiers = [];
    this.sections = [];
    this.getData();
  }

  getCenterLevels(): void {
    this.pageItemSettingService.getCenterLevels().subscribe({
      next: (res) => this.centers = res,
      error: () => this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR)
    });
  }

  getTierLevels(center_Level: string): void {
    this.pageItemSettingService.getTierLevels(center_Level).subscribe({
      next: (res) => {
        this.tiers = res
      },
      error: () => this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR)
    });
  }

  getSections(center_Level: string, tier_Level: string): void {
    this.pageItemSettingService.getSections(center_Level, tier_Level).subscribe({
      next: (res) => this.sections = res,
      error: () => this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR)
    });
  }

  centerLevelChanged(): void {
    this.param.tier_Level = null;
    this.param.class_Level = null;

    if (this.param.center_Level) {
      this.getTierLevels(this.param.center_Level);
    }
  }

  tierLevelChanged(): void {
    this.param.class_Level = null;

    if (this.param.center_Level && this.param.tier_Level)
      this.getSections(this.param.center_Level, this.param.tier_Level);
  }

  pageChanged(e: PageChangedEvent): void {
    this.pagination.pageNumber = e.page;
    this.getData();
  }

  openAddModal() {
    this.bsModalRef = this.modalService.show(AddComponent);
    this.bsModalRef.content.result
      .pipe(takeUntil(this.destroyService.destroyed$))
      .subscribe((res: OperationResult) => {
        if (res.success) {
          this.getData();
        }
      });
  }

  openEditModal(setting: eTM_Page_Item_Settings) {
    const initialState: ModalOptions = { initialState: <any>{ setting: { ...setting } } };
    this.bsModalRef = this.modalService.show(EditComponent, initialState);
    this.bsModalRef.content.result
      .pipe(takeUntil(this.destroyService.destroyed$))
      .subscribe((res: OperationResult) => {
        if (res.success) {
          this.getData();
        }
      });
  }
}