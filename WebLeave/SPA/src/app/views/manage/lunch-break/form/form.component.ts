import { Component, OnInit } from '@angular/core';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { LunchBreak } from '@models/common/lunch-break';
import { DateTimePickerOption } from '@models/leave/personal/dateTimePickerOption';
import { LunchBreakService } from '@services/manage/lunch-break.service';
import { InjectBase } from '@utilities/inject-base-app';
import { OperationResult } from '@utilities/operation-result';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { takeUntil } from 'rxjs';
import { FunctionUtility } from '@utilities/function.utility';

@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.scss']
})
export class FormComponent extends InjectBase implements OnInit {
  modalRef?: BsModalRef;
  data: LunchBreak = <LunchBreak>{
    id: 0
  };
  language: string = localStorage.getItem(LocalStorageConstants.LANG)?.toLowerCase();
  workTimeStart: Date = null;
  workTimeEnd: Date = null;
  lunchTimeStart: Date = null;
  lunchTimeEnd: Date = null;
  optionWTS: DateTimePickerOption = <DateTimePickerOption>{};
  optionWTE: DateTimePickerOption = <DateTimePickerOption>{};
  optionLTS: DateTimePickerOption = <DateTimePickerOption>{};
  optionLTE: DateTimePickerOption = <DateTimePickerOption>{};
  title: string = '';
  constructor(
    private lunchBreakService: LunchBreakService,
    private modalService: BsModalService,
    private _functionUtilies: FunctionUtility
  ) {
    super();
  }

  ngOnInit(): void {
    this.reset();
    this.translateService.onLangChange.pipe(takeUntil(this.destroyService.destroys$)).subscribe(event => {
      this.language = event.lang;

      this.setOption();
    });
  }

  wtsChange() {
    if (this.workTimeStart && 24 - new Date(this.workTimeStart).getHours() > 8)
      this.optionWTE.minDate = this.workTimeStart;
  }

  wteChange() {
    if (this.workTimeEnd && 24 - new Date(this.workTimeStart).getHours() > 8)
      this.optionWTS.maxDate = this.workTimeEnd;
  }

  ltsChange() {
    if (this.lunchTimeStart && 24 - new Date(this.lunchTimeStart).getHours() > 8)
      this.optionLTE.minDate = this.lunchTimeStart;
  }

  lteChange() {
    if (this.lunchTimeEnd && 24 - new Date(this.lunchTimeStart).getHours() > 8)
      this.optionLTS.maxDate = this.lunchTimeEnd;
  }

  saveAdd(type: string) {
    this.execute('create', 'System.Message.CreateOKMsg', 'System.Message.CreateErrorMsg', type);
  }

  saveUpdate() {
    this.execute('update', 'System.Message.UpdateOKMsg', 'System.Message.UpdateErrorMsg');
  }

  execute(method: string, success: string, error: string, type?: string) {
    this.spinnerService.show();
    this.data.key = this.data.key.toUpperCase().trim();
    this.data.workTimeStart = new Date(this.workTimeStart).toStringTime('HH:mm');
    this.data.workTimeEnd = new Date(this.workTimeEnd).toStringTime('HH:mm');
    this.data.lunchTimeStart = new Date(this.lunchTimeStart).toStringTime('HH:mm');
    this.data.lunchTimeEnd = new Date(this.lunchTimeEnd).toStringTime('HH:mm');

    this.lunchBreakService[method](this.data)
      .subscribe({
        next: (res: OperationResult) => {
          this.spinnerService.hide();
          if (res.isSuccess) {
            this.snotifyService.success(this.translateService.instant(success), this.translateService.instant('System.Caption.Success'));
            if (type)
              type === 'SAVE' ? this.close("SAVE") : this.reset(true);
            else
              this.close("SAVE");
          }
          else {
            this.snotifyService.error(this.translateService.instant(res.error), this.translateService.instant('System.Caption.Error'));
          }
        }, error: () => {
          this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
          this.spinnerService.hide();
        }
      })
  }

  close(type: string) {
    this.lunchBreakService.emitDataChange(type !== "CLOSE");
    this.modalService.hide();
  }

  reset(emit?: boolean) {
    if (emit && emit == true)
      this.lunchBreakService.emitDataChange(true);

    this.workTimeStart = null;
    this.workTimeEnd = null;
    this.lunchTimeStart = null;
    this.lunchTimeEnd = null;

    this.lunchBreakService.currentlunchBreak.subscribe(lunchBreak => {
      this.data = { ...lunchBreak };
      if (this.data.type != 'CREATE') {
        this.workTimeStart = new Date(this.data.workTimeStart);
        this.workTimeEnd = new Date(this.data.workTimeEnd);
        this.lunchTimeStart = new Date(this.data.lunchTimeStart);
        this.lunchTimeEnd = new Date(this.data.lunchTimeEnd);
      }

      this.title = this.data.type == 'CREATE' ? 'System.Action.AddNew' : this.data.type == 'UPDATE' ? 'System.Action.Edit' : 'System.Action.Detail';
    });

    this.setOption();
  }

  setOption() {
    let options = this._functionUtilies.getDateTimePickerOption(this.language);
    options.format = 'HH:mm';

    this.optionWTS = { ...options };
    this.optionWTE = { ...options };
    this.optionLTS = { ...options };
    this.optionLTE = { ...options };
  }
}
