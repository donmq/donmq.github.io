import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { ManageRoleRankEditComponent } from '../manage-role-rank-edit/manage-role-rank-edit.component';
import { UserRoleManageService } from '@services/manage/user-role-manage.service';
import { Users } from '@models/manage/user-manage/Users';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { UserManageService } from '@services/manage/user-manage.service';
import { DestroyService } from '@services/destroy.service';
import { takeUntil } from 'rxjs';
import { InjectBase } from '@utilities/inject-base-app';
import { RoleNode, TreeNode } from '@models/auth/treenode';

@Component({
  selector: 'app-manage-role-leave',
  templateUrl: './manage-role-leave.component.html',
  styleUrls: ['./manage-role-leave.component.css'],
  providers: [DestroyService]
})
export class ManageRoleLeaveComponent extends InjectBase implements OnInit {
  lang: string = localStorage.getItem(LocalStorageConstants.LANG)?.toLocaleLowerCase();
  bsModalRef?: BsModalRef;
  user: Users;
  listUsers: string[] = [];
  roleName: string;

  roles: TreeNode<RoleNode>[] = [];
  assignedRoles: TreeNode<RoleNode>[] = [];
  constructor(
    private modalService: BsModalService,
    private userRoleManageService: UserRoleManageService,
    private userManageService: UserManageService
  ) {
    super()
  }

  ngOnInit() {
    this.route.data.subscribe(data => {
      this.user = data['user'];
      this.changeLang();
      this.getAllRoleUser();
    })
    this.getRoleRank();
  }


  onMouseEnter(roleID: number) {
    this.getListUsers(roleID);
  }

  getListUsers(roleID: number) {
    this.userRoleManageService.listUsers(roleID).subscribe({
      next: (res) => {
        this.listUsers = res;
      },
      error: () => {
        this.snotifyService.error(this.translateService.instant('System.Caption.Error'), this.translateService.instant('System.Message.UnknowError'),
        );
      }
    })
  }

  changeLang() {
    this.translateService.onLangChange.pipe(takeUntil(this.destroyService.destroys$)).subscribe((event) => {
      this.lang = event.lang == 'zh' ? 'zh-tw' : event.lang;
      this.getAllRoleUser();
      this.getRoleRank();
    });
  }

  getAllRoleUser() {
    this.spinnerService.show();
    this.userRoleManageService.getAllRoleUser(this.user.userID, this.lang).subscribe({
      next: (res: any) => {
        this.roles = res.roles;
        this.assignedRoles = res.assignedRoles;
      },
      error: () => {
        this.snotifyService.error(
          this.translateService.instant('System.Message.UnknowError'),
          this.translateService.instant('System.Caption.Error')
        );
      }
    }).add(() => { this.spinnerService.hide() });
  }

  // ExportRolesUser
  exportExcel() {
    this.spinnerService.show();
    this.userRoleManageService.exportExcel(this.user.userID, this.lang).subscribe({
      next: (result) => {
        this.spinnerService.hide();
        if(result.error == 'No data for excel download') {
          this.snotifyService.error(
            this.translateService.instant('Manage.UserManage.NoDataExport'),
            this.translateService.instant('System.Caption.Error'))
        }
        else {
          result.isSuccess ? this.functionUtility.exportExcel(result.data, 'ExportRolesUser')
          : this.snotifyService.error(
            this.translateService.instant('System.Message.SystemError'),
            this.translateService.instant('System.Caption.Error')
          );
        }
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

  assignRole(event: any) {
    this.spinnerService.show();
    this.userRoleManageService.assignRole(this.user.userID, event.node.data.roleID).subscribe({
      next: (res) => {
        if (res.isSuccess)
          this.snotifyService.success(
            this.translateService.instant('System.Message.CreateOKMsg'),
            this.translateService.instant('System.Caption.Success')
          );
        else
          this.snotifyService.error(
            this.translateService.instant('System.Message.CreateErrorMsg'),
            this.translateService.instant('System.Caption.Error')
          );

        this.spinnerService.hide();
        this.getAllRoleUser();
      }, error: () => {
        this.snotifyService.error(
          this.translateService.instant('System.Message.UnknowError'),
          this.translateService.instant('System.Caption.Error')
        );
      }
    });
  }

  unAssignRole(event: any) {
    this.spinnerService.show();
    this.userRoleManageService.unAssignRole(this.user.userID, event.node.data.roleID).subscribe({
      next: (res) => {
        if (res.isSuccess)
          this.snotifyService.success(
            this.translateService.instant('System.Message.DeleteOKMsg'),
            this.translateService.instant('System.Caption.Success')
          );
        else
          this.snotifyService.error(
            this.translateService.instant('System.Message.DeleteErrorMsg'),
            this.translateService.instant('System.Caption.Error')
          );

        this.spinnerService.hide();
        this.getAllRoleUser();
      }, error: () => {
        this.snotifyService.error(
          this.translateService.instant('System.Message.UnknowError'),
          this.translateService.instant('System.Caption.Error')
        );
      }
    });
  }

  // getRecursiveRoleId(nodes: TreeNode<RoleNode>[]) {
  //   let roleIds: number[] = [];
  //   nodes.forEach(x => {
  //     if (x.children?.length > 0)
  //       roleIds.push(...this.getRecursiveRoleId(x.children));

  //     roleIds.push(x.data.roleID);
  //   })

  //   return roleIds;
  // }

  editRoleRank(user: Users, roleName: string) {
    const initialState: ModalOptions = {
      class: 'modal-dialog-centered',
      initialState: {
        user,
        roleName
      }
    };
    this.bsModalRef = this.modalService.show(ManageRoleRankEditComponent, initialState);
    this.bsModalRef.content.updateRoles.subscribe(() => {
      this.getAllRoleUser();
      this.getRoleRank();
    })
  }

  getRoleRank() {
    this.userManageService.getUser(this.user.userID).subscribe({
      next: (res) => {
        this.user = res;
        switch (res.userRank) {
          case 1: this.roleName = this.translateService.instant('Manage.UserManage.ViewOnly'); break;
          case 2: this.roleName = this.translateService.instant('Manage.UserManage.ApplyOnly'); break;
          case 3: this.roleName = this.translateService.instant('Manage.UserManage.Approval'); break;
          case 6: this.roleName = this.translateService.instant('Manage.UserManage.ViewSEAHR'); break;
          case 4: this.roleName = 'SEA/HR'; break;
          case 5: this.roleName = this.translateService.instant('Manage.UserManage.FullAccess'); break;
        }
      }
    })
  }
}


