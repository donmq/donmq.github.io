
import { Component, OnInit, TemplateRef } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { CommentArchive } from '@models/common/comment-archive';
import { ManageCommentArchiveService } from '@services/seahr/manage-comment-archive.service';
import { OperationResult } from '@utilities/operation-result';
import { Pagination } from '@utilities/pagination-utility';
import { CommonConstants, IconButton } from '@constants/common.constants';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { DestroyService } from '@services/destroy.service';
import { takeUntil } from 'rxjs';
import { LangConstants } from '@constants/lang.constants';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-manage-comment-archive',
  templateUrl: './manage-comment-archive.component.html',
  styleUrls: ['./manage-comment-archive.component.scss'],
  providers: [DestroyService]
})
export class ManageCommentArchiveComponent extends InjectBase implements OnInit {
  commentData: CommentArchive[] = [];
  commentDelete: CommentArchive;
  modalRef?: BsModalRef;
  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 20
  };
  lang: string = localStorage.getItem(LocalStorageConstants.LANG) ?? LangConstants.VN;
  id: number;
  commentArchiveDataAdd: CommentArchive = {} as CommentArchive;
  commonConstants: typeof CommonConstants = CommonConstants;

  constructor(
    private commentArchiveService: ManageCommentArchiveService,
    private modalService: BsModalService,
  ) {
    super()
  }

  ngOnInit(): void {

    this.getData();
    this.translateService.onLangChange.pipe(takeUntil(this.destroyService.destroys$)).subscribe(async res => {
      this.lang = res.lang;
      this.getData();
    });
  }

  //List Data
  getData() {
    this.spinnerService.show();
    this.commentArchiveService.getAll(this.pagination)
      .subscribe({
        next: (res: any) => {
          this.commentData = res.result;
          this.pagination = res.pagination;
          this.spinnerService.hide();
        },
        error: () => {
          this.snotifyService.error(this.translateService.instant('System.Message.LockErrorMsg'), this.translateService.instant('System.Caption.Error'));
          this.spinnerService.hide();
        }
      })
  }

  pageChanged(event: any) {
    this.pagination.pageNumber = event.page;
    this.getData();
  }

  //Delete
  delete(id: number) {
    this.spinnerService.show();
    this.commentArchiveService.deleteComment(id).subscribe({
      next: (res: OperationResult) => {
        this.spinnerService.show();
        if (res.isSuccess) {
          this.snotifyService.success(this.translateService.instant('System.Message.DeleteOKMsg'), this.translateService.instant('System.Caption.Success'));
          this.modalRef.hide();
        } else {
          this.snotifyService.error(this.translateService.instant('System.Message.DeleteErrorMsg'), this.translateService.instant('System.Caption.Error'));
          this.spinnerService.show();
          this.modalRef.hide();
        }
        this.getData();
      }
    });

  }

  deleteItem(id: number) {
    this.snotifyService.confirm('Are you sure you want to delete the data ? ', 'Delete',
      () => {
        this.delete(id)
      }), () => {
        this.modalRef.hide()
      }
  }

  //Add
  refresh() {
    this.commentArchiveDataAdd = {} as CommentArchive;
    this.commentArchiveDataAdd.value = null;
    this.commentArchiveDataAdd.comment = '';
  }

  openModalAdd(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  add() {
    this.spinnerService.show();
    this.commentArchiveService.addComment(this.commentArchiveDataAdd).subscribe({
      next: (res) => {
        this.spinnerService.hide();

        if (res.isSuccess) {
          this.snotifyService.success(this.translateService.instant('System.Message.CreateOKMsg'), this.translateService.instant('System.Caption.Success'));
          this.getData();
          this.modalRef.hide();
          this.refresh();
        }
        else {
          this.snotifyService.error(this.translateService.instant('System.Message.CreateErrorMsg'), this.translateService.instant('System.Caption.Error'));
          if (res.error) {
          }
          this.refresh();
        }
      },
      error: (e) => {
        this.snotifyService.error(this.translateService.instant('System.Message.CreateErrorMsg'), this.translateService.instant('System.Caption.Error'));
        this.spinnerService.hide();
      }
    });
  }
}
