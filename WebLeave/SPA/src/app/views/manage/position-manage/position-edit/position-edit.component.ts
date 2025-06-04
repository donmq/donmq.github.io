import { Component, OnInit } from '@angular/core';
import { PositionManage } from '@models/manage/position-manage/postion-manage';
import { PositionManageService } from '@services/manage/position-manage.service';
import { DestroyService } from '@services/destroy.service';
import { takeUntil } from 'rxjs';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-position-edit',
  templateUrl: './position-edit.component.html',
  styleUrls: ['./position-edit.component.scss'],
  providers: [DestroyService]
})
export class PositionEditComponent extends InjectBase implements OnInit {
  positionManage: PositionManage = {} as PositionManage;
  editOrDetail: boolean;
  constructor(
    private positionManageService: PositionManageService,
  ) {
    super();
   }

  ngOnInit() {
    this.spinnerService.show();
    if (this.route.snapshot.params['edit'] === 'edit') {
      this.editOrDetail = true;
    } else {
      this.editOrDetail = false;
    };
    this.positionManageService.currentModel
      .pipe(takeUntil(this.destroyService.destroys$))
      .subscribe(res => {
        if (res !== null) {
          this.spinnerService.hide();
          this.getDetailPosition(res);
        } else {
          this.spinnerService.hide();
          this.router.navigate(['manage/position']);
        }
      }).unsubscribe();
  }
  getDetailPosition(IDPosition: number) {
    this.spinnerService.show();
    this.positionManageService.getDetailPosition(IDPosition)
      .pipe(takeUntil(this.destroyService.destroys$))
      .subscribe({
        next: (data) => {
          this.positionManage = data;
          this.spinnerService.hide();
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
  editPosition(item: PositionManage) {
    let model = { ...item };
    this.spinnerService.show();
    this.positionManageService.editPosition(model)
      .subscribe({
        next: (res) => {
          this.spinnerService.hide();
          if (res.isSuccess) {
            this.cancel();
            this.snotifyService.success(this.translateService.instant('System.Message.UpdateOKMsg'), this.translateService.instant('System.Caption.Success'));
          } else {
            this.snotifyService.success(this.translateService.instant('System.Message.UpdateErrorMsg'), this.translateService.instant('System.Caption.Error'));
          }
        },
        error: () => {
          this.spinnerService.hide();
          this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
        }
      });
  }
}
