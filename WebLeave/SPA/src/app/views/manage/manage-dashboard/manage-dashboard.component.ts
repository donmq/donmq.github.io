import { Component, OnInit } from '@angular/core';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { RoleConstants } from '@constants/role.constants';
import { LoggedRoles } from '@models/auth/logged-roles.model';
import { Users } from '@models/auth/users.model';
import { animate, style, trigger, transition } from '@angular/animations';
@Component({
  selector: 'app-manage-dashboard',
  templateUrl: './manage-dashboard.component.html',
  styleUrls: ['./manage-dashboard.component.css'],
  animations: [
    trigger('fadeIn', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(-20px)' }),
        animate('200ms', style({ opacity: 1, transform: 'translateY(0)' }))
      ])
    ])
  ]
})
export class ManageDashboardComponent implements  OnInit {
  user: Users = JSON.parse(localStorage.getItem(LocalStorageConstants.USER));
  roles: LoggedRoles[] = this.user.roles.filter(role => role.roleSym === RoleConstants.DASHBOARD_MANAGE)[0].subRoles;

  constructor() {
  }
  ngOnInit(): void {
  }
}
