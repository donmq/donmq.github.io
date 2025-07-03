import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { CaptionConstants, MessageConstants } from '@constants/system.constants';
import { eTM_Page_Item_Settings } from '@models/etm-page-item-settings';
import { PageItemSettingService } from '@services/production/T2/CTB/page-item-setting.service';
import { OperationResult } from '@utilities/operation-result';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.scss']
})
export class EditComponent extends InjectBase implements OnInit {
  @Output() result: EventEmitter<OperationResult> = new EventEmitter();

  setting: eTM_Page_Item_Settings = <eTM_Page_Item_Settings>{};
  setting_Bak: eTM_Page_Item_Settings = <eTM_Page_Item_Settings>{};

  constructor(
    public bsModalRef: BsModalRef,
    private pageItemSettingService: PageItemSettingService) {
    super();
  }

  ngOnInit(): void {
    this.setting_Bak = { ...this.setting };
  }

  saveForm() {
    this.spinnerService.show();
    this.pageItemSettingService.edit(this.setting).subscribe({
      next: (res) => {
        this.spinnerService.hide();
        if (res.success) {
          this.snotifyService.success(MessageConstants.UPDATED_OK_MSG, CaptionConstants.SUCCESS);
          this.result.emit(res);
          this.bsModalRef.hide();
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

  resetForm() {
    this.setting = { ...this.setting_Bak };
  }
}
