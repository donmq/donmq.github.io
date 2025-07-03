import { Component, OnInit } from "@angular/core";
import { DeptClassificationService } from "@services/DeptClassification.service";
import { Router } from "@angular/router";
 
import { NgSnotifyService } from "@services/ng-snotify.service";
import { KeyValuePair } from "@utilities/key-value-pair";
import { InjectBase } from "@utilities/inject-base-app";

@Component({
  selector: "app-deptclassification-edit",
  templateUrl: "./deptclassification-edit.component.html",
  styleUrls: ["./deptclassification-edit.component.scss"],
})
export class DeptclassificationEditComponent extends InjectBase implements OnInit {
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
    this.deptclassificationService.currentclassification.subscribe(
      (deptclassification) => (this.deptclassification = deptclassification)
    );
    // console.log(this.deptclassification);
    // this.getAllDeptID();
    // this.lineID2=this.deptclassification.dept_Name;
    // flag 0=add, 1=edit
    // this.deptclassificationService.currentFlag.subscribe(
    //   (flag) => (this.flag = flag)
    // );
    // if (this.flag === '0') this.deptclassification.dept_ID = "";
    // console.log(this.flag);
  }

  changeClassification(event: any) {
    this.lineID2 = "";
    if (event != "") {
      // console.log(event);
      this.lineID2 = this.listdataClassification.find(
        (x) => x.dept_ID == event
      ).line_ID_2;
    }
    this.lineID2 = this.deptclassification.dept_Name;
  }

  // Dept 下拉選單
  getAllDeptID() {
    this.deptclassificationService.getDept().subscribe(
      (data) => {
        // console.log(data);
        this.listdataClassification = data;
        // console.log(this.listdataClassification);
        this.deptList = data.map((item) => {
          // console.log(item);
          return { id: item.dept_ID, text: item.dept_Sname };
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

  save() {
    // console.log(this.deptclassification);
    // console.log(this.lineID2);
    // console.log("save edit");
    this.deptclassificationService
      .updateDeptClassification(this.deptclassification)
      .subscribe(
        () => {
          this.snotifyService.success("Updated succeed");
          this.router.navigate(["/classification"]);
        },
        (error) => {
          this.snotifyService.error(error);
        }
      );
  }

  clear() {
    this.deptclassification = {};
  }
}
