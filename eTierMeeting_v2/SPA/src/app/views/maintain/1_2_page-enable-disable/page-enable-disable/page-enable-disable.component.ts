import { Component, OnInit } from '@angular/core';

import { CaptionConstants, MessageConstants } from '@constants/system.constants';
import { PageEnableDisableParam } from '../../../../_core/_helpers/params/page-enable-disable.param';
import { eTM_Page_Settings } from '@models/etm-page-settings';
import { PageEnableDisableService } from '@services/page-enable-disable.service';
import { ChangeRouterService } from '@services/hubs/change-router.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-page-enable-disable',
  templateUrl: './page-enable-disable.component.html',
  styleUrls: ['./page-enable-disable.component.scss']
})
export class PageEnableDisableComponent extends InjectBase implements OnInit {
  centers: KeyValuePair[] = [];
  tiers: KeyValuePair[] = [];
  sections: KeyValuePair[] = [];
  pages: eTM_Page_Settings[] = [];
  curPages: eTM_Page_Settings[] = [];
  param: PageEnableDisableParam = <PageEnableDisableParam>{
    center_Level: null,
    class_Level: null,
    tier_Level: null,
  };

  constructor(
    private pageEnableDisableService: PageEnableDisableService,
    private changeRouterService: ChangeRouterService) {
    super();
  }

  ngOnInit(): void {
    this.getCenters();
  }

  getCenters(): void {
    this.pageEnableDisableService.getCenters().subscribe({
      next: (res) => this.centers = res,
      error: () => this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR)
    });
  }

  getTiers(center: string): void {
    this.pageEnableDisableService.getTiers(center).subscribe({
      next: (res) => this.tiers = res,
      error: () => this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR)
    });
  }

  getSections(center: string, tier: string): void {
    this.pageEnableDisableService.getSections(center, tier).subscribe({
      next: (res) => this.sections = res,
      error: () => this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR)
    });
  }

  centerLevelChanged(): void {
    this.param.tier_Level = null;
    this.param.class_Level = null;

    if (this.param.center_Level) {
      this.getTiers(this.param.center_Level);
    }
  }

  tierLevelChanged(): void {
    this.param.class_Level = null;

    if (this.param.center_Level && this.param.tier_Level) {
      this.getSections(this.param.center_Level, this.param.tier_Level);
    }
  }

  search(): void {
    this.spinnerService.show();
    this.pageEnableDisableService.getPages(this.param).subscribe({
      next: (res) => {
        this.spinnerService.hide();
        this.pages = res;
        this.curPages = [...this.pages];
      },
      error: () => {
        this.spinnerService.hide();
        this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR);
      },
    });
  }

  savePages(): void {
    this.spinnerService.show();
    this.pageEnableDisableService.updatePages(this.pages).subscribe({
      next: (res) => {
        this.spinnerService.hide();
        if (res.success) {
          this.pages.forEach(item => {
            if (item.is_Active == true) {
              this.changeRouterService.sendMessage(item.link)
            }
          })
          this.snotifyService.success(MessageConstants.UPDATED_OK_MSG, CaptionConstants.SUCCESS);
          this.clearForm();
        } else {
          this.snotifyService.error(MessageConstants.UPDATED_ERROR_MSG, CaptionConstants.ERROR);
        }

      },
      error: () => {
        this.spinnerService.hide();
        this.snotifyService.error(MessageConstants.UN_KNOWN_ERROR, CaptionConstants.ERROR);
      },
    })
  }

  clearForm(): void {
    this.param = <PageEnableDisableParam>{
      center_Level: null,
      class_Level: null,
      tier_Level: null,
    };
    this.pages = [];
  }

  onChecked(item: eTM_Page_Settings) {
    if (this.param.center_Level.trim() === 'Production' && this.param.tier_Level.trim() === 'T5') {
      this.pages.map(e => {
        if (e.page_Name !== item.page_Name) {
          e.is_Active = e.is_Active ? false : false;
          return e;
        }
      });
    }
  }
}
