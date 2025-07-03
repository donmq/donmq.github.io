import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { SnotifyService } from 'ng-alt-snotify';
import { NgxSpinnerService } from 'ngx-spinner';
import { VW_Production_T1_UPF_Delivery_RecordDTO } from '@models/production/T1/UPF/vW_Production_T1_UPF_Delivery_RecordDTO';
import { ProductionT1UPFDeliveryService } from '@services/production/T1/UPF/ProductionT1UPFDelivery.service';
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { CommonService } from '@services/common.service';
import { CommonComponent } from '@commons/common/common';
import { LocalStorageConstants } from "@constants/storage.constants";

@UntilDestroy()
@Component({
  selector: 'app-delivery-main',
  templateUrl: './delivery-main.component.html',
  styleUrls: ['./delivery-main.component.scss']
})
export class DeliveryMainComponent extends CommonComponent implements OnInit, OnDestroy {
  deliveryList: VW_Production_T1_UPF_Delivery_RecordDTO[] = [];
  previousLink: string = "";
  nextLink: string = "";
  constructor(
    private commonService: CommonService,
    private route: ActivatedRoute,
    private deliveryService: ProductionT1UPFDeliveryService,
    private spinner: NgxSpinnerService,
    private snotify: SnotifyService) {
    super(commonService, route);
  }

  ngOnInit(): void {
    this.deptId = this.route.snapshot.params.deptId;
    this.center_Level = localStorage.getItem(LocalStorageConstants.CENTER_LEVEL);
    this.tier_Level = localStorage.getItem(LocalStorageConstants.TIER_LEVEL);
    this.previousLink = this.center_Level + "/" + this.tier_Level + "/quality/qualitymain/UPF/" + this.deptId;
    this.nextLink = this.center_Level + "/" + this.tier_Level + "/efficiency/efficiencymain/UPF/" + this.deptId;
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
    this.deliveryService
      .getAllDelivery(this.deptId)
      .pipe(untilDestroyed(this))
      .subscribe(
        (res) => {
          this.deliveryList = res;
          this.spinner.hide();
        },
        (error) => {
          this.snotify.error(error);
        }
      );
  }
}
