import { LeaveSendNotiUser } from '@models/leave/leaveSendNotiUser';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { LeaveDetail } from '@models/common/leave-data';
import { LeaveEditApproval } from '@models/leave/leaveEditApproval';
import { LeaveEditCommentArchive } from '@models/leave/leaveEditCommentArchive';
import { catchError, firstValueFrom, of, takeUntil, tap } from 'rxjs';
import { CommonConstants } from '@constants/common.constants';
import { LangConstants } from '@constants/lang.constants';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { Users } from '@models/auth/users.model';
import { CommentArchive } from '@models/common/comment-archive';
import { LeaveDetailService } from '@services/leave/leave-detail.service';
import { Location } from '@angular/common';
import { DestroyService } from '@services/destroy.service';
import { InjectBase } from '@utilities/inject-base-app';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-leave-detail',
  templateUrl: './leave-detail.component.html',
  styleUrls: ['./leave-detail.component.scss'],
  providers: [DestroyService]
})
export class LeaveDetailComponent extends InjectBase implements OnInit {
  detail: LeaveDetail = <LeaveDetail>{}
  langConstants: typeof LangConstants = LangConstants;
  commonConstants: typeof CommonConstants = CommonConstants;
  lang: string = localStorage.getItem(LocalStorageConstants.LANG);
  user: Users = JSON.parse(localStorage.getItem(LocalStorageConstants.USER));
  userRank: string = JSON.parse(localStorage.getItem(LocalStorageConstants.USER)).userRank;
  isBtnRequestValid: boolean = true;
  commentArchives: CommentArchive[] = [];
  commentLeave: number = 0;
  notiText: string = '';
  slEditApproval: number = 0;
  sendNoti: LeaveSendNotiUser = <LeaveSendNotiUser>{}
  leaveEditCommentArchive: LeaveEditCommentArchive = <LeaveEditCommentArchive>{}
  leaveEditApproval: LeaveEditApproval = <LeaveEditApproval>{};
  modalRef?: BsModalRef;
  listComment: string[] = [];

  constructor(
    private location: Location,
    private leaveDetailService: LeaveDetailService,
    private modalService: BsModalService,
  ) {
    super();
  }

  async ngOnInit(): Promise<void> {
    this.detail = this.route.snapshot.data['res'];

    await this.changeDetailLanguage();
    await this.getCommentArchives();

    this.translateService.onLangChange.pipe(takeUntil(this.destroyService.destroys$)).subscribe(async res => {
      this.lang = res.lang === 'zh' ? LangConstants.ZH_TW : res.lang;
      await this.changeDetailLanguage();
    });

    this.leaveEditCommentArchive = {
      commentArchiveID: this.commentLeave,
      leaveID: this.detail.leaveData.leaveID
    }

    this.commentArchives.forEach(item => item.text = `${item.value}. ${item.comment}`);

    this.leaveEditApproval = {
      empId: this.detail.leaveData.empID,
      leaveID: this.detail.leaveData.leaveID,
      notiText: this.notiText,
      slEditApproval: this.slEditApproval
    };
  }

  async getCommentArchives() {
    this.commentArchives = await firstValueFrom(this.commonService.getCommentArchives().pipe(
      catchError(err => {
        this.snotifyService.error(
          this.translateService.instant('System.Message.UnknowError'),
          this.translateService.instant('System.Caption.Error')
        );
        return of(null);
      })
    ));
  }
  async getLeaveData() {
    this.detail = await firstValueFrom(
      this.leaveDetailService.getLeaveDetail(this.detail.leaveData.leaveID).pipe(
        tap(async res => {
          res.leaveData.translatedComment = await this.functionUtility.changeCommentLanguage(res.leaveData.comment);
          res.deletedLeaves.forEach(async item => {
            item.translatedComment = await this.functionUtility.changeCommentLanguage(item.comment)
          });
        }),
        catchError(err => {
          this.snotifyService.error(
            this.translateService.instant('System.Message.UnknowError'),
            this.translateService.instant('System.Caption.Error')
          );
          return of(null);
        })));
  }

  async requestEditLeave() {
    this.spinnerService.show();
    await firstValueFrom(
      this.leaveDetailService.editRequestLeave(this.detail.leaveData.leaveID, this.detail.leaveData.reasonAdjust).pipe(
        tap(async res => {
          if (res.isSuccess) {
            await this.getLeaveData()
            await this.changeDetailLanguage()
            this.closeModal();
          }
          this.spinnerService.hide();
        }),
        catchError(err => {
          this.spinnerService.hide();
          return of(null);
        })
      ));
  }

  async changeDetailLanguage() {
    this.detail.leaveData.translatedComment = await this.functionUtility.changeCommentLanguage(this.detail.leaveData.comment);
    this.detail.deletedLeaves.forEach(async item => {
      item.translatedComment = await this.functionUtility.changeCommentLanguage(item.comment)
    });
    this.listComment = this.detail.leaveData.translatedComment.split('-').map(x => x.includes(',') ? x.replace(',', ' -') : x);
  }

  async editApproval() {
    this.spinnerService.show();
    await firstValueFrom(this.leaveDetailService.editApproval(this.leaveEditApproval)
      .pipe(
        tap(async res => {
          this.spinnerService.hide();
          if (res.isSuccess) {
            this.snotifyService.success(this.translateService.instant('System.Message.UpdateOKMsg'), this.translateService.instant('System.Caption.Success'));
            this.back()
            await this.getLeaveData()
          } else {
            this.snotifyService.error(this.translateService.instant('System.Message.UpdateErrorMsg'), this.translateService.instant('System.Caption.Error'));
          }
        }),
        catchError(err => {
          this.spinnerService.hide();
          this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
          return of(null);
        })
      ));
  }

  async editCommentArchive() {
    if (this.leaveEditCommentArchive.commentArchiveID == 0) {

      this.snotifyService.warning(this.translateService.instant('System.Message.SelctNote'),
        this.translateService.instant('System.Caption.Warning'));

      return;
    };

    this.spinnerService.show();
    await firstValueFrom(this.leaveDetailService.editCommentArchive(this.leaveEditCommentArchive)
      .pipe(tap(async res => {
        this.spinnerService.hide();
        if (res.isSuccess) {
          this.snotifyService.success(this.translateService.instant('System.Message.UpdateOKMsg'), this.translateService.instant('System.Caption.Success'));
          this.leaveEditApproval = <LeaveEditApproval>{};
          await this.getLeaveData();
          this.leaveEditCommentArchive.commentArchiveID = null;
        } else {
          this.snotifyService.error(this.translateService.instant('System.Message.UpdateErrorMsg'), this.translateService.instant('System.Caption.Error'));
        }

      }), catchError(err => {
        this.spinnerService.hide();
        this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
        return of(null);
      }))
    )
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template, { backdrop: 'static', keyboard: false });
  }
  closeModal() {
    this.detail.leaveData.reasonAdjust = ""
    this.modalRef?.hide()
  }

  back() {
    this.location.back();
  }

}
