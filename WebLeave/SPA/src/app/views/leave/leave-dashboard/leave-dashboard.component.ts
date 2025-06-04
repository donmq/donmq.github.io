import { Component, OnInit } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { RoleConstants } from '@constants/role.constants';
import { LoggedRoles } from '@models/auth/logged-roles.model';
import { Users } from '@models/auth/users.model';
import { AuthService } from '@services/auth/auth.service';
import { InjectBase } from '@utilities/inject-base-app';
import { animate, style, transition, trigger } from '@angular/animations';

@Component({
  selector: 'app-leave-dashboard',
  templateUrl: './leave-dashboard.component.html',
  styleUrls: ['./leave-dashboard.component.css'],
  animations: [
    trigger('fadeInUp', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(-20px)' }),
        animate('200ms', style({ opacity: 1, transform: 'translateY(0)' }))
      ])
    ])
  ]
})
export class LeaveDashboardComponent implements OnInit {
  user: Users = JSON.parse(localStorage.getItem(LocalStorageConstants.USER));
  roles: LoggedRoles[] = this.user?.roles?.find(role => role.roleSym === RoleConstants.DASHBOARD_LEAVE)?.subRoles || [];

  constructor(private authService: AuthService) {}

  async ngOnInit(): Promise<void> {
    await this.countLeaveEdit(this.user.userID);
  }

  async countLeaveEdit(userID: number) {
    const res = await firstValueFrom(this.authService.countLeaveEdit(userID));
    this.roles.forEach(item => {
      if (item.roleSym === RoleConstants.LEAVE_EDIT_LEAVE) {
        item.badge = res;
      }
    });
  }
}
