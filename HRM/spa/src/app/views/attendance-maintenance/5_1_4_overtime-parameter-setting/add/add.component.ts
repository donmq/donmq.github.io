import { Component, OnInit, effect } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { IconButton } from '@constants/common.constants';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import { HRMS_Att_Overtime_ParameterDTO } from '@models/attendance-maintenance/5_1_4_overtime-parameter-setting';
import { S_5_1_4_OvertimeParameterSettingService } from '@services/attendance-maintenance/s_5_1_4_overtime-parameter-setting.service';
import { InjectBase } from '@utilities/inject-base-app';
import { KeyValuePair } from '@utilities/key-value-pair';

@Component({
  selector: 'app-add',
  templateUrl: './add.component.html',
  styleUrls: ['./add.component.css']
})
export class AddComponent extends InjectBase implements OnInit {
  title: string = '';
  url: string = '';
  data: HRMS_Att_Overtime_ParameterDTO = <HRMS_Att_Overtime_ParameterDTO>{};
  listDivision: KeyValuePair[] = [];
  listFactory: KeyValuePair[] = [];
  listWorkShiftType: KeyValuePair[] = [];
  effective_Month_Value: Date;
  iconButton = IconButton;
  constructor(
    private service: S_5_1_4_OvertimeParameterSettingService,
  ) {
    super();
    this.translateService.onLangChange.pipe(takeUntilDestroyed()).subscribe(res => {
      this.title = this.functionUtility.getTitle(this.route.snapshot.data['program'])
      this.getListDivision();
      this.getListFactory();
      this.getListWorkShiftType();
    });
    this.getDataFromSource()
  }

  getDataFromSource() {
    effect(() => {
      let source = this.service.paramSource();
      if (source && source != null) {
        this.data = { ...source.data };
        this.data.update_By = JSON.parse((localStorage.getItem(LocalStorageConstants.USER))).account;
        this.data.update_Time = this.functionUtility.getDateTimeFormat(new Date())
      }
      else this.back();
    })
  }

  ngOnInit() {
    this.title = this.functionUtility.getTitle(this.route.snapshot.data['program']);
    this.url = this.functionUtility.getRootUrl(this.router.routerState.snapshot.url);
    this.getListDivision();
    this.getListFactory();
    this.getListWorkShiftType();
  }

  //#region
  getListWorkShiftType() {
    this.service.getListWorkShiftType().subscribe({
      next: (res) => {
        this.listWorkShiftType = res;
      },
      error: () => this.functionUtility.snotifySystemError(),
    });
  }

  getListDivision() {
    this.service.getListDivision().subscribe({
      next: (res) => {
        this.listDivision = res
      },
      error: () => this.functionUtility.snotifySystemError(),
    });
  }

  getListFactory() {
    this.spinnerService.show();
    this.service.getListFactory(this.data.division).subscribe({
      next: (res) => {
        this.listFactory = res
        this.spinnerService.hide();
      },
      error: () => this.functionUtility.snotifySystemError(),
    });
  }
  //#endregion

  onFactoryChange() {
    this.listFactory = []
    this.deleteProperty('factory')
    this.getListFactory()
  }

  deleteProperty(name: string) {
    delete this.data[name]
  }

  save() {
    this.data.effective_Month = (!this.functionUtility.checkEmpty(this.effective_Month_Value)
      && (this.effective_Month_Value.toString() != 'Invalid Date' && this.effective_Month_Value.toString() != 'NaN/NaN'))
      ? this.functionUtility.getDateFormat(this.effective_Month_Value)
      : "";
    this.spinnerService.show();
    this.service.create(this.data).subscribe({
      next: result => {
        if (result.isSuccess) {
          this.functionUtility.snotifySuccessError(result.isSuccess, result.error)
          this.back();
        }
        else this.functionUtility.snotifySuccessError(result.isSuccess, result.error)
      },
      error: () => this.functionUtility.snotifySystemError(),
      complete: () => this.spinnerService.hide()
    })
  }

  saveAndContinue() {
    if (this.effective_Month_Value)
      this.data.effective_Month = this.effective_Month_Value.toDate().toStringYearMonth();

    this.spinnerService.show();
    this.service.create(this.data).subscribe({
      next: result => {
        if (result.isSuccess) {
          this.functionUtility.snotifySuccessError(result.isSuccess, result.error)
          this.clear()
        }
        else this.functionUtility.snotifySuccessError(result.isSuccess, result.error)
      },
      error: () => this.functionUtility.snotifySystemError(),
      complete: () => this.spinnerService.hide()
    })
  }

  clear() {
    this.listFactory = []
    this.deleteProperty('factory')
    this.deleteProperty('division')
    this.deleteProperty('work_Shift_Type')
    this.deleteProperty('effective_Month')
    this.deleteProperty('overtime_Start')
    this.deleteProperty('overtime_End')
    this.deleteProperty('overtime_Hours')
    this.deleteProperty('night_Hours')
    this.effective_Month_Value = null
  }

  back = () => this.router.navigate([this.url]);
}
