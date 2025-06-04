import { Component, OnInit } from '@angular/core';
import { EmployeeService } from '@services/manage/employee.service';
import { Pagination } from '@utilities/pagination-utility';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { ListEmployee } from '@models/manage/employee-manage/employee';
import { DestroyService } from '@services/destroy.service';
import { LangConstants } from '@constants/lang.constants';
import { Subject, debounceTime, takeUntil } from 'rxjs';
import { InjectBase } from '@utilities/inject-base-app';
import { EmployeeParam } from '@params/manage/employee-param';
@Component({
  selector: 'app-employee-list',
  templateUrl: './employee-list.component.html',
  styleUrls: ['./employee-list.component.css'],
  providers: [DestroyService]
})
export class EmployeeListComponent extends InjectBase implements OnInit {
  lang: string = localStorage.getItem(LocalStorageConstants.LANG) ?? LangConstants.VN;
  employee: ListEmployee[] = [];
  param :EmployeeParam = <EmployeeParam>{}
  search_key: string = '';
  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 20
  };
  searchSubject: Subject<string> = new Subject<string>();

  constructor(
    private employeeService: EmployeeService,

  ) {
    super();
  }


  ngOnInit() {
    this.employeeService.dataSource.subscribe({
      next: result => {
        if (result) {
          this.param = result;
        }
      }
    })
    this.search();
    this.translateService.onLangChange.pipe(takeUntil(this.destroyService.destroys$)).subscribe(async res => {
      this.lang = res.lang == 'zh' ? 'zh-TW' : res.lang;
      this.search();
    });
    this.searchSubject.pipe(debounceTime(500)).subscribe(() => {
      this.search();
    });
  }

  //Button export file Excel
  // change Excel
  exportExcel() {
    this.spinnerService.show();
    this.employeeService.exportExcel().subscribe({
      next: (result) => {
        this.spinnerService.hide();
        result.isSuccess ? this.functionUtility.exportExcel(result.data, 'Employee')
          : this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
      },
      error: () => {
        this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
        this.spinnerService.hide();
      },

    });
  }

  //Danh sách hiển thị khi search
  getData() {
    this.spinnerService.show();
    this.param.lang = this.lang
    this.employeeService.getAll(this.pagination, this.param).subscribe(
      {
        next: (res) => {
          this.employee = res.result;
          this.pagination = res.pagination;
          this.spinnerService.hide();
        },
        error: () => {
          this.snotifyService.error(this.translateService.instant('System.Message.LockErrorMsg'), this.translateService.instant('System.Caption.Error'));
          this.spinnerService.hide();
        }
      })
  }

  onKeyUpEmployee() {
    this.searchSubject.next(this.param.keyword);
  }

  search() {
    this.pagination.pageNumber == 1 ? this.getData() : this.pagination.pageNumber = 1;
  }
  clearSearch() {
    this.param.keyword = '';
    this.getData();
  }

  pageChanged(event: any) {
    this.pagination.pageNumber = event.page;
    this.getData();
  }
  back(){
    this.employeeService.dataSource.next(this.param)
    this.router.navigateByUrl('/manage');
  }

  //Điều hướng tới trang edit và detail
  changePageEdit(model: ListEmployee) {
    this.employeeService.employeeSource.next(model);//Gửi đi
    this.employeeService.dataSource.next(this.param)
    this.router.navigate([`/manage/employee/edit/${model.empID}`]);
  }

  changePageDetail(model: ListEmployee) {
    this.router.navigate([`/manage/employee/detail/${model.empID}/empList`]).then(
      () => {
        this.employeeService.dataSource.next(this.param)
      },
      (error) => { }
    );
  }

}
