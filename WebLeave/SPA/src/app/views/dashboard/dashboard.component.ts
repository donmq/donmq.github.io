import { animate, style, transition, trigger } from '@angular/animations';
import { Component } from '@angular/core';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { Users } from '@models/auth/users.model';
import { LeaveHistoryService } from '@services/leave/leave-history.service';
import { LeaveApproveService } from '@services/leave/leave-approve.service';
import { DepartmentService } from '@services/manage/Department.service';
import { EmployeeService } from '@services/manage/employee.service';
import { TeamManagementService } from '@services/manage/team-management.service';
import { UserManageService } from '@services/manage/user-manage.service';
import { ExportHpService } from '@services/seahr/export-hp.service';
import { SeaConfirmService } from '@services/seahr/sea-confirm.service';
import { SeahrEmployeeManagerService } from '@services/seahr/seahr-employee-manager.service';
import { SeahrHistoryService } from '@services/seahr/seahr-history.service';
import { SeahrPermissionRightsService } from '@services/seahr/seahr-permission-rights.service';
import { ViewConfirmDailyService } from '@services/seahr/view-confirm-daily.service';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.scss'],
  animations: [
    trigger('fadeInUp', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(-20px)' }),
        animate('300ms', style({ opacity: 1, transform: 'translateY(0)' }))
      ])
    ])
  ]
})
export class DashboardComponent extends InjectBase {
  constructor(private seaConfirmService: SeaConfirmService, private exportHPService: ExportHpService,
    private serviceSeahrEmployeeManager: SeahrEmployeeManagerService,private seahrHistoryService: SeahrHistoryService,
    private viewConfirmDailyService: ViewConfirmDailyService,  private teamManagementService: TeamManagementService,
    private serviceSeahrPermissionRights: SeahrPermissionRightsService, private userManageService: UserManageService,
    private employeeService: EmployeeService,private serviceDepartment: DepartmentService,
    private leaveHistoryService: LeaveHistoryService, private leaveaApproveService: LeaveApproveService,

    ) {
    super();
  }
   ngOnInit() {
    this.seaConfirmService.dataSource.next(null);
    this.exportHPService.dataSource.next(null);
    this.serviceSeahrEmployeeManager.dataSource.next(null);
    this.seahrHistoryService.dataSource.next(null);
    this.viewConfirmDailyService.dataSource.next(null);
    this.serviceSeahrPermissionRights.dataSource.next(null);
    this.teamManagementService.dataSource.next(null);
    this.userManageService.dataSource.next(null);
    this.serviceDepartment.dataSource.next(null);
    this.employeeService.dataSource.next(null);
    this.leaveHistoryService.dataSource.next(null);
    this.leaveaApproveService.dataSource.next(null);
  }
  user: Users = JSON.parse(localStorage.getItem(LocalStorageConstants.USER));
}
