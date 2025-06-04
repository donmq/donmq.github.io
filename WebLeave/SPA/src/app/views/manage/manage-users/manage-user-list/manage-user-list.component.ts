import { Component, OnInit } from '@angular/core';
import { BsModalService, BsModalRef, ModalOptions } from 'ngx-bootstrap/modal';
import { PageChangedEvent } from 'ngx-bootstrap/pagination';
import { Pagination } from '@utilities/pagination-utility';
import { Users } from '@models/manage/user-manage/Users';
import { UserManageService } from '@services/manage/user-manage.service';
import { ManageUserAddComponent } from '../manage-user-add/manage-user-add.component';
import { ManageUserEditComponent } from '../manage-user-edit/manage-user-edit.component';
import { InjectBase } from '@utilities/inject-base-app';
import { Subject, debounceTime, takeUntil } from 'rxjs';
import { UserManageParam, UserManageTitleExcel } from '@params/manage/user-manage-param';
import { LocalStorageConstants } from '@constants/local-storage.enum';

@Component({
  selector: 'app-manage-user-list',
  templateUrl: './manage-user-list.component.html',
  styleUrls: ['./manage-user-list.component.css']
})
export class ManageUserListComponent extends InjectBase implements OnInit {
  bsModalRef?: BsModalRef;
  titleExcel: UserManageTitleExcel = <UserManageTitleExcel>{}
  lang: string = localStorage.getItem(LocalStorageConstants.LANG)?.toLocaleLowerCase();
  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 20
  };
  param : UserManageParam = <UserManageParam>{}
  users: Users[] = [];
  searchSubject: Subject<string> = new Subject<string>();
  userRank: string = JSON.parse(localStorage.getItem(LocalStorageConstants.USER)).userRank;

  constructor(
    private userManageService: UserManageService,
    private modalService: BsModalService,
  ) {
    super()
  }
  ngOnInit() {
    this.userManageService.dataSource.subscribe({
      next: result => {
        if (result) {
          this.param = result;
        }
      }
    })
    this.translateService.onLangChange.pipe(takeUntil(this.destroyService.destroys$)).subscribe(async res => {
      this.lang = res.lang == 'zh' ? 'zh-tw' : res.lang;
    });
    this.search();
    this.searchSubject.pipe(debounceTime(500)).subscribe(() => {
      this.search();
    });
  }

  onKeyUpUser() {
    this.searchSubject.next(this.param.keyword);
  }

  search() {
    this.pagination.pageNumber == 1 ? this.getData() : this.pagination.pageNumber = 1;
  }

  back(){
    this.userManageService.dataSource.next(this.param)
    this.router.navigate(['/manage']);
  }

  rolesDetail(item: any){
    this.router.navigate(['/manage/user/roles/', item.userID]).then(
      () => {
        this.userManageService.dataSource.next(this.param)
      },
      (error) => { }
    );
  }

  getData() {
    this.spinnerService.show();
    this.userManageService.getAll(this.pagination, this.param).subscribe({
      next: (res) => {
        this.users = res.result;
        this.pagination = res.pagination;
      },
      error: () => {
        this.snotifyService.error(
          this.translateService.instant('System.Message.UnknowError'),
          this.translateService.instant('System.Caption.Error')
        );
      }
    }).add(() => { this.spinnerService.hide() });
  }

  editUser(user: Users) {
    const initialState: ModalOptions = {
      class: 'modal-dialog-centered',
      initialState: {
        user
      }
    };
    this.bsModalRef = this.modalService.show(ManageUserEditComponent, initialState);
    this.bsModalRef.content.updateUsers.subscribe(() => {
      this.search();
    });
  }

  addUser() {
    const initialState: ModalOptions = {
      class: 'modal-dialog-centered'
    };
    this.bsModalRef = this.modalService.show(ManageUserAddComponent, initialState);
    this.bsModalRef.content.addUsers.subscribe((values: any) => {
      this.param.keyword = values.userName;
      this.search();
    });
  }
  exportExcel() {
    this.initLang()
    this.spinnerService.show();
    this.userManageService.exportExcel(this.titleExcel, this.param, this.lang).subscribe({
      next: (result) => {
        this.spinnerService.hide();
        result.isSuccess ? this.functionUtility.exportExcel(result.data, 'ExportUsers')
          : this.snotifyService.error(
            this.translateService.instant('System.Message.UnknowError'),
            this.translateService.instant('System.Caption.Error')
          );
      },
      error: () => {
        this.snotifyService.error(
          this.translateService.instant('System.Message.UnknowError'),
          this.translateService.instant('System.Caption.Error')
        );
        this.spinnerService.hide();
      },
    }).add(() => { this.spinnerService.hide() });
  }
  private initLang(): void {
    this.titleExcel.label_Username = this.translateService.instant('Manage.UserManage.UserName');
    this.titleExcel.label_Fullname = this.translateService.instant('Manage.UserManage.FullName');
    this.titleExcel.label_Email = this.translateService.instant('Manage.UserManage.Email');
    this.titleExcel.label_Visible = this.translateService.instant('Manage.UserManage.Visible');
    this.titleExcel.label_RankTitle = this.translateService.instant('Manage.UserManage.Role');
    this.titleExcel.label_CurrentRole = this.translateService.instant('Manage.UserManage.CurrentRole');
    this.titleExcel.label_Rank1 = this.translateService.instant('Manage.UserManage.ViewOnly');
    this.titleExcel.label_Rank2 = this.translateService.instant('Manage.UserManage.ApplyOnly');
    this.titleExcel.label_Rank3 = this.translateService.instant('Manage.UserManage.Approval');
    this.titleExcel.label_Rank6 = this.translateService.instant('Manage.UserManage.ViewSEAHR');
    this.titleExcel.label_Rank5 = this.translateService.instant('Manage.UserManage.FullAccess');
  }

  refreshSearch() {
    this.param.keyword = '';
    this.search();
  }

  pageChanged(event: PageChangedEvent) {
    this.pagination.pageNumber = event.page;
    this.getData();
  }

  getRoleGroup(userRank: number) {
    switch (userRank) {
      case 1:
        return this.translateService.instant('Manage.UserManage.ViewOnly');
      case 2:
        return this.translateService.instant('Manage.UserManage.ApplyOnly');
      case 3:
        return this.translateService.instant('Manage.UserManage.Approval');
      case 6:
        return this.translateService.instant('Manage.UserManage.ViewSEAHR');;
      case 4:
        return "SEA/HR";
      case 5:
        return this.translateService.instant('Manage.UserManage.FullAccess');
      default:
        return null;
    }
  }


}
