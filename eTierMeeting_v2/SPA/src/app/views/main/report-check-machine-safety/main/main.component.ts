import { Component, OnInit } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { BsDatepickerConfig, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { FunctionUtility } from '../../../../_core/_utility/function-utility';
import { ReportCheckMachineSafetyService } from '../../../../_core/_services/report-check-machine-safety.service';
import { ReportCheckMachineSafetyParam } from '../../../../_core/_dtos/report-check-machine-safety';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent implements OnInit {
  lang: string = localStorage.getItem('lang');
  fromDate: Date = null;
  toDate: Date = null;
  param: ReportCheckMachineSafetyParam = <ReportCheckMachineSafetyParam>{
    lang: '',
    fromDate: '',
    toDate: ''
  }
  datePickerConfig: Partial<BsDatepickerConfig> = { dateInputFormat: 'YYYY/MM/DD', showClearButton: true, clearPosition: 'right' };
  constructor(
    private service: ReportCheckMachineSafetyService,
    private spinnerService: NgxSpinnerService,
    private localeService: BsLocaleService,
    private functionUtility: FunctionUtility,
    private translate: TranslateService,
    private alertifyService: AlertifyService,
  ) { }

  ngOnInit() {
    this.dateLanguage();
  }
  dateLanguage() {
    this.lang = localStorage.getItem('lang');
    this.localeService.use(this.lang.substring(0, 2));
  }

  exportExcel() {
    if (this.inValidateDate())
      return this.alertifyService.error(this.translate.instant('historyinventory.please_select_date'));
    else {
      this.param.lang = this.lang
      this.param.fromDate = this.fromDate != null ? this.functionUtility.getDateFormat(this.fromDate) : '';
      this.param.toDate = this.toDate != null ? this.functionUtility.getDateFormat(this.toDate) : '';

      this.spinnerService.show();
      this.service.exportExcel(this.param).subscribe({
        next: (result) => {
          this.spinnerService.hide();
          if (result.success)
            this.functionUtility.exportExcel(result.data, '4_6_ReportCheckMachineSafety');
          else
            this.alertifyService.warning(result.message);
        },
        error: () => {
          this.spinnerService.hide();
          this.alertifyService.error(this.translate.instant('error.system_error'));
        },
      });

    }
  }

  inValidateDate(): boolean {
    return (this.fromDate != null && this.toDate == null) || (this.fromDate == null && this.toDate != null);
  }


}
