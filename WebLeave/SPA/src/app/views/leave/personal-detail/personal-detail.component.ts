import { Component, OnInit } from '@angular/core';
import { HistoryEmployee } from '@models/leave/personal/historyEmployee';
import { LeaveDataViewModel } from '@models/leave/leaveDataViewModel';
import { EmployeeData } from '@models/leave/personal/employeeData';
import { LeavePersonalService } from '@services/leave/leave-personal.service';
import { InjectBase } from '@utilities/inject-base-app';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { takeUntil } from 'rxjs';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-personal-detail',
  templateUrl: './personal-detail.component.html',
  styleUrls: ['./personal-detail.component.scss'],
  imports: [
    CommonModule,
    TranslateModule,
    FormsModule
  ]
})
export class PersonalDetailComponent extends InjectBase implements OnInit {
  language: string = localStorage.getItem(LocalStorageConstants.LANG)?.toLowerCase();
  employee: EmployeeData = <EmployeeData>{};
  leaveData: LeaveDataViewModel[] = [];
  history: HistoryEmployee = <HistoryEmployee>{};
  empNumber: string;

  constructor(private leavePersonalService: LeavePersonalService) {
    super();
  }

  ngOnInit() {
    this.empNumber = this.route.snapshot.params['empNumber'];

    this.getData(this.empNumber);
    this.translateService.onLangChange
      .pipe(takeUntil(this.destroyService.destroys$))
      .subscribe((event) => {
        this.language = event.lang;
        this.getData(this.empNumber);
      });
  }

  getData(empNumber: string) {
    this.spinnerService.show();
    this.leavePersonalService.getDataDetail(empNumber).subscribe({
      next: (res) => {
        this.spinnerService.hide();
        if (res == null)
          this.router.navigate(['/login']);
        else {
          this.employee = res.employee;
          this.leaveData = res.leaveDataViewModel;
          this.history = <HistoryEmployee>{
            totalDay: (+res.history.totalDay)?.toFixed(5).replace(/\.?0*$/g, ''),
            countAgent: `${(+res.history.countAgent.split('/')[0])
              ?.toFixed(5)
              .replace(/\.?0*$/g, '')} / ${res.history.countAgent.split('/')[1]}`,
            countArran: `${(+res.history.countArran.split('/')[0])
              ?.toFixed(5)
              .replace(/\.?0*$/g, '')} / ${res.history.countArran.split('/')[1]}`,
            countLeave: (+res.history.countLeave)
              ?.toFixed(5)
              .replace(/\.?0*$/g, ''),
            countTotal: (+res.history.countTotal)
              ?.toFixed(5)
              .replace(/\.?0*$/g, ''),
            countRestAgent: (+res.history.countRestAgent)
              ?.toFixed(5)
              .replace(/\.?0*$/g, ''),
            countRestArran: (+res.history.countRestArran)
              ?.toFixed(5)
              .replace(/\.?0*$/g, ''),
          };
        }
      },
      error: () => {
        this.spinnerService.hide();
        this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
      },
    });
  }

}
