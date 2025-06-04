import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { LangConstants } from '@constants/lang.constants';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { LeaveData } from '@models/common/leave-data';
import { DetailEmployee } from '@models/seahr/edit-leave/detail-employee';
import { SeaEditLeaveService } from '@services/seahr/sea-edit-leave.service';
import { Pagination, PaginationResult } from '@utilities/pagination-utility';
import { OperationResult } from '@utilities/operation-result';
import { CommonConstants } from '@constants/common.constants';
import { DestroyService } from '@services/destroy.service';
import { takeUntil } from 'rxjs';
import { SeahrEditLeaveEmpDetailComponent } from '../seahr-edit-leave-emp-detail/seahr-edit-leave-emp-detail.component';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-seahr-edit-leave',
  templateUrl: './seahr-edit-leave.component.html',
  styleUrls: ['./seahr-edit-leave.component.scss'],
  providers: [DestroyService]
})
export class SeahrEditLeaveComponent extends InjectBase implements OnInit {
  listLeaveData: LeaveData[] = [];
  leaveData: LeaveData = <LeaveData>{};
  detailEmployee: DetailEmployee = <DetailEmployee>{};
  lang: string = localStorage.getItem(LocalStorageConstants.LANG) ?? LangConstants.VN;
  modalRef?: BsModalRef;
  commonConstants: typeof CommonConstants = CommonConstants;
  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 20,
  }
  constructor(
    private service: SeaEditLeaveService,
    private modalService: BsModalService
  ) {
    super();
    const res: PaginationResult<LeaveData> = this.route.snapshot.data['res'];
    this.pagination = res.pagination;
    this.listLeaveData = res.result;
  }

  ngOnInit(): void {
    this.translateService.onLangChange.pipe(takeUntil(this.destroyService.destroys$)).subscribe({
      next: (event) => this.lang = event.lang
    });

  }

  getAllEditLeave() {
    this.spinnerService.show();
    this.service.getAllEditLeave(this.pagination).subscribe({
      next: (res: PaginationResult<LeaveData>) => {
        this.listLeaveData = res.result;
        this.pagination = res.pagination;
      },
      error: (err) => {
        this.spinnerService.hide();
        this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
      },
      complete: () => this.spinnerService.hide()
    });
  }

  pageChanged(event: any) {
    this.pagination.pageNumber = event.page;
    this.getAllEditLeave();
  }

  acceptEditLeave(leaveID: number) {
    this.spinnerService.show();
    this.service.acceptEditLeave(leaveID).subscribe({
      next: (res: OperationResult) => {
        if (res.isSuccess) {
          this.snotifyService.success(this.translateService.instant('System.Message.UpdateOKMsg'), this.translateService.instant('System.Caption.Success'));
          this.getAllEditLeave();
        } else {
          this.snotifyService.success(this.translateService.instant('System.Message.UpdateErrorMsg'), this.translateService.instant('System.Caption.Error'));
        }
      },
      error: (err) => {
        this.spinnerService.hide();
        this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
      },
      complete: () => this.spinnerService.hide()
    });
  }

  rejectEditLeave(leaveID: number) {
    this.spinnerService.show();
    this.service.rejectEditLeave(leaveID).subscribe({
      next: (res: OperationResult) => {
        if (res.isSuccess) {
          this.snotifyService.success(this.translateService.instant('System.Message.UpdateOKMsg'), this.translateService.instant('System.Caption.Success'));
          this.getAllEditLeave();
        } else {
          this.snotifyService.success(this.translateService.instant('System.Message.UpdateErrorMsg'), this.translateService.instant('System.Caption.Error'));
        }
      },
      error: (error) => {
        this.spinnerService.hide();
        this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
      },
      complete: () => this.spinnerService.hide()
    });
  }

  getDetailEmployee(leaveData: LeaveData) {
    this.spinnerService.show();
    this.service.getDetailEmployee(leaveData.empID).subscribe({
      next: (res: DetailEmployee) => {
        this.detailEmployee = res;
        this.detailEmployee.listLeave.forEach(async element => element.translatedComment = await this.functionUtility.changeCommentLanguage(element.comment))
        this.spinnerService.hide();
        const initState: ModalOptions = {
          class: 'modal-lg',
          initialState: {
            leaveData,
            detailEmployee: this.detailEmployee
          }
        };
        this.modalRef = this.modalService.show(SeahrEditLeaveEmpDetailComponent, initState);
      },
      error: (err) => {
        this.spinnerService.hide();
        this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
      }
    });

  }

  back() {
    this.router.navigate(['/seahr']);
  }

  viewDetail(leaveID: number) {
    this.router.navigate(['/leave/detail', leaveID]);
  }
}
