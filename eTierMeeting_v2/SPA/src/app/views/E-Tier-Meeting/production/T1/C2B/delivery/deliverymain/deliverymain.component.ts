import { Component, OnDestroy, OnInit } from "@angular/core";
import { UntilDestroy, untilDestroyed } from "@ngneat/until-destroy";
import { SnotifyService } from "ng-alt-snotify";
import { NgxSpinnerService } from "ngx-spinner";
import { DeliveryDTO } from "@models/deliveryDTO";
import { DeliveryService } from "@services/production/T1/C2B/delivery.service";
import { CommonComponent } from "@commons/common/common";
import { ActivatedRoute } from '@angular/router';
import { CommonService } from "@services/common.service";
import { LocalStorageConstants } from "@constants/storage.constants";
@UntilDestroy()
@Component({
  selector: "app-deliverymain",
  templateUrl: "./deliverymain.component.html",
  styleUrls: ["./deliverymain.component.scss"],
})
export class DeliverymainComponent extends CommonComponent implements OnInit, OnDestroy {
  deliveryList: DeliveryDTO[] = [];
  previousLink: string = "";
  nextLink: string = "";
  constructor(
    private deliveryService: DeliveryService,
    private spinner: NgxSpinnerService,
    private snotify: SnotifyService,
    private commonService: CommonService,
    private route: ActivatedRoute
  ) {
    super(commonService, route);
  }

  ngOnInit(): void {
    this.deptId = this.route.snapshot.params.deptId;
    this.center_Level = localStorage.getItem(LocalStorageConstants.CENTER_LEVEL);
    this.tier_Level = localStorage.getItem(LocalStorageConstants.TIER_LEVEL);
    this.previousLink = this.center_Level + "/" + this.tier_Level + "/quality/qualitymain/CTB/" + this.deptId;
    this.nextLink = this.center_Level + "/" + this.tier_Level + "/efficiency/efficiencymain/CTB/" + this.deptId;
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
