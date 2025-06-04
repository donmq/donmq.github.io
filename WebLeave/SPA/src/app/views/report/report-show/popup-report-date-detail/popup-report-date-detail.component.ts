import { Component, Input, OnInit } from '@angular/core';
import { ExportExcelDateDto } from '@models/report/report-show/export-excel-date-dto';
import { ReportShowService } from '@services/report/report-show/reportShow.service';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-popup-report-date-detail',
  templateUrl: './popup-report-date-detail.component.html',
  styleUrls: ['./popup-report-date-detail.component.css'],
})
export class PopupReportDateDetailComponent extends InjectBase implements OnInit {
  @Input() public items: ExportExcelDateDto;
  constructor(
    private reportShowService: ReportShowService,
  ) {
    super();
  }

  ngOnInit() { }

  onExportDateDetail() {
    this.spinnerService.show();
    this.reportShowService.exportExcelDateDetail(this.items).subscribe({
      next: (result) => {
        this.spinnerService.hide();
        const timeNow = new Date();
        const fileName: string =
          'Report_' +
          timeNow.getFullYear().toString() +
          '_' +
          (timeNow.getMonth() + 1) +
          '_' +
          timeNow.getDate();
        console.log(result);
        result.isSuccess ? this.functionUtility.exportExcel(result.data, fileName, 'xlsx')
          : this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));

        this.spinnerService.hide();
      },
      error: () => {
        this.snotifyService.error(
          this.translateService.instant('System.Message.UnknowError'),
          this.translateService.instant('System.Caption.Error')
        );
        this.spinnerService.hide();
      },
    });
  }
}
