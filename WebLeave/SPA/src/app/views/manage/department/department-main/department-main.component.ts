import { Component, OnInit, TemplateRef } from '@angular/core';
import { LangChangeEvent, TranslateService } from '@ngx-translate/core';
import { DestroyService } from '@services/destroy.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Subject, debounceTime, takeUntil } from 'rxjs';
import { Departments } from '@models/manage/departments';
import { DepartmentParams } from '@params/manage/departmentParams';
import { DepartmentService } from '@services/manage/Department.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { Pagination } from '@utilities/pagination-utility';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-department-main',
  templateUrl: './department-main.component.html',
  styleUrls: ['./department-main.component.css'],
  providers: [DestroyService]
})
export class DepartmentMainComponent extends InjectBase implements OnInit {
  areaList: KeyValuePair[] = [];
  departmentList: Departments[] = [];
  detailDepartments: Departments = <Departments>{};
  modalRef?: BsModalRef;
  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 20,
    totalPage: 0,
    totalCount: 0,
  };
  paramSearch: DepartmentParams = {
    areaID: 'All',
    deptCode: '',
  };
  lang: string = 'vi';
  searchSubject: Subject<string> = new Subject<string>();
  constructor(
    private _service: DepartmentService,
    private modal: BsModalService,
    public translate: TranslateService,
  ) {
    super();
  }

  ngOnInit(): void {
    this._service.dataSource.subscribe({
      next: result => {
        if (result) {
          this.paramSearch = result;
        }
      }
    })
    this.getAllAreas();
    this.getAllDepartments();
    this.translate.onLangChange.pipe(takeUntil(this.destroyService.destroys$)).subscribe((event: LangChangeEvent) => {
      this.lang = event.lang;
    })
    this.searchSubject.pipe(debounceTime(500)).subscribe(() => {
      this.search();
    });
  }
  getAllAreas() {
    this._service.getAllAreas().subscribe({
      next: (res) => {
        this.areaList = res;
        this.areaList.unshift({
          key: 'All',
          value: 'Chọn Khu Vực Phòng Ban',
        });
      },
    });
  }
  getAllDepartments() {
    this.spinnerService.show();
    this._service
      .getAllDepartments(this.pagination, this.paramSearch)
      .subscribe({
        next: (res) => {
          if (res) {
            this.departmentList = res.result;
            this.pagination = res.pagination;
            this.spinnerService.hide();
          } else {
            this.router.navigate(['manage']);
          }
        },
        error: () => {
          this.router.navigate(['manage']);
        }
      });
  }

  onKeyUpDeptCode() {
    this.searchSubject.next(this.paramSearch.deptCode);
  }
  search() {
    this.pagination.pageNumber = 1;
    this.getAllDepartments();
  }
  pageChanged(e: any) {
    this.pagination.pageNumber = e.page;
    this.getAllDepartments();
  }
  detail(template: TemplateRef<any>, item: Departments) {
    this._service.viewAndEdit(item);

    this.detailDepartments = item;
    this.modalRef = this.modal.show(template);
  }
  add() {
    this.router.navigate(['manage/department/add']);
  }
  edit(item: Departments) {
    this._service.viewAndEdit(item);
    this._service.dataSource.next(this.paramSearch)
    this.router.navigate(['manage/department/update']);
  }
  back() {
    this._service.dataSource.next(this.paramSearch)
    this.router.navigateByUrl('/manage');
  }
}
