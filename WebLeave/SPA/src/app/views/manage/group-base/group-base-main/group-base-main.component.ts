import { Component, OnInit, TemplateRef } from "@angular/core";
import { CommonConstants, IconButton } from "@constants/common.constants";
import { LocalStorageConstants } from "@constants/local-storage.enum";
import { ActionConstants, MessageConstants } from "@constants/message.enum";
import { GroupBaseAndGroupLangDto, GroupBaseTitleExcel } from "@models/manage/group-base/group-base-and-group-lang";
import { GroupBase } from "@models/seahr/groupBase";
import { DestroyService } from "@services/destroy.service";
import { GroupBaseService } from "@services/manage/group-base.service";
import { InjectBase } from "@utilities/inject-base-app";
import { time } from "console";
import { BsModalRef, BsModalService } from "ngx-bootstrap/modal";
import { takeUntil } from 'rxjs';

@Component({
  selector: 'app-group-base-main',
  templateUrl: './group-base-main.component.html',
  styleUrls: ['./group-base-main.component.scss'],
  providers: [DestroyService]
})
export class GroupBaseMainComponent extends InjectBase implements OnInit {
  listGroupBaseManage: GroupBaseAndGroupLangDto[] = [];
  addGroupBase: GroupBaseAndGroupLangDto = <GroupBaseAndGroupLangDto>{};
  editGroupBase: GroupBaseAndGroupLangDto = <GroupBaseAndGroupLangDto>{};
  _addGroupBase: GroupBaseAndGroupLangDto = <GroupBaseAndGroupLangDto>{}
  _editGroupBase: GroupBaseAndGroupLangDto = <GroupBaseAndGroupLangDto>{}

  lang: string = 'vi';

  modalRef?: BsModalRef;

  commonConstants: typeof CommonConstants = CommonConstants;

  constructor(
    private groupBaseManageService: GroupBaseService,
    private modalService: BsModalService,
  ) {
    super();
  }

  ngOnInit(): void {
    this.getData();
    this.lang = localStorage.getItem(LocalStorageConstants.LANG);
    this.translateService.onLangChange.pipe(takeUntil(this.destroyService.destroys$)).subscribe((event) => {
      this.lang = event.lang;
    })
  }

  //Get Data
  getData() {
    this.spinnerService.show();
    this.groupBaseManageService.getGroupBase().subscribe({
      next: (res) => {
        this.listGroupBaseManage = res;
        this.spinnerService.hide();
      },
      error: () => {
        this.spinnerService.hide();
        this.snotifyService.error(this.translateService.instant('System.Message.Nodata'), this.translateService.instant('System.Caption.Error'))
      }
    });
  }
  titleExcel: GroupBaseTitleExcel = <GroupBaseTitleExcel>{}
  //Export Excel File
  // change Excel1
  exportExcel() {
    this.initLang()
    this.spinnerService.show();
    this.groupBaseManageService.exportExcel(this.titleExcel).subscribe({
      next: (result) => {
        this.spinnerService.hide();
        result.isSuccess ? this.functionUtility.exportExcel(result.data, 'Group_Base')
          : this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
      },
      error: () => {
        this.snotifyService.error(this.translateService.instant('System.Message.Nodata'), this.translateService.instant('System.Caption.Error'))
        this.spinnerService.hide();
      },
    });
  }

  private initLang(): void {
    this.titleExcel.label_BaseName = this.translateService.instant('GroupBase.BaseName');
    this.titleExcel.label_BaseSym = this.translateService.instant('GroupBase.BaseSym');
  }

  //Add
  openModalAdd(templateAdd: TemplateRef<any>) {
    this.modalRef = this.modalService.show(templateAdd);
  }

  saveAdd() {
    this.spinnerService.show();
    this.groupBaseManageService.addGroup(this.addGroupBase).subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.spinnerService.hide();
          this.snotifyService.success(this.translateService.instant('System.Message.CreateOKMsg'),
            this.translateService.instant('System.Caption.Success'));
          this.modalRef?.hide();
          this.resetAdd();
          this.getData();
        }
        else {
          this.spinnerService.hide();
          this.resetAdd();
          this.snotifyService.error(this.translateService.instant('System.Message.CreateErrorMsg'),
            this.translateService.instant('System.Caption.Error'));
        }
      },
      error: () => {
        this.spinnerService.hide();
        this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
      }
    });
  }

  resetAdd() {
    this.addGroupBase = { ...this._addGroupBase };
  }

  //Edit
  openModalEdit(templateEdit: TemplateRef<any>, item: GroupBaseAndGroupLangDto) {
    this.groupBaseManageService.changeModelDetail(item);
    this.groupBaseManageService.getDetailGroupBase(item.gbid).subscribe(res => {
      this.editGroupBase = res;
      this._editGroupBase = { ...this.editGroupBase };
    })
    this.modalRef = this.modalService.show(templateEdit);
  }

  saveEdit() {
    this.spinnerService.show();
    this.groupBaseManageService.editGroup(this.editGroupBase).subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.spinnerService.hide();
          this.snotifyService.success(this.translateService.instant('System.Message.UpdateOKMsg'),
            this.translateService.instant('System.Caption.Success'));
          this.modalRef?.hide();
          this.getData();
        }
        else {
          this.spinnerService.hide();
          this.snotifyService.error(this.translateService.instant('System.Message.UpdateErrorMsg'),
            this.translateService.instant('System.Caption.Error'));
        }
      },
      error: () => {
        this.spinnerService.hide();
        this.snotifyService.error(this.translateService.instant('System.Message.UpdateErrorMsg'), this.translateService.instant('System.Caption.Error'));
      }
    })
  }

  resetEdit() {
    this.editGroupBase = { ...this._editGroupBase };
  }
  removeGroup(item: GroupBase) {
    this.snotifyService.confirm(MessageConstants.CONFIRM_DELETE, ActionConstants.DELETE, () => {
      this.groupBaseManageService.removeGroup(item.gbid).subscribe(res => {
        if (res.isSuccess) {
          this.getData();
          this.snotifyService.success(this.translateService.instant('System.Message.DeleteOKMsg'), this.translateService.instant('System.Caption.Success'));
        } else {
          this.snotifyService.error(this.translateService.instant('System.Message.DeleteErrorMsg'), this.translateService.instant('System.Caption.Error'));
        }
      });
    })
  }
}
