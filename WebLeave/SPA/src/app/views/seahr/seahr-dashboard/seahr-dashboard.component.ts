import { Component, OnInit } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { RoleConstants } from '@constants/role.constants';
import { LoggedRoles } from '@models/auth/logged-roles.model';
import { Users } from '@models/auth/users.model';
import { AuthService } from '@services/auth/auth.service';
import { InjectBase } from '@utilities/inject-base-app';
import { animate, style, trigger, transition } from '@angular/animations';
@Component({
  selector: 'app-seahr-dashboard',
  templateUrl: './seahr-dashboard.component.html',
  styleUrls: ['./seahr-dashboard.component.css'],
  animations: [
    trigger('fadeInUp', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(-20px)' }),
        animate('300ms', style({ opacity: 1, transform: 'translateY(0)' }))
      ])
    ])
  ]
})

export class SeahrDashboardComponent extends InjectBase implements OnInit {
  user: Users;
  roles: LoggedRoles[];
  constructor(private authService: AuthService) {
    super()
    this.roles = this.route.snapshot.data['data'];
    console.log(this.roles)
  }
  async ngOnInit(): Promise<void> {
    this.spinnerService.show()
    this.countSeaHrEdit();
    this.countSeaHrConfirm();
    setTimeout(() => {
      this.spinnerService.hide()
    }, 500);
  }

  async countSeaHrEdit(): Promise<void> {
    const res: number = await firstValueFrom(this.authService.countSeaHrEdit());
    this.roles.forEach(item => {
      if (item.roleSym === RoleConstants.SEAHR_EDIT_LEAVE) {
        item.badge = res;
      }
    });
  }

  async countSeaHrConfirm(): Promise<void> {
    const res: number = await firstValueFrom(this.authService.countSeaHrConfirm());
    this.roles.forEach(item => {
      if (item.roleSym === RoleConstants.SEAHR_SEA_CONFIRM) {
        item.badge = res;
      }
    });
  }
}
