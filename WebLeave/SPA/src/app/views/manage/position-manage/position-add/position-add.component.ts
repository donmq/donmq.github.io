import { Component, OnInit } from '@angular/core';
import { PositionManage } from '@models/manage/position-manage/postion-manage';
import { PositionManageService } from '@services/manage/position-manage.service';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-position-add',
  templateUrl: './position-add.component.html',
  styleUrls: ['./position-add.component.scss']
})
export class PositionAddComponent extends InjectBase implements OnInit {
  positionManage: PositionManage = {} as PositionManage;
  constructor(
    private positionManageService: PositionManageService,
  ) {
    super();
  }

  ngOnInit() {
  }
  addPosition() {
    this.spinnerService.show();
    this.positionManageService.addPosition(this.positionManage)
      .subscribe({
        next: (res) => {
          this.spinnerService.hide();
          if (res.isSuccess) {
            this.cancel();
            this.snotifyService.success(this.translateService.instant('System.Message.CreateOKMsg'), this.translateService.instant('System.Caption.Success'));
          } else {
            this.snotifyService.error(this.translateService.instant('System.Message.CreateErrorMsg'), this.translateService.instant('System.Caption.Error'));
          }
        },
        error: () => {
          this.spinnerService.hide();
          this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
        }
      });
  }
  cancel() {
    this.router.navigate(['manage/position']);
  }

}
