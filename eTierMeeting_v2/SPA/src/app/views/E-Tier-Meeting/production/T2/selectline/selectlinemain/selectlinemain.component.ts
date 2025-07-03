import { Component, OnDestroy, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { ProductionT2SelectLineDTO } from "@models/productionT2SelectLineDTO";
import { CommonService } from "@services/common.service";
import { NgSnotifyService } from "@services/ng-snotify.service";
import { SelectmainService } from "@services/production/T2/selectmain/selectmain.service";
import { RecordMeetingDurationService } from "@services/record-meeting-duration.service";
import { LocalStorageConstants } from "@constants/storage.constants";

@Component({
  selector: "app-selectlinemain",
  templateUrl: "./selectlinemain.component.html",
  styleUrls: ["./selectlinemain.component.scss"],
})
export class SelectlinemainComponent implements OnInit, OnDestroy {
  listData: ProductionT2SelectLineDTO = <ProductionT2SelectLineDTO>{
    listC2B: []
  };
  C2B: number[];
  constructor(
    private _service: SelectmainService,
    private router: Router,
    private snotifyService: NgSnotifyService,
    private recordService: RecordMeetingDurationService,
    private commonServices: CommonService,
  ) { }
  ngOnDestroy(): void {
    this.commonServices.end_time.set(null);
  }

  ngOnInit(): void {
    this.getData();
  }
  getData() {
    this._service.GetListProductionT2CTB().subscribe({
      next: (res) => {
        this.listData = res;
        this.C2B = this.listData.listC2B
          .map((item) => +item.lineNum)
          .filter((value, index, self) => self.indexOf(value) === index)
          .sort();
      },
      error: (err) => {
        this.snotifyService.error(err);
      }
    });
  }

  getByLineNumCTB_2ndWay(lineNum: number) {
    return this.listData.listC2B.filter((item) => +item.lineNum === lineNum);
  }

  getLine(deptid: string, class_Level: string) {
    this.t2Record(deptid, class_Level);
    const center_Level: string = localStorage.getItem(LocalStorageConstants.CENTER_LEVEL);
    const tier_Level: string = localStorage.getItem(LocalStorageConstants.TIER_LEVEL);
    this.router.navigate([
      center_Level,
      tier_Level,
      "safety",
      "safetymain",
      class_Level,
      deptid,
    ]);
  }

  t2Record(deptid: string, class_Level: string) {
    this.recordService.t2StartRecord(deptid, class_Level);
  }
}
