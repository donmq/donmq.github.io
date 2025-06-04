import { Component, OnInit } from '@angular/core';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { Users } from '@models/auth/users.model';
import { Surrogate, SurrogateRemove } from '@models/leave/surrogate';
import { LeaveSurrogateService } from '@services/leave/leave-surrogate.service';
import { InjectBase } from '@utilities/inject-base-app';
import { KeyValuePair } from '@utilities/key-value-pair';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent extends InjectBase implements OnInit {
  surrogate: Surrogate = <Surrogate>{};
  surrogates: KeyValuePair[] = [];
  surrogateId: number = 0;
  curentUser: Users = JSON.parse(localStorage.getItem(LocalStorageConstants.USER));
  surrogateView: KeyValuePair = <KeyValuePair>{};
  constructor(private readonly _service: LeaveSurrogateService) {
    super();
  }

  ngOnInit() {
    this.getSurrogates();
  }

  getSurrogates() {
    this.spinnerService.show();
    this._service.getSurrogates(this.curentUser.userID)
      .subscribe({
        next: (res) => {
          this.surrogates = res;
          this.surrogates.unshift({ key: 0, value: this.translateService.instant('Leave.LeaveSurrogate.SelectTitle') as string });

          this.getDetail(res);
        },
        error: () => {
          this.snotifyService.error(
            this.translateService.instant('System.Message.UnknowError'),
            this.translateService.instant('System.Caption.Error')
          );
          this.spinnerService.hide();
        }
      })
  }

  getDetail(surrogates: KeyValuePair[]) {
    this.spinnerService.show();
    this._service.getDetail(this.curentUser.userID)
      .subscribe({
        next: (res) => {
          this.surrogate = res;
          if (this.surrogate.surrogateId > 0)
            this.surrogateView = surrogates.find(x => x.key == this.surrogate.surrogateId);
          this.spinnerService.hide();
        },
        error: () => {
          this.snotifyService.error(
            this.translateService.instant('System.Message.UnknowError'),
            this.translateService.instant('System.Caption.Error')
          );
          this.spinnerService.hide();
        }
      })
  }

  save() {
    if (this.surrogate.surrogateId > 0)
      return this.snotifyService.error(this.translateService.instant('Leave.LeaveSurrogate.Message'), this.translateService.instant('System.Caption.Error'));

    this.surrogate.surrogateId = this.surrogateId;
    this.spinnerService.show();
    this._service.saveSurrogate(this.surrogate)
      .subscribe({
        next: (res) => {
          this.spinnerService.hide();
          if (res) {
            this.snotifyService.success(this.translateService.instant('System.Message.UpdateOKMsg'), this.translateService.instant('System.Caption.Success'));
            this.surrogateView = this.surrogates.find(x => x.key == this.surrogateId);
          }
          else {
            this.snotifyService.error(this.translateService.instant('System.Message.UpdateErrorMsg'), this.translateService.instant('System.Caption.Error'));
          }
        }, error: () => {
          this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
          this.spinnerService.hide();
        }
      })
  }

  remove() {
    this.snotifyService.confirm(
      this.translateService.instant('System.Message.ConfirmDeleteMsg'),
      this.translateService.instant('System.Caption.Confirm'),
      () => {
        let removes = <SurrogateRemove>{
          userID: this.surrogate.userID,
          surrogateId: this.surrogate.surrogateId
        }
        this.spinnerService.show();
        this._service.removeSurrogate(removes)
          .subscribe({
            next: (res) => {
              this.spinnerService.hide();
              if (res) {
                this.snotifyService.success(this.translateService.instant('System.Message.DeleteOKMsg'), this.translateService.instant('System.Caption.Success'));
                this.clear();
              }
              else {
                this.snotifyService.success(this.translateService.instant('System.Message.DeleteErrorMsg'), this.translateService.instant('System.Caption.Error'));
              }
            }, error: () => {
              this.snotifyService.success(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
              this.spinnerService.hide();
            }
          })
      });
  }

  clear() {
    this.surrogateId = 0;
    this.surrogate.surrogateId = 0;
    this.surrogateView = <KeyValuePair>{};
  }

  cancel() {
    this.router.navigate(['leave']);
  }
}
