import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from "@angular/router";
import { UntilDestroy, untilDestroyed } from '@ngneat/until-destroy';
import { environment } from '@environments/environment';
import { DefectTop3 } from '@models/production/T1/C2B/defectop3';
import { FRIBADefect } from '@models/production/T1/C2B/fri-ba-defect';

import { Quality } from '@models/production/T1/C2B/quality';
import { ProductionT1STFQualityService } from '@services/production/T1/STF/production-t1-stf-quality.service';
import { CommonService } from '@services/common.service';
import { CommonComponent } from '@commons/common/common';
import { LocalStorageConstants } from "@constants/storage.constants";

@UntilDestroy()
@Component({
  selector: 'app-qualitymain',
  templateUrl: './qualitymain.component.html',
  styleUrls: ['./qualitymain.component.scss']
})
export class QualitymainComponent extends CommonComponent implements OnInit, OnDestroy {
  quality: Quality = {} as Quality
  defecttop3: DefectTop3[] = []
  fribadefect: FRIBADefect[] = []
  previousLink: string = '';
  nextLink: string = '';
  img_path: string = environment.ip_img_path;
  constructor(
    private qualityser: ProductionT1STFQualityService,
    private commonService: CommonService,
    private route: ActivatedRoute
  ) {
    super(commonService, route);
  }

  ngOnInit(): void {
    this.deptId = this.route.snapshot.params.deptId;
    this.center_Level = localStorage.getItem(LocalStorageConstants.CENTER_LEVEL);
    this.tier_Level = localStorage.getItem(LocalStorageConstants.TIER_LEVEL);
    this.previousLink = this.center_Level + "/" + this.tier_Level + '/safety/safetymain/STF/' + this.deptId;
    this.nextLink = this.center_Level + "/" + this.tier_Level + '/delivery/deliverymain/STF/' + this.deptId;
    this.loadData();
    this.loadDefectTop3();
    setTimeout(() => {
      this.addMeetingLogPage();
    }, 1000);
  }

  async ngOnDestroy() {
    await this.updateMeetingLogPage();
  }

  loadData() {
    this.qualityser.getData(this.deptId)
      .pipe(untilDestroyed(this))
      .subscribe(data => this.quality = data)
  }

  loadDefectTop3() {
    this.qualityser.GetDefectTop3(this.deptId)
      .pipe(untilDestroyed(this))
      .subscribe(data => {
        this.defecttop3 = data
      })
  }
}



