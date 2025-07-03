import { Component, EventEmitter, OnInit, Output } from '@angular/core';
 
import { BsModalRef } from 'ngx-bootstrap/modal';
import { CaptionConstants, MessageConstants } from '@constants/system.constants';
import { eTM_Page_Item_Settings } from '@models/etm-page-item-settings';
import { PageItemSettingService } from '@services/production/T2/CTB/page-item-setting.service';
import { OperationResult } from '@utilities/operation-result';
import { KeyValuePair } from '@utilities/key-value-pair';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-add',
  templateUrl: './add.component.html',
  styleUrls: ['./add.component.scss']
})
export class AddComponent extends InjectBase implements OnInit {
  @Output() result: EventEmitter<OperationResult> = new EventEmitter();

  centers: KeyValuePair[] = [];
  tiers: KeyValuePair[] = [];
  sections: KeyValuePair[] = [];
  pages: KeyValuePair[] = [];
  setting: eTM_Page_Item_Settings = <eTM_Page_Item_Settings>{
    center_Level: null,
    tier_Level: null,
    class_Level: null,
    page_Name: null
  };

  constructor(
    public bsModalRef: BsModalRef,
    private pageItemSettingService: PageItemSettingService) {
    super();
  }

  ngOnInit(): void {
    this.getCenterLevels();
    this.getPages();
  }

  getCenterLevels(): void {
    this.pageItemSettingService.getCenterLevels().subscribe({
      next: (res) => this.centers = res,
      error: () => this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR)
    });
  }

  getTierLevels(center_Level: string): void {
    this.pageItemSettingService.getTierLevels(center_Level).subscribe({
      next: (res) => this.tiers = res,
      error: () => this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR)
    });
  }

  getSections(center_Level: string, tier_Level: string): void {
    this.pageItemSettingService.getSections(center_Level, tier_Level).subscribe({
      next: (res) => this.sections = res,
      error: () => this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR)
    });
  }

  getPages(): void {
    this.pageItemSettingService.getPages().subscribe({
      next: (res) => this.pages = res,
      error: () => this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR)
    });
  }

  centerLevelChanged(): void {
    this.setting.tier_Level = null;
    this.setting.class_Level = null;

    if (this.setting.center_Level)
      this.getTierLevels(this.setting.center_Level);
  }

  tierLevelChanged(): void {
    this.setting.class_Level = null;

    if (this.setting.center_Level && this.setting.tier_Level)
      this.getSections(this.setting.center_Level, this.setting.tier_Level);
  }

  saveForm() {
    this.spinnerService.show();
    this.pageItemSettingService.add(this.setting).subscribe({
      next: (res) => {
        this.spinnerService.hide();
        if (res.success) {
          this.snotifyService.success(MessageConstants.CREATED_OK_MSG, CaptionConstants.SUCCESS);
          this.result.emit(res);
          this.bsModalRef.hide();
        } else {
          this.snotifyService.error(MessageConstants.CREATED_ERROR_MSG, CaptionConstants.ERROR);
        }
      },
      error: () => {
        this.spinnerService.hide();
        this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR);
      },
    })
  }

  resetForm() {
    this.tiers = [];
    this.sections = [];
    this.setting = <eTM_Page_Item_Settings>{
      center_Level: null,
      tier_Level: null,
      class_Level: null,
      page_Name: null
    };
  }
}