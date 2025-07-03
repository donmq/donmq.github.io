import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { environment } from '@environments/environment';
import { DefectTop3 } from '@models/production/T1/UPF/defectTop3';
import { Quality } from '@models/production/T1/UPF/quality';
import { ProductionT1UpfQualityService } from '@services/production/T1/UPF/production-t1-upf-quality.service';
import { CommonComponent } from '@commons/common/common';
import { ActivatedRoute } from '@angular/router';
import { CommonService } from "@services/common.service";
import { LocalStorageConstants } from "@constants/storage.constants";

@Component({
  selector: 'app-qualitymain',
  templateUrl: './qualitymain.component.html',
  styleUrls: ['./qualitymain.component.scss']
})
export class QualitymainComponent extends CommonComponent implements OnInit, OnDestroy {
  quality: Quality = <Quality>{}
  defecttop3: DefectTop3[] = []
  // fribadefect: FRIBADefect[] = []
  previousLink: string = '';
  nextLink: string = '';
  img_path: string = environment.ip_img_path;
  private subscriptions: Subscription[] = [];
  constructor(
    private productionT1UPFQualityService: ProductionT1UpfQualityService,
    private commonService: CommonService,
    private route: ActivatedRoute
  ) {
    super(commonService, route);
  }

  ngOnInit() {
    this.deptId = this.route.snapshot.params.deptId;
    this.center_Level = localStorage.getItem(LocalStorageConstants.CENTER_LEVEL);
    this.tier_Level = localStorage.getItem(LocalStorageConstants.TIER_LEVEL);
    this.previousLink = this.center_Level + "/" + this.tier_Level + '/safety/safetymain/UPF/' + this.deptId;
    this.nextLink = this.center_Level + "/" + this.tier_Level + '/delivery/deliverymain/UPF/' + this.deptId;
    this.loadData();
    this.loadDefectTop3();
    setTimeout(() => {
      this.addMeetingLogPage();
    }, 1000);
  }

  async ngOnDestroy() {
    await this.updateMeetingLogPage();
  }

  loadData = () => {
    const sub = this.productionT1UPFQualityService.getData(this.deptId).subscribe({
      next: res => {
        this.quality = res;
      }, error: error => {
        console.log(error);
      }
    });
    this.subscriptions.push(sub);
  }

  loadDefectTop3 = () => {
    const sub = this.productionT1UPFQualityService.getDefectTop3(this.deptId).subscribe({
      next: res => {
        this.defecttop3 = res;
      }, error: error => {
        console.log(error);
      }
    });
    this.subscriptions.push(sub);
  }
}
