import { Component, OnDestroy, OnInit, effect } from '@angular/core';
import { InjectBase } from '@utilities/inject-base-app';
import { S_5_1_17_DailyAttendancePostingService } from '@services/attendance-maintenance/s_5_1_17_daily-attendance-posting.service';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { IconButton } from '@constants/common.constants';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { DailyAttendancePostingBasic, DailyAttendancePostingParam } from '@models/attendance-maintenance/5_1_17_daily-attendance-posting';
import { KeyValuePair } from '@utilities/key-value-pair';
@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent extends InjectBase implements OnInit, OnDestroy {
  title: string = '';
  listFactory: KeyValuePair[] = [];
  param: DailyAttendancePostingParam = <DailyAttendancePostingParam>{}
  processedRecords: number;
  iconButton = IconButton;
  language: string = localStorage.getItem(LocalStorageConstants.LANG);
  constructor(
    private service: S_5_1_17_DailyAttendancePostingService
  ) {
    super();

    effect(() => {
      this.param = this.service.paramSource().param;
      this.processedRecords = this.service.paramSource().processedRecords;
    });

    this.translateService.onLangChange.pipe(takeUntilDestroyed()).subscribe(res => {
      this.title = this.functionUtility.getTitle(this.route.snapshot.data['program'])
      this.language = res.lang;
      this.getListFactory();
    });
  }

  ngOnInit() {
    this.title = this.functionUtility.getTitle(this.route.snapshot.data['program'])
    this.getListFactory()
  }

  ngOnDestroy(): void {
    this.service.setSource(<DailyAttendancePostingBasic>{ param: this.param});
  }

  getListFactory() {
    this.service.getListFactory().subscribe({
      next: (res) => {
        this.listFactory = res;
        if (!this.listFactory.some(x => x.key === this.param.factory)) {
          this.param.factory = null;
        }
      },
      error: () => this.functionUtility.snotifySystemError(),
    });
  }
  execute() {
    this.snotifyService.confirm(this.translateService.instant('System.Message.ConfirmExecution'), this.translateService.instant('System.Action.Confirm'),
      () => {
        this.spinnerService.show();
        this.service.execute(this.param).subscribe({
          next: (result) => {
            this.spinnerService.hide();
            if (result.isSuccess) {
              this.functionUtility.snotifySuccessError(result.isSuccess, result.error)
              this.processedRecords = result.data;
            }
            else this.functionUtility.snotifySuccessError(result.isSuccess, result.error)
          },
          error: () => this.functionUtility.snotifySystemError(),
        });

      });
  }
  deleteProperty(name: string) {
    delete this.param[name]
  }
}
