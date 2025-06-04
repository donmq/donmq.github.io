import { Component, OnInit, TemplateRef, ViewChild, ViewContainerRef } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';
import { firstValueFrom, take, takeUntil } from 'rxjs';
import { CommonConstants } from '@constants/common.constants';
import { LangConstants } from '@constants/lang.constants';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { CommentArchive } from '@models/common/comment-archive';
import { LeaveData } from '@models/common/leave-data';
import { SeaConfirmEmpDetail } from '@models/seahr/sea-confirm-emp-detail';
import { SeaConfirmParam } from '@params/seahr/sea-confirm.param';
import { SeaConfirmService } from '@services/seahr/sea-confirm.service';
import { Pagination } from '@utilities/pagination-utility';
import { DestroyService } from '@services/destroy.service';
import { KeyValueUtility } from '@utilities/key-value-utility';
import { SeahrConfirmEmpDetailComponent } from '../seahr-confirm-emp-detail/seahr-confirm-emp-detail.component';
import { bsDatepickerConfig } from '@constants/bs-config.enum';
import { SeaConfirmResolverDto } from '@models/seahr/sea-confirm';
import { InjectBase } from '@utilities/inject-base-app';
@Component({
  selector: 'app-seahr-confirm',
  templateUrl: './seahr-confirm.component.html',
  styleUrls: ['./seahr-confirm.component.scss'],
  providers: [DestroyService]
})
export class SeahrConfirmComponent extends InjectBase implements OnInit {
  commentArchives: CommentArchive[] = [];
  leaveData: LeaveData[] = [];
  countEachCategory: KeyValueUtility[] = [];
  empDetail: SeaConfirmEmpDetail = <SeaConfirmEmpDetail>{};
  leaveItem: LeaveData = <LeaveData>{};
  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 100
  };
  date = new Date();
  param: SeaConfirmParam = <SeaConfirmParam>{
    cateID: 0
  };
  categories: KeyValueUtility[] = [];
  departments: KeyValueUtility[] = [];
  parts: KeyValueUtility[] = [];
  selectedLeaveData: LeaveData[] = [];
  bsModalRef?: BsModalRef;
  lang: string = localStorage.getItem(LocalStorageConstants.LANG) ?? LangConstants.VN;
  userRank: string = JSON.parse(localStorage.getItem(LocalStorageConstants.USER)).userRank;
  bsDatepickerConfig: Partial<BsDatepickerConfig> = bsDatepickerConfig;
  checkboxAll: boolean = false;
  commonConstants: typeof CommonConstants = CommonConstants;
  fromDateDate: Date = new Date();
  toDateDate: Date = new Date();
  constructor(
    private viewContainerRef: ViewContainerRef,
    private seaConfirmService: SeaConfirmService,
    private modalService: BsModalService
  ) {
    super();
  }

  ngOnInit() {
    this.seaConfirmService.dataSource.pipe(take(1)).subscribe({
      next: result => {
        if (result) {
          this.param = result;
          this.fromDateDate = this.param.fromDate == null ? null : new Date(this.param.fromDate);
          this.toDateDate = this.param.toDate == null ? null : new Date(this.param.toDate);
        }
        else {
          this.fromDateDate = this.fromDateDate.toFirstDateOfMonth();
          this.toDateDate = this.toDateDate.toLastDateOfMonth();
        }
        this.getData();
      }
    })
    const res: SeaConfirmResolverDto = this.route.snapshot.data['res'];
    this.categories = res.category_List;
    this.categories.unshift({
      key: 0,
      value_en: 'All types of leave',
      value_vi: 'Tất cả loại phép',
      value_zh: '所有类型的休假'
    });
    this.departments = res.department_List
    this.commentArchives = res.comment_List
    if (this.param.deptID != null)
      this.getParts(this.param.deptID)
    this.commentArchives.forEach(item => item.text = `${item.value}. ${item.comment}`);
    this.commentArchives.unshift({ value: 0, text: 'None' } as CommentArchive)
    this.translateService.onLangChange.pipe(takeUntil(this.destroyService.destroys$)).subscribe(async res => this.lang = res.lang);
  }


  onDropdownOpen(id: string) {
    setTimeout(() => {
      const selectElement = document.getElementById(id);

      if (selectElement) {
        // Tính toán vị trí của dropdown
        const rect = selectElement.getBoundingClientRect();

        const dropdown = document.querySelector('.ng-dropdown-panel.custom-select-dropdown') as HTMLElement;
        if (dropdown) {
          // Lấy thông tin độ rộng của view
          const viewportWidth = document.documentElement.clientWidth;
          // Tính toán right cho dropdown-panel
          const rightPosition = viewportWidth - rect.right;

          dropdown.style.right = `${rightPosition}px`;
        }
      }
    });
  }

  back() {
    this.seaConfirmService.dataSource.next(this.param)
    this.router.navigate(['/seahr']);
  }
  getParts(deptID: number) {
    this.seaConfirmService.getParts(deptID)
      .subscribe({
        next: (res) => {
          this.parts = res
        },
        error: () => {
          this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
        }
      });
  }

  departmentChanged() {
    this.param.partID = null;
    if (!this.functionUtility.checkEmpty(this.param.deptID))
      this.getParts(this.param.deptID);
  }
  deleteProperty(name: string) {
    delete this.param[name]
  }
  getData() {
    if (this.functionUtility.checkEmpty(this.param.leaveDay))
      this.deleteProperty('leaveDay')

    if (!this.functionUtility.checkEmpty(this.fromDateDate))
      this.param.fromDate = this.functionUtility.getDateFormat(this.fromDateDate);
    else
      this.deleteProperty('fromDate')

    if (!this.functionUtility.checkEmpty(this.toDateDate))
      this.param.toDate = this.functionUtility.getDateFormat(this.toDateDate);
    else
      this.deleteProperty('toDate')

    if (this.param.fromDate > this.param.toDate) {
      this.snotifyService.warning(this.translateService.instant('System.Message.CompareDate'),
        this.translateService.instant('System.Caption.Warning'));
      return;
    }
    this.spinnerService.show();
    this.seaConfirmService.search(this.param, this.pagination)
      .subscribe({
        next: (res) => {
          this.spinnerService.hide();
          this.pagination = res.leaveData.pagination;
          let result: LeaveData[] = res.leaveData.result;
          this.countEachCategory = res.countEachCategory;
          if (this.checkboxAll) {
            // Trường hợp có ChecboxAll được chọn thì chọn tất cả phần tử
            result.forEach(item => item.checkbox = true);
          } else if (this.selectedLeaveData.length > 0) {
            // Nếu leaveID thuộc trong danh sách selectedLeaveData thì chọn leaveID đó
            result.forEach(item => item.checkbox = this.selectedLeaveData.some(data => data.leaveID === item.leaveID));
          }
          this.leaveData = result;
        },
        error: () => {
          this.spinnerService.hide();
          this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
        }
      })
  }

  @ViewChild('tooltipTemplate', { read: TemplateRef }) tooltipTemplate!: TemplateRef<any>;
  //#region Hover
  startHover() {
    this.viewContainerRef.createEmbeddedView(this.tooltipTemplate);
  }

  cancelHover() {
    this.closeModal();
  }

  closeModal() {
    this.viewContainerRef.clear();
  }
  //#endregion

  search() {
    this.pagination.pageNumber = 1;
    this.getData();
  }

  pageChanged(event: PageChangedEvent) {
    this.pagination.pageNumber = event.page;
    this.getData();
  }

  resetSearch(isResetParam: boolean = true) {
    if (isResetParam) {
      this.param = <SeaConfirmParam>{};
      this.fromDateDate = this.fromDateDate.toFirstDateOfMonth();
      this.toDateDate = this.toDateDate.toLastDateOfMonth();
    }
    this.empDetail = <SeaConfirmEmpDetail>{};
    this.leaveItem = <LeaveData>{};
    this.pagination.pageNumber = 1;
    this.selectedLeaveData = [];
    this.parts = [];
    this.checkboxAll = false;
    this.getData();
  }

  async empNumberClicked(item: LeaveData, isShowModal: boolean, type: string): Promise<void> {
    this.leaveItem = item;
    if (isShowModal)
      this.empDetail = await firstValueFrom(this.seaConfirmService.getEmpDetail(item.empID));
    else
      this.empDetail = await firstValueFrom(this.seaConfirmService.getLeaveDeleteTopFive(item.empID));

    this.empDetail.leaveData.forEach(async item => item.translatedComment = await this.functionUtility.changeCommentLanguage(item.comment));

    const initialState: ModalOptions = {
      class: 'modal-lg',
      initialState: {
        leaveItem: this.leaveItem,
        empDetail: this.empDetail
      }
    };

    if (isShowModal) {
      this.bsModalRef = this.modalService.show(SeahrConfirmEmpDetailComponent, initialState);
    }
  }
  leaveDetail(item: any) {
    this.router.navigate(['/leave/detail', item.leaveID]).then(
      () => {
        this.seaConfirmService.dataSource.next(this.param);
      },
      () => { }
    );
  }

  confirmLeaveData() {
    this.selectedLeaveData = [];
    if (!this.checkboxAll && !this.leaveData.some(item => item.checkbox)) {
      return this.snotifyService.warning(this.translateService.instant('System.Message.SelectRecord'), this.translateService.instant('System.Caption.Warning'));
    } else {
      this.selectedLeaveData = this.leaveData.filter(x => x.checkbox);
      if (this.selectedLeaveData.length < 20) {
        this.submitConfirm();
      }
      else {
        this.snotifyService.confirm(this.translateService.instant('System.Message.ChangeState'), this.translateService.instant('System.Caption.Confirm'), async () => {
          this.submitConfirm();
        })
      }
    }
  }

  submitConfirm() {
    // const body: SeaConfirm = {
    //   listLeaveData: this.selectedLeaveData
    // }

    this.spinnerService.show();
    this.seaConfirmService.confirm(this.selectedLeaveData)
      .subscribe({
        next: (res) => {
          this.spinnerService.hide();
          if (res.isSuccess) {
            this.snotifyService.success(this.translateService.instant('System.Message.UpdateOKMsg'), this.translateService.instant('System.Caption.Success'));
            this.resetSearch(false);
          } else {
            this.snotifyService.error(this.translateService.instant('System.Message.UpdateErrorMsg'), this.translateService.instant('System.Caption.Error'));
          }
        },
        error: () => {
          this.spinnerService.hide();
          this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
        }
      })
  }

  commentArchiveChanged(item: LeaveData) {
    this.selectedLeaveData = this.selectedLeaveData.filter(data => data.leaveID !== item.leaveID);
  }

  exportExcel() {
    this.initLang();
    this.spinnerService.show();
    this.seaConfirmService.exportExcel(this.param, this.pagination)
      .subscribe({
        next: (res) => {
          this.spinnerService.hide();
          res.isSuccess ? this.functionUtility.exportExcel(res.data, 'SEAHR_SEAConfirm')
            : this.snotifyService.error(this.translateService.instant('System.Message.SystemError'),
              this.translateService.instant('System.Caption.Error'));
        },
        error: () => {
          this.spinnerService.hide();
          this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
        }
      })
  }

  onKeyUpEmpNumber() {
    if (this.param.empNumber && this.param.empNumber.length === 5)
      this.search();
  }

  checkboxAllChanged(): void {
    this.leaveData.forEach(item => { item.checkbox = this.checkboxAll });
    console.log(this.leaveData)
  }

  selectItem() {
    this.checkboxAll = this.leaveData.every(x => x.checkbox);
  }

  private initLang(): void {
    this.param.label_PartCode = this.translateService.instant('SeaHr.SeaHrSeaConfirm.PartCode');
    this.param.label_DeptName = this.translateService.instant('SeaHr.SeaHrSeaConfirm.DeptName');
    this.param.label_Employee = this.translateService.instant('SeaHr.SeaHrSeaConfirm.EmpName');
    this.param.label_NumberID = this.translateService.instant('SeaHr.SeaHrSeaConfirm.EmpNumber');
    this.param.label_Category = this.translateService.instant('SeaHr.SeaHrSeaConfirm.Category');
    this.param.label_TimeStart = this.translateService.instant('SeaHr.SeaHrSeaConfirm.TimeStart');
    this.param.label_DateStart = this.translateService.instant('SeaHr.SeaHrSeaConfirm.DateStart');
    this.param.label_TimeEnd = this.translateService.instant('SeaHr.SeaHrSeaConfirm.TimeEnd');
    this.param.label_DateEnd = this.translateService.instant('SeaHr.SeaHrSeaConfirm.DateEnd');
    this.param.label_LeaveDay = this.translateService.instant('SeaHr.SeaHrSeaConfirm.LeaveDay');
    this.param.label_Status = this.translateService.instant('SeaHr.SeaHrSeaConfirm.Status');
    this.param.label_UpdateTime = this.translateService.instant('SeaHr.SeaHrSeaConfirm.UpdateTime');
    this.param.status1 = this.translateService.instant('Common.Status1');
    this.param.status2 = this.translateService.instant('Common.Status2');
    this.param.status3 = this.translateService.instant('Common.Status3');
    this.param.status4 = this.translateService.instant('Common.Status4');
  }
}


