import { Component, OnInit } from "@angular/core";
import { DeptClassificationService } from "@services/DeptClassification.service";
import { Router } from "@angular/router";
 
import { NgSnotifyService } from "@services/ng-snotify.service";
import { KeyValuePair } from "@utilities/key-value-pair";
import { InjectBase } from "@utilities/inject-base-app";

@Component({
  selector: "app-deptclassification-add",
  templateUrl: "./deptclassification-add.component.html",
  styleUrls: ["./deptclassification-add.component.scss"],
})
export class DeptclassificationAddComponent extends InjectBase implements OnInit {
  listdataClassification: any;
  lineID2: string = "";
  deptclassification: any = {};
  deptList: KeyValuePair[]=[];
  // flag = "100";
  constructor(
    private deptclassificationService: DeptClassificationService,
  ) {
    super();
  }

  ngOnInit() {
    // this.deptclassificationService.currentclassification.subscribe(
    //   (deptclassification) => (this.deptclassification = deptclassification)
    // );
    // console.log(this.deptclassification);
    // flag 0=add, 1=edit
    // this.deptclassificationService.currentFlag.subscribe(
    //   (flag) => (this.flag = flag)
    // );
    // this.deptclassification.dept_ID = "";
    // if (this.flag === "0") {
    //   this.deptclassification.dept_ID = "";
    // }

    // this.getAllDeptID();
  }

  changeClassification(event: any) {
    this.lineID2 = "";
    if (event != "") {
      // console.log(event);
      this.lineID2 = this.listdataClassification.find(
        (x) => x.dept_ID == event
      ).line_ID_2;
    }
  }

  // Dept 下拉選單 改為使用者輸入暫時不使用
  getAllDeptID() {
    this.deptclassificationService.getDept().subscribe(
      (data) => {
        // console.log(data);
        this.listdataClassification = data;
        // console.log(this.listdataClassification);
        this.deptList = data.map((item) => {
          // console.log(item);
          return { key: item.dept_ID, value: item.line_ID_2 };
        });
      },
      (error) => {
        this.snotifyService.error(error);
      }
    );
  }

  backList() {
    this.router.navigate(["/classification/"]);
  }

  saveAndNext() {
    // console.log(this.deptclassification);
    // console.log(this.lineID2);
    // console.log("save and next");
    this.deptclassificationService
      .createDeptClassification(this.deptclassification)
      .subscribe(
        () => {
          this.snotifyService.success("Add succeed");
          this.deptclassification = {};
          // save page
        },
        (error) => {
          this.snotifyService.error(error);
        }
      );
  }

  save() {
    // console.log("save add");
    this.deptclassificationService
      .createDeptClassification(this.deptclassification)
      .subscribe(
        () => {
          this.snotifyService.success("Add succeed");
          this.router.navigate(["/classification"]);
        },
        (error) => {
          // console.log(error);
          this.snotifyService.error(error);
        }
      );
  }

  clear() {
    this.deptclassification = {};
  }
}
