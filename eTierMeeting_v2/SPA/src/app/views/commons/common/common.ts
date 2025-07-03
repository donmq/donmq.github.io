import { Component, effect } from '@angular/core';
import { CommonService } from '../../../_core/_services/common.service';
import { eTM_Meeting_Log_Page, eTM_Meeting_Log_PageParamDTO } from '../../../_core/_models/eTM_Meeting_Log_Page';
import { ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-common',
  template: `
  `,
  styles: [
  ]
})
export class CommonComponent {
  center_Level: string = '';
  tier_Level: string = '';
  record_ID: string = '';
  clickLink: boolean = false;
  deptId: string = "";
  end_time: Date = null;
  constructor(private commonServices: CommonService, private routes: ActivatedRoute) {
    effect(() => {
      this.record_ID = this.commonServices.recordID();
      this.end_time = this.commonServices.end_time();
    });
  }

  async updateMeetingLogPage(clickLinkKaizenSystem: boolean = false) {
    let param: eTM_Meeting_Log_PageParamDTO = {
      clickLinkKaizenSystem: clickLinkKaizenSystem,
      record_ID: this.record_ID
    }
    await this.commonServices.updateMeetingLogPage(param).subscribe({
      next: (res) => {
        this.commonServices.end_time.set(res.data);
      },
      error: (err) => {
      }
    })
  }

  async addMeetingLogPage() {
    let eTM_Meeting_Log_Page: eTM_Meeting_Log_Page = <eTM_Meeting_Log_Page>{
      center_Level: this.center_Level,
      click_Link: this.clickLink,
      deptId: this.deptId,
      tier_Level: this.tier_Level,
      class_Level: this.routes.snapshot.data['class_Level'],
      page_Name: this.routes.snapshot.data['page_Name'],
      start_Time: this.end_time
    }
    this.record_ID = await this.commonServices.addMeetingLogPage(eTM_Meeting_Log_Page).toPromise()
    this.commonServices.recordID.set(this.record_ID);
  }

}
