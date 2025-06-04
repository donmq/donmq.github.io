import { Component, OnInit } from '@angular/core';
import { CommonConstants } from '@constants/common.constants';
import { HistoryEmp } from '@models/seahr/employee-managers/HistoryEmp';
import { SeaEmployeeFilter } from '@params/seahr/employee-managers/seaEmployeeFilter';
import { SeahrEmployeeManagerService } from '@services/seahr/seahr-employee-manager.service';
import { Pagination } from '@utilities/pagination-utility';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { LangConstants } from '@constants/lang.constants';
import { DestroyService } from '@services/destroy.service';
import { Subject, debounceTime, takeUntil } from 'rxjs';
import { KeyValuePair } from '@utilities/key-value-pair';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-seahr-emp-management',
  templateUrl: './seahr-emp-management.component.html',
  styleUrls: ['./seahr-emp-management.component.scss'],
  providers: [DestroyService]
})
export class SeahrEmpManagementComponent extends InjectBase implements OnInit {

  areas: KeyValuePair[] = [];
  departments: KeyValuePair[] = [];
  parts: KeyValuePair[] = [];
  listData: HistoryEmp[] = [];

  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 20,
  }

  filter: SeaEmployeeFilter = <SeaEmployeeFilter>{ employeeId: '' }
  commonConstants: typeof CommonConstants = CommonConstants;
  lang: string = localStorage.getItem(LocalStorageConstants.LANG) ?? LangConstants.VN;
  searchSubject: Subject<string> = new Subject<string>();

  constructor(
    private services: SeahrEmployeeManagerService,
  ) {
    super();
  }

  ngOnInit(): void {
    this.services.dataSource.subscribe({
      next: result => {
        if (result) {
          this.filter = result;
          if (this.filter.areaId)
            this.getDepartment(this.filter.areaId)
          if (this.filter.departmentId)
            this.getParts(this.filter.departmentId)
          this.loadData();
        }
        else {
          this.loadData();
        }
      }
    })
    this.getArea();
    this.translateService.onLangChange.pipe(takeUntil(this.destroyService.destroys$)).subscribe(async res => {
      this.lang = res.lang;
      this.getArea();
      this.loadData();
    });
    this.searchSubject.pipe(debounceTime(500)).subscribe(() => {
      this.search();
    });
  }

  back() {
    this.services.dataSource.next(this.filter)
    this.router.navigate(['/seahr']);
  }

  loadData() {
    this.spinnerService.show();
    this.services.search(this.pagination, this.filter).subscribe(
      data => {
        this.listData = data.result;
        this.pagination = data.pagination;
        this.spinnerService.hide();
      }
    )
  }
  onKeyUpEmployee() {
    this.searchSubject.next(this.filter.employeeId);
  }
  search() {
    this.pagination.pageNumber = 1;
    this.loadData();
  }

  getArea() {
    this.spinnerService.show();
    this.services.getAreas().subscribe(
      result => {
        this.areas = result;
        this.spinnerService.hide();
      }
    )
  }

  getDepartment(areaId: number) {
    this.spinnerService.show();
    this.services.getDepartments(areaId).subscribe(
      result => {
        this.departments = result;
        this.spinnerService.hide();
      }
    )
  }

  getParts(deptId: number) {
    this.spinnerService.show();
    this.services.getParts(deptId).subscribe(
      result => {
        this.parts = result;
        this.spinnerService.hide();
      }
    )
  }

  areaChange(e: any) {
    if (!this.functionUtility.checkEmpty(e)) {
      this.filter.departmentId = null;
      this.departments = [];
      this.filter.partId = null;
      this.parts = [];
      this.getDepartment(e);
    }
  }

  departmentChange(e: any) {
    if (!this.functionUtility.checkEmpty(e)) {
      this.filter.partId = null;
      this.parts = [];
      this.getParts(e);
    }
  }

  partChange(e: any) {
    if (!this.functionUtility.checkEmpty(e)) this.filter.partId = e;
  }

  pageChanged(event) {
    this.pagination.pageNumber = event.page;
    this.loadData();
  }

  clear() {
    this.filter.employeeId = '',
      this.filter.areaId = null;
    this.filter.departmentId = null;
    this.departments = [];
    this.filter.partId = null;
    this.parts = [];
    this.pagination.pageNumber = 1;
    this.loadData();
  }

  clearPart = () => this.filter.partId = null;


  clearDepartment() {
    this.filter.partId = null;
    this.parts = [];
    this.filter.departmentId = null;
  }

  onGoToDetail(empId: number) {
    this.router.navigate([`/manage/employee/detail/${empId}/seahrEmpManager`]).then(
      () => {
        this.services.dataSource.next(this.filter)
      },
      (error) => { }
    );
  };
}
