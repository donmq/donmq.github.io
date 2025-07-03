import { Component, OnInit } from "@angular/core";

import {
  PaginatedResult,
  Pagination,
} from "@models/pagination";
import { DeptClassification } from "@models/deptclassification";
import { DeptClassificationService } from "@services/DeptClassification.service";
import { NgSnotifyService } from "@services/ng-snotify.service";
import { KeyValuePair } from "@utilities/key-value-pair";
import { InjectBase } from "@utilities/inject-base-app";



@Component({
  selector: "app-deptclassification-list",
  templateUrl: "./deptclassification-list.component.html",
  styleUrls: ["./deptclassification-list.component.scss"],
})
export class DeptclassificationListComponent extends InjectBase implements OnInit {
  deptClassifications: DeptClassification[]; // model container
  deptClassification: any = {}; // add function容器
  deptList: KeyValuePair[]=[];
  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 1,
    totalPages: 1,
  };
  paramSearch: any = {}; // API helper(query condition)

  constructor(
    private deptClassificationService: DeptClassificationService,
  ) {
    super();
  }

  ngOnInit() {
    this.spinnerService.show();
    this.paramSearch.class_kind = "";
    this.paramSearch.dept_ID = "";
    this.paramSearch.class_Name = "";
    this.loadClassification();
    this.getAllDeptID();
    this.spinnerService.hide();
  }

  // get data
  loadClassification() {
    // console.log('Page: ',this.pagination);
    // console.log(this.paramSearch);
    this.deptClassificationService.search(
      this.pagination.currentPage,
      this.pagination.itemsPerPage,
      this.paramSearch)
      .subscribe(res => {
        // debugger;
        this.pagination = res.pagination;
        this.deptClassifications = res.result;
        // console.log("Classification data : ", this.deptClassifications);
      });
    setTimeout(() => {
    }, 3000);
  }

  // Dept 下拉選單
  getAllDeptID() {
    this.deptClassificationService.getDept().subscribe(
      (data) => {
        // console.log(data);
        this.deptList = data.map((item) => {
          // console.log(item);
          return { id: item.dept_ID, text: item.dept_Sname };
        });
      },
      (error) => {
        this.snotifyService.error(error);
      }
    )
  }

  // 刪除部門分類function
  deleteClassification(item: DeptClassification) {
    this.snotifyService.confirm('Delete ' + item.class_Kind + ' Class_Kine, ' + item.dept_ID + ' Dept', 'Are you sure you want to delete this Classification??', () => {
      this.deptClassificationService.deleteClassification(item).subscribe(() => {
        this.loadClassification();
        this.snotifyService.success('Classification has been deleted!');
      }, error => {
        this.snotifyService.error('This Classification is using!');
      });
    });
  }

  // 換頁
  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.loadClassification();
  }

  // open add page
  addClassification() {
    this.deptClassification = {};
    this.deptClassificationService.changeDeptClassification(this.deptClassification);
    this.deptClassificationService.changeFlag("0");
    // console.log(this.deptClassification);
    // console.log(this.deptClassificationService.flagSource.value);
    this.router.navigate(["/classification/add"]);
  }

  // open edit page
  changeToEdit(item: DeptClassification) {
    // console.log(item);
    this.deptClassificationService.changeDeptClassification(item);
    this.deptClassificationService.changeFlag("1");
    // console.log(this.deptClassificationService.flagSource.value);
    this.router.navigate(["/classification/edit"]);
  }

  // clear inputbox
  clearSearch() {
    // bind API helper
    this.paramSearch.class_kind = "";
    this.paramSearch.dept_ID = "";
    this.paramSearch.class_Name = "";
  }

}
