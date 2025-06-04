import { DatePipe } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { TeamManagementData } from '@models/manage/team-management/teamManagementData';
import { TeamManagementService } from '@services/manage/team-management.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { Pagination } from '@utilities/pagination-utility';
import { BsModalService } from 'ngx-bootstrap/modal';
import { CreateOrUpdateComponent } from '../create-or-update/create-or-update.component';
import { Part, PartParam } from '@models/manage/team-management/part';
import { Subject, debounceTime, takeUntil } from 'rxjs';
import { DestroyService } from '@services/destroy.service';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css'],
  providers: [DatePipe, DestroyService]
})
export class MainComponent extends InjectBase implements OnInit {
  data: TeamManagementData[] = [];
  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 20
  };
  listDept: KeyValuePair[] = [];
  clearable: boolean = true;
  partDetail: TeamManagementData = <TeamManagementData>{};
  language: string = localStorage.getItem(LocalStorageConstants.LANG)?.toLowerCase();
  part: Part = <Part>{};
  param: PartParam = <PartParam>{
    deptID : ""
  }
  searchSubject: Subject<string> = new Subject<string>();
  constructor(
    private teamManagementService: TeamManagementService,
    private modalService: BsModalService,
  ) {
    super()
  }

  ngOnInit(): void {
    this.teamManagementService.dataSource.subscribe({
      next: result => {
        if (result) {
          this.param = result;
        }
      }
    })
    this.getAllDepartment();
    this.getData();
    this.translateService.onLangChange.pipe(takeUntil(this.destroyService.destroys$)).subscribe(event => {
      this.language = event.lang;
      this.listDept[0] = { key: '', value: this.translateService.instant('Manage.TeamManager.SelectDept') as string };
    });
    this.teamManagementService.partEmitter.pipe(takeUntil(this.destroyService.destroys$)).subscribe((res) => {
      if (res)
        this.search();
    })
    this.searchSubject.pipe(debounceTime(500)).subscribe(() => {
      this.search();
    });
  }

  onKeyUpPartCode() {
    this.searchSubject.next(this.param.partCode);
  }

  back() {
    this.teamManagementService.dataSource.next(this.param)
    this.router.navigate(['/manage']);
  }

  search() {
    this.pagination.pageNumber === 1 ? this.getData() : this.pagination.pageNumber = 1;
  }

  refreshSearch() {
    this.param.deptID = '';
    this.param.partCode = '';
    this.search();
  }

  getData() {
    this.clearable = this.param.deptID === '' ? false : true;
    this.spinnerService.show();
    this.teamManagementService.getData(this.pagination, this.param)
      .subscribe({
        next: (res) => {
          this.data = res.result;
          this.pagination = res.pagination;
          this.spinnerService.hide();
        }, error: () => {
          this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
          this.spinnerService.hide();
        }
      })
  }

  getAllDepartment() {
    this.spinnerService.show();
    this.teamManagementService.getAllDepartment()
      .subscribe({
        next: (res) => {
          this.listDept = res;
          this.listDept.unshift({ key: '', value: this.translateService.instant('Manage.TeamManager.SelectDept') as string });
          this.spinnerService.hide();
        }, error: () => {
          this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
          this.spinnerService.hide();
        }
      })
  }

  pageChanged(event: any) {
    this.pagination.pageNumber = event.page;
    this.getData();
  }

  exportExcel() {
    this.initLang()
    this.spinnerService.show();
    this.teamManagementService.exportExcel(this.pagination, this.param)
      .subscribe({
        next: (result) => {
          this.spinnerService.hide();
          result.isSuccess ? this.functionUtility.exportExcel(result.data, 'PartDepartment')
            : this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
        },
        error: () => {
          this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
          this.spinnerService.hide();
        }
      });
  }

  private initLang(): void {
    this.param.label_PartName = this.translateService.instant('Manage.TeamManager.PartName');
    this.param.label_PartCode = this.translateService.instant('Manage.TeamManager.PartCode');
    this.param.label_Number = this.translateService.instant('Manage.TeamManager.Number');
  }
  add() {
    this.part = <Part>{
      number: null,
      partID: 0,
      visible: true,
      deptID: '',
      type: 'CREATE'
    };
    this.teamManagementService.changeData(this.part);
    this.modalService.show(CreateOrUpdateComponent);
  }

  edit(item: TeamManagementData) {
    this.spinnerService.show();
    this.teamManagementService.getDataDetail(item.partID)
      .subscribe({
        next: (res) => {
          this.part = res;
          this.part.type = "UPDATE";
          this.spinnerService.hide();
          this.teamManagementService.changeData(this.part);
          this.modalService.show(CreateOrUpdateComponent);
        }, error: () => {
          this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
          this.spinnerService.hide();
        }
      })
  }

  detail(item: TeamManagementData) {
    this.partDetail = { ...item };
  }
}
