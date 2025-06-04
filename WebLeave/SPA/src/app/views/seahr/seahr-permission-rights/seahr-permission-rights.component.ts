import { Component, OnInit } from '@angular/core';
import { CaptionConstants, MessageConstants } from '@constants/message.enum';
import { PermissionParam, PermissionRightsDTO } from '@models/seahr/permissionRightsDTO';
import { InjectBase } from '@utilities/inject-base-app';
import { Pagination } from '@utilities/pagination-utility';
import { SeahrPermissionRightsService } from '../../../_core/services/seahr/seahr-permission-rights.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { DestroyService } from '@services/destroy.service';
import { CommonConstants } from '@constants/common.constants';
import { Subject, debounceTime } from 'rxjs';

@Component({
  selector: 'app-seahr-permission-rights',
  templateUrl: './seahr-permission-rights.component.html',
  styleUrls: ['./seahr-permission-rights.component.scss'],
  providers: [DestroyService]
})
export class SeahrPermissionRightsComponent extends InjectBase implements OnInit {

  data: PermissionRightsDTO[] = []
  commonConstants = CommonConstants;
  param: PermissionParam = <PermissionParam>{
    partID: 0
  }
  searchSubject: Subject<string> = new Subject<string>();
  parts: KeyValuePair[] = []
  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 20,
  };
  constructor(
    private service: SeahrPermissionRightsService,
  ) {
    super();
  }

  ngOnInit(): void {
    this.getPart();
    this.service.dataSource.subscribe({
      next: result => {
        if (result) {
          this.param = result;
        }
      }
    })
    this.searchSubject.pipe(debounceTime(500)).subscribe(() => {
      this.search();
    });
  }

  getData() {
    this.spinnerService.show();
    this.service.getDataPagination(this.pagination, this.param).subscribe({
      next: res => {
        this.data = res.result
        this.pagination = res.pagination;
        this.spinnerService.hide()
      }, error: () => {
        this.spinnerService.hide()
        this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
      }
    })
  }

  getPart() {
    this.spinnerService.show();
    this.service.getPart().subscribe({
      next: (res) => {
        this.spinnerService.hide();
        this.parts = res;
        this.getData();
      },
      error: () => {
        this.spinnerService.hide();
        this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
      }
    });
  }

  onKeyUpEmpNumber() {
    this.searchSubject.next(this.param.empNumber);
  }

  clear() {
    this.param.partID = undefined
    this.param.empNumber = ''
    this.data = []
    this.pagination.totalPage = 0
  }
  search() {
    if (this.functionUtility.checkEmpty(this.param.empNumber) && (this.param.partID == undefined)) {
      return this.snotifyService.warning(this.translateService.instant('System.Message.SearchCondition'), this.translateService.instant('System.Caption.Warning'))
    }
    this.pagination.pageNumber === 1 ? this.getData() : this.pagination.pageNumber = 1;
  }

  exportExcel() {
    this.initLang()
    this.spinnerService.show()
    this.service.exportExcel(this.param).subscribe({
      next: res => {
        this.spinnerService.hide()
        this.functionUtility.exportExcel(res.data, "Permission Rights")
      }, error: () => {
        this.spinnerService.hide()
        this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
      }
    })
  }
  private initLang(): void {
    this.param.label_Stt = this.translateService.instant('SeaHr.Permission.STT');
    this.param.label_EmpNumber = this.translateService.instant('SeaHr.Permission.EmpNumber');
    this.param.label_EmpName = this.translateService.instant('SeaHr.Permission.EmpName');
    this.param.label_PositionName = this.translateService.instant('SeaHr.Permission.PositionName');
    this.param.label_Part = this.translateService.instant('SeaHr.Permission.Part');
    this.param.label_ApprovalUsers = this.translateService.instant('SeaHr.Permission.Approval');
  }


  pageChanged(event) {
    this.pagination.pageNumber = event.page;
    this.getData();
  }

  back(){
    this.service.dataSource.next(this.param)
    this.router.navigate(['/seahr/']);
  }

}
