import { Component, OnInit } from '@angular/core';
import { DestroyService } from '@services/destroy.service';
import { SubscriptionLike } from 'rxjs';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { Users } from '@models/auth/users.model';
import { UserRoleManageService } from '@services/manage/user-role-manage.service';
import { takeUntil } from 'rxjs';
import { InjectBase } from '@utilities/inject-base-app';
import { GroupBaseNode, TreeNode } from '@models/auth/treenode';

@Component({
  selector: 'app-manage-role-group',
  templateUrl: './manage-role-group.component.html',
  styleUrls: ['./manage-role-group.component.css'],
  providers: [DestroyService]
})
export class ManageRoleGroupComponent extends InjectBase implements OnInit {
  lang: string = localStorage.getItem(LocalStorageConstants.LANG)?.toLocaleLowerCase();
  subscription: SubscriptionLike;
  user: Users;
  roles: TreeNode<GroupBaseNode>[] = [];
  assignedRoles: TreeNode<GroupBaseNode>[] = [];

  constructor(
    private userRoleManageService: UserRoleManageService,
  ) {
    super();
  }

  ngOnInit() {
    this.changeLang();
    this.route.data.subscribe(data => {
      this.user = data['user'];
      this.getAllGroupBase();
    })
  }

  changeLang() {
    this.translateService.onLangChange.pipe(takeUntil(this.destroyService.destroys$)).subscribe((event) => {
      this.lang = event.lang == 'zh' ? 'zh-tw' : event.lang;
      this.getAllGroupBase();
    });
  }

  getAllGroupBase() {
    this.userRoleManageService.getAllGroupBase(this.user.userID, this.lang).subscribe({
      next: (res) => {
        this.roles = res.roles;
        this.assignedRoles = res.assignedRoles;
      },
      error: () => {
        this.snotifyService.error(
          this.translateService.instant('System.Message.UnknowError'),
          this.translateService.instant('System.Caption.Error')
        );
      }
    });
  }

  assignRole(event: any) {
    this.spinnerService.show();
    this.userRoleManageService.assignGroupBase(this.user.userID, event.node.data.gbid).subscribe({
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
        this.getAllGroupBase();
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
    this.userRoleManageService.unAssignGroupBase(this.user.userID, event.node.data.gbid).subscribe({
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
        this.getAllGroupBase();
      }, error: () => {
        this.snotifyService.error(
          this.translateService.instant('System.Message.UnknowError'),
          this.translateService.instant('System.Caption.Error')
        );
      }
    });
  }
}
