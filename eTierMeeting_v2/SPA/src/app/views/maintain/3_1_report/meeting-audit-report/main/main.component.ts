import { Component, OnInit } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { MeetingAuditReportService } from '@services/meeting-audit-report.service';
import { VW_T2_Meeting_LogParam } from '@models/vW_T2_Meeting_LogDTO';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.scss']
})
export class MainComponent extends InjectBase implements OnInit {

  param: VW_T2_Meeting_LogParam = <VW_T2_Meeting_LogParam>{}
  bsConfig: BsDatepickerConfig = <BsDatepickerConfig>{
    isAnimated: true,
    dateInputFormat: 'YYYY/MM/DD',
  };
  time_end = ''
  time_start = ''
  constructor(
    private service: MeetingAuditReportService,
  ) {
    super();
  }

  ngOnInit(): void {
  }
  checkDate() {
    this.param.startDate = this.functionUtility.checkEmpty(this.time_start) ? '' : this.functionUtility.getDateFormat(new Date(this.time_start));
    this.param.endDate = this.functionUtility.checkEmpty(this.time_end) ? '' : this.functionUtility.getDateFormat(new Date(this.time_end));
  }

  export() {
    this.checkDate()
    this.spinnerService.show();
    this.service.exportExcel(this.param).subscribe({
      next: (result: Blob) => {
        this.spinnerService.hide()
        this.functionUtility.exportExcel(result, 'Meeting Audit Report');
      },
      error: () => {
        this.spinnerService.hide()
      },
    })
  }
}
