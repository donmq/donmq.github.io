import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgxSpinnerService } from 'ngx-spinner';
import { Efficiency } from '@models/production/efficiency';
import { ProductionT1UPFEfficiencyService } from '@services/production/T1/UPF/production-t1-upf-efficiency.service';
import { CommonService } from '@services/common.service';
import { CommonComponent } from '@commons/common/common';
import { LocalStorageConstants } from "@constants/storage.constants";

@Component({
  selector: 'app-efficiencymain',
  templateUrl: './efficiencymain.component.html',
  styleUrls: ['./efficiencymain.component.scss']
})
export class EfficiencymainComponent extends CommonComponent implements OnInit, OnDestroy {
  data: Efficiency[] = [];
  previousLink: string = "";
  nextLink: string = "";

  constructor(
    private spinner: NgxSpinnerService,
    private efficiencyService: ProductionT1UPFEfficiencyService,
    private commonService: CommonService,
    private route: ActivatedRoute
  ) {
    super(commonService, route);
  }

  ngOnInit(): void {
    this.deptId = this.route.snapshot.params["deptId"];
    this.center_Level = localStorage.getItem(LocalStorageConstants.CENTER_LEVEL);
    this.tier_Level = localStorage.getItem(LocalStorageConstants.TIER_LEVEL);
    this.previousLink = this.center_Level + "/" + this.tier_Level + "/delivery/deliverymain/UPF/" + this.deptId;
    this.nextLink = this.center_Level + "/" + this.tier_Level + "/kaizen/kaizenmain/UPF/" + this.deptId;
    this.loadData();
    setTimeout(() => {
      this.addMeetingLogPage();
    }, 1000);
  }

  async ngOnDestroy() {
    await this.updateMeetingLogPage();
  }

  loadData() {
    this.spinner.show();
    this.efficiencyService
      .getData(this.deptId)
      .subscribe(
        (res) => {
          this.data = res;
        },
        (e) => {
          console.log(e);
        }).add(() => this.spinner.hide());
  }
}
