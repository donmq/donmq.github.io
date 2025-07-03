import { Component, OnDestroy, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { NgSnotifyService } from "@services/ng-snotify.service";
import { environment } from "@environments/environment";
import { DeptClassificationService } from "@services/DeptClassification.service";
import { ProductionT2SelectLineDTO } from "@models/productionT2SelectLineDTO";
import { CommonService } from "@services/common.service";
import { LocalStorageConstants } from "@constants/storage.constants";

@Component({
  selector: "app-selectlinemain",
  templateUrl: "./selectlinemain.component.html",
  styleUrls: ["./selectlinemain.component.css"],
})
export class SelectlinemainComponent implements OnInit, OnDestroy {
  baseUrl = environment.apiUrl;
  // deptClassifications: DeptClassification[]; // model container
  // // new
  // tOrg: TOrg[]; // model container
  listData: ProductionT2SelectLineDTO = <ProductionT2SelectLineDTO>{
    listC2B: [],
    listSTF: [],
    listUPF: [],
  };
  C2B: number[];
  STF: number[];
  UPF: number[];

  constructor(
    private router: Router,
    private snotifyService: NgSnotifyService,
    private service: DeptClassificationService,
    private commonServices: CommonService,
  ) { }

  ngOnInit() {
    this.getDept();
  }

  ngOnDestroy(): void {
    this.commonServices.end_time.set(null);
  }

  getDept() {
    // this.totalrows = 0;
    this.service.getDeptForSelectLine().subscribe(
      (res) => {
        this.listData = res;

        this.C2B = this.listData.listC2B
          .map((item) => item.lineNum)
          .filter((value, index, self) => self.indexOf(value) === index)
          .sort((a, b) => a + b);

        this.STF = this.listData.listSTF
          .map((item) => item.lineNum)
          .filter((value, index, self) => self.indexOf(value) === index)
          .sort((a, b) => a + b);

        this.UPF = this.listData.listUPF
          .map((item) => item.lineNum)
          .filter((value, index, self) => self.indexOf(value) === index)
          .sort((a, b) => a + b);
      },
      (error) => {
        this.snotifyService.error(error);
      }
    );
  }

  getByLineNumCTB_2ndWay(lineNum: number) {
    return this.listData.listC2B.filter((item) => item.lineNum === lineNum);
  }

  getByLineNumSTF_2ndWay(lineNum: number) {
    return this.listData.listSTF.filter((item) => item.lineNum === lineNum);
  }

  getByLineNumUPF_2ndWay(lineNum: number) {
    return this.listData.listUPF.filter((item) => item.lineNum === lineNum);
  }

  getLine(deptid: string, class_Level: string) {
    const center_Level: string = localStorage.getItem(LocalStorageConstants.CENTER_LEVEL);
    const tier_Level: string = localStorage.getItem(LocalStorageConstants.TIER_LEVEL);

    this.router.navigate([
      center_Level,
      tier_Level,
      "safety",
      "safetymain",
      class_Level,
      deptid
    ]);
  }
}
