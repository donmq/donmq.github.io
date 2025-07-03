import { Component, OnDestroy, OnInit } from '@angular/core';
import { CommonComponent } from '@commons/common/common';
import { NgxSpinnerService } from 'ngx-spinner';
import { CommonService } from '@services/common.service';
import { ActivatedRoute } from '@angular/router';
import { environment } from '@environments/environment';
import { LocalStorageConstants } from "@constants/storage.constants";

@Component({
  selector: 'app-kaizenmain',
  templateUrl: './kaizenmain.component.html',
  styleUrls: ['./kaizenmain.component.scss']
})
export class KaizenmainComponent extends CommonComponent implements OnInit, OnDestroy {
  previousLink: string = '';
  nextLink: string = '';
  tuCode: string = '';
  urlKaizenSystem: string = environment.ipKaizen;
  constructor(
    private spinner: NgxSpinnerService,
    private commonService: CommonService,
    private route: ActivatedRoute
  ) {
    super(commonService, route);
  }

  ngOnInit(): void {
    this.tuCode = this.deptId.trim();
    this.center_Level = localStorage.getItem(LocalStorageConstants.CENTER_LEVEL);
    this.tier_Level = localStorage.getItem(LocalStorageConstants.TIER_LEVEL);
    this.deptId = this.route.snapshot.params["deptId"];
    this.previousLink = this.center_Level + "/" + this.tier_Level + "/efficiency/efficiencymain_2/CTB/" + this.deptId;
    this.nextLink = this.center_Level + "/" + this.tier_Level + "/safety/safetymain/CTB/" + this.deptId;
    setTimeout(() => {
      this.addMeetingLogPage();
    }, 1000);
  }

  async ngOnDestroy() {
    await this.updateMeetingLogPage();
  }

  redirectToKaizenSystem() {
    this.updateMeetingLogPage(true);
    window.open(this.urlKaizenSystem, '_blank')
  }

}
