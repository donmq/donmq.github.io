import { Component, OnInit } from '@angular/core';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { ReportShowModel } from '@models/report/report-show/report-show-model.model';
import { ReportGridDetailParam } from '@params/report/report-show/report-grid-detail-param.param';
import { ReportShowParam } from '@params/report/report-show/report-show-param.param';
import { ReportShowService } from '@services/report/report-show/reportShow.service';
import { BsModalService, BsModalRef, ModalOptions } from 'ngx-bootstrap/modal';
import { PopupReportGridDetailComponent } from '../popup-report-grid-detail/popupReportGridDetail.component';
import { ReportDateDetailParam } from '@params/report/report-show/report-date-detail-param.param';
import { PopupReportDateDetailComponent } from '../popup-report-date-detail/popup-report-date-detail.component';
import { ReportIndexViewModel } from '@models/report/report-show/report-index-view-model.model';
import { LangConstants } from '@constants/lang.constants';
import { KeyValuePair } from '@utilities/key-value-pair';
import { MessageConstants } from '@constants/message.enum';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { takeUntil } from 'rxjs';
import { ReportDateDetail } from '@models/report/report-show/report-date-detail.model';
import { ExportExcelDateDto } from '@models/report/report-show/export-excel-date-dto';
import { ExportExcelGridDto } from '@models/report/report-show/export-excel-grid-dto';
import { DestroyService } from '@services/destroy.service';
import { GetTitleByLang } from '@models/report/report-show/title-by-lang';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-reportshow',
  templateUrl: './reportshow.component.html',
  styleUrls: ['./reportshow.component.css'],
  providers: [DestroyService]
})
export class ReportshowComponent extends InjectBase implements OnInit {
  bsModalRef?: BsModalRef;
  bsValueStartDay: Date;
  bsValueEndDay: Date;
  reportIndexViewModels: ReportShowModel[];
  dataToExports: ReportIndexViewModel = <ReportIndexViewModel>{
    lang: '',
    titleExportExcel: '',
  };
  listParents: ReportShowModel[][];
  reportGridDetails: ReportShowModel[];
  title: GetTitleByLang = <GetTitleByLang>{
    vi: '',
    en: '',
    zh_TW: '',
  };
  datePickerConfig: Partial<BsDatepickerConfig> = {
    showWeekNumbers: false,
    dateInputFormat: 'DD/MM/YYYY',
    containerClass: 'theme-dark-blue',
  };
  param: ReportShowParam = <ReportShowParam>{};
  paramFromReportPage: KeyValuePair = <KeyValuePair>{};
  isToggleGridDetail: boolean = true;
  lang: string = localStorage.getItem(LocalStorageConstants.LANG);
  constructor(
    private reportShowService: ReportShowService,
    private modalService: BsModalService,

  ) {
    super();
  }

  ngOnInit() {
    this.getIndexFromReportPage();
    this.param.lang = this.lang;
    this.translateService.onLangChange
      .pipe(takeUntil(this.destroyService.destroys$))
      .subscribe((e) => {
        this.param.lang = e.lang == 'zh' ? LangConstants.ZH_TW : e.lang;
        this.dataToExports.lang = this.param.lang;
        this.dataToExports.titleExportExcel =
          this.title[
          this.param.lang == LangConstants.ZH_TW ? 'zh_TW' : this.param.lang
          ];
      });
    // set from date = ngày đầu tiên của tháng hiện tại -- end date = ngày cuối cùng của tháng
    this.setDateIndex();
    if (this.param.id)
      this.reportShow();
  }

  getIndexFromReportPage = () => {
    this.reportShowService.param.subscribe(
      (res) => (this.paramFromReportPage = res)
    );
    this.param.id = this.paramFromReportPage.key;
    if (this.param.id == undefined) this.router.navigate(['report']);
    if (this.paramFromReportPage.value == 'company') {
      this.param.index = 0;
    } else if (this.paramFromReportPage.value == 'area') {
      this.param.index = 1;
    } else if (this.paramFromReportPage.value == 'building') {
      this.param.index = 2;
    } else if (this.paramFromReportPage.value == 'department') {
      this.param.index = 3;
    } else if (this.paramFromReportPage.value == 'part') {
      this.param.index = 4;
    }
  };

  setDateIndex = () => {
    this.bsValueStartDay = new Date().toFirstDateOfMonth();
    this.bsValueEndDay = new Date().toLastDateOfMonth();
  };

  onToggleGrid() {
    this.isToggleGridDetail = true;
  }

  onToggleMonth() {
    this.isToggleGridDetail = false;
  }

  onSearch() {
    this.param.from = this.bsValueStartDay?.toStringDate('yyyy/MM/dd') ?? '';
    this.param.to = this.bsValueEndDay?.toStringDate('yyyy/MM/dd') ?? '';

    this.spinnerService.show();
    this.reportShowService.reportShow(this.param).subscribe({
      next: (res) => {
        if (res.listReportShowModel.length != 0) {
          this.reportIndexViewModels = res.listReportShowModel;
          this.listParents = res.listParent;
          this.dataToExports = res;
          this.dataToExports.lang = this.param.lang;
          this.dataToExports.titleExportExcel =
            res.title[
            this.param.lang == LangConstants.ZH_TW ? 'zh_TW' : this.param.lang
            ];
        } else {
          this.snotifyService.error(MessageConstants.COMPARE_DATE, 'Error!');
        }
        this.spinnerService.hide();
      },
      error: (error) => {
        this.snotifyService.error(
          this.translateService.instant('System.Message.UnknowError'),
          this.translateService.instant('System.Caption.Error')
        );
        this.spinnerService.hide();
      },
    });
  }
  // CLick từng dòng table ở dạng lưới
  onReportGridDetail(item: ReportShowModel) {
    if (item.dayOfWeek == 0) {
    } else {
      this.spinnerService.show();
      let _paramGridDetail = <ReportGridDetailParam>{
        year: new Date(item.leaveDate).getFullYear(),
        month: new Date(item.leaveDate).getMonth() + 1,
        day: new Date(item.leaveDate).getDate(),
        index: this.param.index,
        id: this.param.id,
        language: this.param.lang,
      };
      this.reportShowService.reportGridDetail(_paramGridDetail).subscribe({
        next: (res) => {
          this.reportGridDetails = res;
          var initialState: ModalOptions = {
            initialState: {
              items: this.reportGridDetails,
              title:
                this.title[
                this.param.lang == LangConstants.ZH_TW
                  ? 'zh_TW'
                  : this.param.lang
                ],
              leaveDay:
                _paramGridDetail.day +
                '/' +
                _paramGridDetail.month +
                '/' +
                _paramGridDetail.year,
            },
          };
          this.bsModalRef = this.modalService.show(
            PopupReportGridDetailComponent,
            initialState
          );
          this.bsModalRef.setClass('modal-xl');
          this.spinnerService.hide();
        },
        error: (e) => {
          this.snotifyService.error(
            this.translateService.instant('System.Message.SystemError'),
            this.translateService.instant('System.Caption.Error')
          );
          this.spinnerService.hide();
        },
      });
    }
  }

  onExportExcel() {
    this.spinnerService.show();
    let dataExport = <ExportExcelGridDto>{
      leaveDate: this.translateService.instant('Report.ReportShow_ViewData.Date'),
      sEAMP: this.translateService.instant('Report.ReportShow_ViewData.SEAMP'),
      applied: this.translateService.instant('Report.ReportShow_ViewData.Applied'),
      approved: this.translateService.instant('Report.ReportShow_ViewData.Approved'),
      actual: this.translateService.instant('Report.ReportShow_ViewData.Actual'),
      mPPoolOut: this.translateService.instant('Report.ReportShow_ViewData.MPPoolOut'),
      mPPoolIn: this.translateService.instant('Report.ReportShow_ViewData.MPPoolIn'),
      total: this.translateService.instant('Report.ReportShow_ViewData.Total'),
      reportIndexViewModelDTO: this.dataToExports,
    };

    this.reportShowService.exportExcel(dataExport).subscribe({
      next: (result) => {
        this.spinnerService.hide();
        const timeNow = new Date();
        const fileName =
          this.title[
            this.param.lang == LangConstants.ZH_TW ? 'zh_TW' : this.param.lang
          ].trim() +
          '_' +
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

  reportShow() {
    this.spinnerService.show();
    this.reportShowService.reportShow(this.param).subscribe({
      next: (res) => {
        this.reportIndexViewModels = res.listReportShowModel;
        this.title = res.title;
        this.listParents = res.listParent;
        this.dataToExports = res;
        this.dataToExports.lang = this.param.lang;
        this.dataToExports.titleExportExcel =
          res.title[
          this.param.lang == LangConstants.ZH_TW ? 'zh_TW' : this.param.lang
          ];
        this.spinnerService.hide();
      },
      error: (error) => {
        this.snotifyService.error(
          this.translateService.instant('System.Message.SystemError'),
          this.translateService.instant('System.Caption.Error')
        );
        this.spinnerService.hide();
      },
    });
  }
  // Click button applied dạng table hiển thị theo tháng
  onShowDateApplied(item: ReportShowModel) {
    this.spinnerService.show();
    let _paramShowDateDetail = <ReportDateDetailParam>{
      year: new Date(item.leaveDate).getFullYear(),
      month: new Date(item.leaveDate).getMonth() + 1,
      day: new Date(item.leaveDate).getDate(),
      language: this.param.lang,
      statusline: 1,
      index: this.param.index,
      id: this.param.id,
    };
    this.reportShowService.reportDateDetail(_paramShowDateDetail).subscribe({
      next: (res) => {
        let _data = <ReportDateDetail>{
          title:
            this.title[
            this.param.lang == LangConstants.ZH_TW ? 'zh_TW' : this.param.lang
            ],
          lang: _paramShowDateDetail.language,
          leaveDate:
            _paramShowDateDetail.day +
            '/' +
            _paramShowDateDetail.month +
            '/' +
            _paramShowDateDetail.year,
          listReportShowDateDetail: res,
        };
        let _dataExport = <ExportExcelDateDto>{
          no: this.translateService.instant('Report.ReportShow_ViewData.NO'),
          employeeNumber: this.translateService.instant(
            'Report.ReportShow_ViewData.EmpID'
          ),
          employeeName: this.translateService.instant(
            'Report.ReportShow_ViewData.FullName'
          ),
          partCode: this.translateService.instant(
            'Report.ReportShow_ViewData.Department'
          ),
          employeePostition: this.translateService.instant(
            'Report.ReportShow_ViewData.Position'
          ),
          leaveType: this.translateService.instant(
            'Report.ReportShow_ViewData.LeaveCategory'
          ),
          time_Of_Leave: this.translateService.instant(
            'Report.ReportShow_ViewData.TimeOfLeave'
          ),
          status: this.translateService.instant('Report.ReportShow_ViewData.Status'),
          fromDate: this.translateService.instant(
            'Report.ReportShow_ViewData.FromDate'
          ),
          endDate: this.translateService.instant('Report.ReportShow_ViewData.ToDate'),
          reportDateDetailDTO: _data,
        };
        var initialState: ModalOptions = {
          initialState: { items: _dataExport },
        };
        this.bsModalRef = this.modalService.show(
          PopupReportDateDetailComponent,
          initialState
        );
        this.bsModalRef.setClass('modal-xl');
        this.spinnerService.hide();
      },
      error: (e) => {
        this.snotifyService.error(
          this.translateService.instant('System.Message.SystemError'),
          this.translateService.instant('System.Caption.Error')
        );
        this.spinnerService.hide();
      },
    });
  }
  // Click button approved table hiên thị theo tháng
  onShowDateApproved(item: ReportShowModel) {
    this.spinnerService.show();
    let _paramShowDateDetail = <ReportDateDetailParam>{
      year: new Date(item.leaveDate).getFullYear(),
      month: new Date(item.leaveDate).getMonth() + 1,
      day: new Date(item.leaveDate).getDate(),
      language: this.param.lang,
      statusline: 2,
      index: this.param.index,
      id: this.param.id,
    };

    this.reportShowService.reportDateDetail(_paramShowDateDetail).subscribe({
      next: (res) => {
        let _data = <ReportDateDetail>{
          title:
            this.title[
            this.param.lang == LangConstants.ZH_TW ? 'zh_TW' : this.param.lang
            ],
          lang: _paramShowDateDetail.language,
          leaveDate:
            _paramShowDateDetail.day +
            '/' +
            _paramShowDateDetail.month +
            '/' +
            _paramShowDateDetail.year,
          listReportShowDateDetail: res,
        };
        let _dataToExport = <ExportExcelDateDto>{
          no: this.translateService.instant('Report.ReportShow_ViewData.NO'),
          employeeNumber: this.translateService.instant(
            'Report.ReportShow_ViewData.EmpID'
          ),
          employeeName: this.translateService.instant(
            'Report.ReportShow_ViewData.FullName'
          ),
          partCode: this.translateService.instant(
            'Report.ReportShow_ViewData.Department'
          ),
          employeePostition: this.translateService.instant(
            'Report.ReportShow_ViewData.Position'
          ),
          leaveType: this.translateService.instant(
            'Report.ReportShow_ViewData.LeaveCategory'
          ),
          time_Of_Leave: this.translateService.instant(
            'Report.ReportShow_ViewData.TimeOfLeave'
          ),
          status: this.translateService.instant('Report.ReportShow_ViewData.Status'),
          fromDate: this.translateService.instant(
            'Report.ReportShow_ViewData.FromDate'
          ),
          endDate: this.translateService.instant('Report.ReportShow_ViewData.ToDate'),
          reportDateDetailDTO: _data,
        };

        var initialState: ModalOptions = {
          initialState: { items: _dataToExport },
        };
        this.bsModalRef = this.modalService.show(
          PopupReportDateDetailComponent,
          initialState
        );
        this.bsModalRef.setClass('modal-xl');
        this.spinnerService.hide();
      },
      error: (e) => {
        this.snotifyService.error(
          this.translateService.instant('System.Message.SystemError'),
          this.translateService.instant('System.Caption.Error')
        );
        this.spinnerService.hide();
      },
    });
  }

  onBack = () => {
    this.router.navigate(['report']);
  };
}
