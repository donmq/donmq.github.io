import { Component, OnDestroy, OnInit } from "@angular/core";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";

import { Efficiency } from "@models/production/efficiency";
import { ProductionT1STFEfficiencyService } from "@services/production/T1/STF/production-t1-stf-efficiency.service";
import { CommonComponent } from "@commons/common/common";
import { ActivatedRoute } from '@angular/router';
import { CommonService } from "@services/common.service";
import { LocalStorageConstants } from "@constants/storage.constants";


@UntilDestroy()
@Component({
  selector: "app-efficiencymain",
  templateUrl: "./efficiencymain.component.html",
  styleUrls: ["./efficiencymain.component.scss"],
})
export class EfficiencymainComponent extends CommonComponent implements OnInit, OnDestroy {
  data: Efficiency[] = [];
  previousLink: string = "";
  nextLink: string = "";
  constructor(
    private efficiencyService: ProductionT1STFEfficiencyService,
    private commonService: CommonService,
    private route: ActivatedRoute
  ) {
    super(commonService, route);
  }

  ngOnInit(): void {
    this.deptId = this.route.snapshot.params["deptId"];
    this.center_Level = localStorage.getItem(LocalStorageConstants.CENTER_LEVEL);
    this.tier_Level = localStorage.getItem(LocalStorageConstants.TIER_LEVEL);
    this.previousLink = this.center_Level + "/" + this.tier_Level + "/delivery/deliverymain/STF/" + this.deptId;
    this.nextLink = this.center_Level + "/" + this.tier_Level + "/kaizen/kaizenmain/STF/" + this.deptId;
    this.loadData();
    setTimeout(() => {
      this.addMeetingLogPage();
    }, 1000);
  }

  async ngOnDestroy() {
    await this.updateMeetingLogPage();
  }

  loadData() {
    this.efficiencyService
      .getData(this.deptId)
      .pipe(untilDestroyed(this))
      .subscribe((res) => this.data = res);
  }
}
