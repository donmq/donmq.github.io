import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { Datepicker } from '@models/manage/datepicker-management/datepicker';
import { DatepickerService } from '@services/manage/datepicker.service';
import { OperationResult } from '@utilities/operation-result';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-datepicker-management',
  templateUrl: './datepicker-management.component.html',
  styleUrls: ['./datepicker-management.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class DatepickerManagementComponent extends InjectBase implements OnInit {
  datepicker: Datepicker[] = [];
  constructor(
    private datepickerService: DatepickerService,
  ) {
    super();
   }

  ngOnInit(): void {
    this.getDatepicker();
  }

  getDatepicker() {
    this.spinnerService.show();
    this.datepickerService.getAllDatepicker().subscribe({
      next: (data : Datepicker[]) => this.datepicker = data,
      error: (err) => {
        this.spinnerService.hide();
        this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
      },
      complete: () => this.spinnerService.hide()
    })
  }

  updateDatepicker(datepicker: Datepicker) {
    this.spinnerService.show();
    this.datepickerService.updateDatepicker(datepicker).subscribe({
      next: (data : OperationResult) =>
        data.isSuccess ? this.snotifyService.success(this.translateService.instant('System.Message.UpdateOKMsg'), this.translateService.instant('System.Caption.Success'))
          : this.snotifyService.error(this.translateService.instant('System.Message.UpdateErrorMsg'), this.translateService.instant('System.Caption.Error')),
      error: (err) => {
        this.spinnerService.hide();
        this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
       },
      complete: () => this.spinnerService.hide()
    })
  }

  back() {
    this.router.navigate(['/manage']);
  }

}
