import { Component, OnInit } from '@angular/core';
import * as moment from 'moment';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { AddManuallyViewModel } from '@models/seahr/seahr/add-manually-view-model';
import { AddManuallyParam } from '@params/seahr/add-manually-param';
import { KeyValuePair } from '@utilities/key-value-pair';
import { SeaHrAddManuallyService } from '@services/seahr/sea-hr-add-manually.service';
import { CalculatorLeaveHoursUtility } from '@utilities/calculator-leave-hours-utility';
import { LeaveCommonService } from '@services/common/leave-common.service';
import { Holiday } from '@models/leave/personal/holiday';
import { DestroyService } from '@services/destroy.service';
import { of, takeUntil } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { DateTimePickerOption } from '@models/leave/personal/dateTimePickerOption';
import { InjectBase } from '@utilities/inject-base-app';
import { LunchBreak } from '@models/common/lunch-break';
import { LunchBreakService } from '@services/manage/lunch-break.service';

@Component({
  selector: 'app-seahr-add-manually',
  templateUrl: './seahr-add-manually.component.html',
  styleUrls: ['./seahr-add-manually.component.scss'],
  providers: [DestroyService],
})
export class SeahrAddManuallyComponent extends InjectBase implements OnInit {
  data: AddManuallyViewModel[] = [];
  listCategory: KeyValuePair[] = [];
  leaveHours: number = null;
  leaveHoursView: string = null;
  leaveDay: number = null;
  leaveDayView: string = null;
  addManuallyParam: AddManuallyParam = <AddManuallyParam>{};
  stepMinute: number = 0;
  startAt: Date = new Date();
  endAt: Date = new Date();
  language: string = localStorage
    .getItem(LocalStorageConstants.LANG)
    ?.toLowerCase();
  holidays: Holiday[] = [];
  listLunchTime: LunchBreak[] = [];
  leaveRestAgent: number | null = null;
  leaveGranted: boolean = true;
  checkError: any = '';
  filterDay: number[] = [];
  check: boolean;
  isSun: boolean = false;
  // Time Start
  options: DateTimePickerOption = <DateTimePickerOption>{};
  // Time End
  optionsTimeEnd: DateTimePickerOption = <DateTimePickerOption>{};

  isLoading: boolean = false;

  constructor(
    private _service: SeaHrAddManuallyService,
    private calculatorLeaveHoursUtility: CalculatorLeaveHoursUtility,
    private leaveCommonService: LeaveCommonService,
    private lunchBreakService: LunchBreakService
  ) {
    super()
  }

  ngOnInit() {
    this.getHoliday();
    this.changeLang();
    this.getAllCategory();
    this.setOption()
    this.getListLunchBreak();
    this.clearForm();
  }

  changeLang() {
    this.translateService.onLangChange
      .pipe(takeUntil(this.destroyService.destroys$))
      .subscribe((event) => {
        this.language = event.lang;
        this.options.locale = this.language;
        this.optionsTimeEnd.locale = this.language;
        this.getAllCategory();
        this.getListLunchBreak();
      });
  }

  getHoliday() {
    this.leaveCommonService.getListHoliday().subscribe({
      next: (res) => {
        this.holidays = res;
      },
      error: () => {
        this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
      },
    });
  }

  getAllCategory() {
    this._service.getAllCategory(this.language).subscribe({
      next: (res) => {
        this.listCategory = res;
      },
      error: () => {
        this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
      },
    });
  }

  getListLunchBreak() {
    this.lunchBreakService.getListLunchBreak()
      .subscribe({
        next: (res) => {
          this.listLunchTime = res;
          this.listLunchTime.unshift({
            key: '0',
            value_vi: 'Chọn giờ nghỉ giữa ca',
            value_en: 'Select the Break Time',
            value_zh: '請選擇吃飯時間'
          } as LunchBreak);
        },
        error: () => {
          this.snotifyService.error(
            this.translateService.instant('System.Message.UnknowError'),
            this.translateService.instant('System.Caption.Error')
          );
        }
      })
  }

  getStepMinute() {
    this.checkData();
    let optionCate = this.listCategory
      .find((x) => x.key == this.addManuallyParam.slCategoryPersonal)
      .value.split(' - ')[0];
    this.addManuallyParam.txtPersonalBeign = null
    this.addManuallyParam.txtPersonalEnd = null
    if (optionCate == 'A' || optionCate == '7' || optionCate == 'B')
      this.stepMinute = 5;
    else this.stepMinute = 15;
    this.startAt = this.calculatorLeaveHoursUtility.checkTime(
      this.stepMinute,
      this.addManuallyParam.txtPersonalBeign as string
    );

  }

  validateTime(propName: string) {
    if (this.addManuallyParam[propName]) {
      const leaveTime = new Date(this.addManuallyParam[propName]);
      const option = this.addManuallyParam.lunchTime;

      if (option !== "C" && leaveTime.getDay() === 0 && this.isSun != true) {
        this.addManuallyParam[propName] = null;
        this.leaveDayView = null;
        this.leaveHoursView = null;
      } else {
        this.addManuallyParam[propName] = leaveTime
      }
    }
  }

  changeDate() {
    this.startAt = this.calculatorLeaveHoursUtility.checkTime(
      this.stepMinute,
      this.addManuallyParam.txtPersonalBeign as string
    );
    this.endAt = new Date(
      this.startAt.setMinutes(this.startAt.getMinutes() + this.stepMinute)
    );
    // set giờ và stepping
    this.optionsTimeEnd.minDate = this.startAt;
    this.optionsTimeEnd.stepping = this.stepMinute;
    this.options.stepping = this.stepMinute;

    this.addManuallyParam.txtPersonalEnd = null;
    this.leaveDay = this.leaveDayView = null;
    this.leaveHours = this.leaveHoursView = null;
  }

  getLeaveHourAndDay() {
    if (
      this.addManuallyParam.txtPersonalBeign != null &&
      this.addManuallyParam.txtPersonalEnd != null
    ) {
      let shift = this.addManuallyParam.lunchTime;
      let dateRanger = this.calculatorLeaveHoursUtility.rangerDate(
        shift,
        new Date(this.addManuallyParam.txtPersonalBeign),
        new Date(this.addManuallyParam.txtPersonalEnd),
        this.holidays,
        this.isSun
      );
      this.leaveHours = this.calculatorLeaveHoursUtility.sumTotalLeave(
        dateRanger,
        shift
      );
      this.leaveHoursView = this.leaveHours.toFixed(5).replace(/\.?0*$/g, '');
      this.leaveDay = this.calculatorLeaveHoursUtility.convertHourToDay(
        this.leaveHours
      );
      this.leaveDayView = this.leaveDay.toFixed(5).replace(/\.?0*$/g, '');
    }
  }
  lunchTimeChange(e) {
    this.addManuallyParam.txtPersonalBeign = null;
    this.addManuallyParam.txtPersonalEnd = null;
    this.leaveDay = this.leaveDayView = null;
    this.leaveHours = this.leaveHoursView = null;
    let optionLunch: string = this.addManuallyParam.lunchTime;
    // Kiểm tra nếu isSun = true hoặc optionLunch == C thì không vô hiệu hóa Chủ nhật
    this.filterDay = optionLunch === 'C' || this.isSun == true ? [] : [0];

    this.options.daysOfWeekDisabled = this.filterDay;
    this.optionsTimeEnd.daysOfWeekDisabled = this.filterDay;
  }

  checkIsSun(empNumber: string) {
    this.addManuallyParam.lunchTime = '0'
    this._service.checkIsSun(empNumber).subscribe({
      next: res => {
        this.isSun = res
      }
    })
  }
  async checkData() {
    await this.leaveCommonService.checkData().subscribe({
      next: res => {
        this.check = res
      }
    })
  }

  async save() {
    this.isLoading = true;
    await this.checkData();
    if (this.leaveDay <= 0) {
      this.isLoading = false;

      return this.snotifyService.error(
        this.translateService.instant('Leave.ErrorMessage.DateError'),
        this.translateService.instant('System.Caption.Error')
      );
    }

    let minutes: number = Math.round(this.calculatorLeaveHoursUtility.getMinutes(
      this.addManuallyParam.txtPersonalBeign as string,
      this.addManuallyParam.txtPersonalEnd as string
    ));
    let optionCate: string = this.listCategory
      .find((x) => x.key == this.addManuallyParam.slCategoryPersonal)
      .value.split(' - ')[0];
    if (minutes < 15 && optionCate == 'J') {
      this.isLoading = false;
      return this.snotifyService.error(
        this.translateService.instant('Leave.ErrorMessage.AnnualLeaveUnit'),
        this.translateService.instant('System.Caption.Error')
      );
    }
    else if (minutes < 5 && (optionCate == 'A' || optionCate == '7' || optionCate == 'B')) {
      this.isLoading = false;
      return this.snotifyService.error(
        this.translateService.instant('Leave.ErrorMessage.PersonalLeaveUnit'),
        this.translateService.instant('System.Caption.Error')
      );
    }

    await this.getCountRestAgent();

    //Nếu leaveRestAgent == 0 và biến leaveGranted = false nghĩa là chọn xin phép năm mà chưa được nhân sự cấp phép thì không xin được
    if ((this.leaveRestAgent == 0 || this.leaveRestAgent == null) && this.leaveGranted == false) {
      this.isLoading = false;
      return this.snotifyService.error(
        this.translateService.instant('Leave.ErrorMessage.NotGranted'),
        this.translateService.instant('System.Caption.Error')
      );
    }

    //nếu phép cá nhân chưa nghỉ nhỏ hơn hoặc bằng 0 và chọn phép năm
    if (this.leaveRestAgent <= 0 && optionCate == 'J') {
      this.isLoading = false;
      return this.snotifyService.error(
        this.translateService.instant('Leave.ErrorMessage.AnnualLeave'),
        this.translateService.instant('System.Caption.Error')
      );
    }

    // Phép năm k còn đủ, phải chọn loại phép khác hoặc sử dụng đúng thời gian còn lại của phép năm
    if (optionCate == 'J' && this.leaveRestAgent < this.leaveDay) {
      this.isLoading = false;
      return this.snotifyService.error(
        this.translateService.instant('Leave.ErrorMessage.AnnualLeaveNotEnough'),
        this.translateService.instant('System.Caption.Error')
      );
    }

    //Nếu check = true là nhân sự đang bật chức năng bắt buộc sử dụng hết phép năm
    if (this.check == false) {
      //Nếu là việc riêng, Sl phép năm còn lại phải nhỏ hơn 1
      if (optionCate == 'A' && this.addManuallyParam.slCategoryPersonal == 2 && this.leaveRestAgent >= 1) {
        this.isLoading = false;
        return this.snotifyService.error(this.translateService.instant('Leave.ErrorMessage.AnnualLeaveEnough'), this.translateService.instant('System.Caption.Error'));
      }
    } else {
      // khi check = false là nhân sự đang tắt chức năng cho phép xin VR khi còn PN, ko bắt buộc sử dụng hết phép năm rồi mới được xin phép VR
      if (optionCate == 'A' && this.addManuallyParam.slCategoryPersonal == 2 && this.leaveRestAgent >= 0.03125) {
        this.isLoading = false;
        return this.snotifyService.error(this.translateService.instant('Leave.ErrorMessage.AnnualLeaveEnough'), this.translateService.instant('System.Caption.Error'));
      }
    }

    //Nếu chọn phép năm thì chỉ được xin trong 1 năm, không được xin năm này sang năm khác, chức năng xin phép năm từ năm này sang năm khác
    if (
      optionCate == 'J' &&
      new Date(this.addManuallyParam.txtPersonalBeign).getFullYear() !=
      new Date(this.addManuallyParam.txtPersonalEnd).getFullYear()
    ) {
      this.isLoading = false;
      return this.snotifyService.error(
        this.translateService.instant('Leave.ErrorMessage.AnotherYear'),
        this.translateService.instant('System.Caption.Error')
      );
    }

    //Kiểm tra xem ngày nghỉ đang chọn có trùng với những ngày nghỉ đã đăng ký trước đó không
    await this.checkDateLeave();
    if (this.checkError == 'EXISTLEAVEWAITLOCK') {
      this.isLoading = false;
      return this.snotifyService.error(
        this.translateService.instant('Leave.ErrorMessage.ExistLeaveWaitLock'),
        this.translateService.instant('System.Caption.Error')
      );
    }
    else if (this.checkError == 'ERROR') {
      this.isLoading = false;
      return this.snotifyService.error(
        this.translateService.instant('Leave.ErrorMessage.LeaveDuplicate'),
        this.translateService.instant('System.Caption.Error')
      );
    }

    this.spinnerService.show();

    this.addManuallyParam.leaveday = this.leaveDay.toString();

    this._service.addManually(this.addManuallyParam).subscribe({
      next: (res) => {
        if (res) {
          this.spinnerService.hide();
          if (res.isSuccess == false) {
            this.isLoading = false;
            this.snotifyService.error(this.translateService.instant('System.Message.CreateErrorMsg'),
              this.translateService.instant('System.Caption.Error'));
            return;
          }
          this.loadDataTable(res.data);
          this.clearForm();
          this.snotifyService.success(this.translateService.instant('System.Message.CreateOKMsg'),
            this.translateService.instant('System.Caption.Success'));
        }
      },
      error: () => {
        this.isLoading = false;
        this.spinnerService.hide();
        // this.clearForm();
        this.snotifyService.error(this.translateService.instant('System.Message.DataNotFound'), this.translateService.instant('System.Caption.Error'));
      },
    });
  }

  async checkDateLeave() {
    await this._service
      .checkDateLeave(
        moment(this.addManuallyParam.txtPersonalBeign).format(
          'yyyy/MM/DD HH:mm:ss'
        ),
        moment(this.addManuallyParam.txtPersonalEnd).format(
          'yyyy/MM/DD HH:mm:ss'
        ),
        this.addManuallyParam.empNumber
      )
      .pipe(
        tap((res) => {
          this.spinnerService.hide();
          this.checkError = res.result;
        }),
        catchError(() => {
          this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
          this.spinnerService.hide();
          this.isLoading = false;
          return of(null);
        })
      )
      .toPromise();
  }

  async getCountRestAgent() {
    await this._service
      .getCountRestAgent(
        new Date(this.addManuallyParam.txtPersonalBeign).getFullYear(),
        this.addManuallyParam.empNumber
      )
      .pipe(
        tap((res) => {
          if (res != null) this.leaveRestAgent = res;
          else {
            if (this.addManuallyParam.slCategoryPersonal == 1) {
              this.leaveGranted = false;
            }
          }
          this.spinnerService.hide();
        }),
        catchError(() => {
          this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
          this.spinnerService.hide();
          this.isLoading = false;
          return of(null);
        })
      )
      .toPromise();
  }

  loadDataTable(leaveId: number) {
    if (leaveId) {
      this._service.getDetail(leaveId).subscribe((res) => {
        this.data.push(res);
        this.spinnerService.hide();
      });
    }
  }

  delete(leaveId: string) {
    this.snotifyService.confirm(
      this.translateService.instant('System.Message.ConfirmDeleteMsg'),
      this.translateService.instant('System.Caption.Confirm'),
      () => {
        this.spinnerService.show();
        this._service.delete(leaveId).subscribe({
          next: (res) => {
            this.spinnerService.hide();
            if (res.isSuccess) {
              var findItem = this.data.findIndex((x) => x.leaveId == leaveId);
              this.data.splice(findItem, 1);
              return this.snotifyService.success(this.translateService.instant('System.Message.DeleteOKMsg'), this.translateService.instant('System.Caption.Success'));
            } else
              this.snotifyService.error(this.translateService.instant('System.Message.DeleteErrorMsg'), this.translateService.instant('System.Caption.Error'));
          },
          error: () => {
            this.spinnerService.hide();
            this.snotifyService.error(this.translateService.instant('System.Message.UnknowError'), this.translateService.instant('System.Caption.Error'));
          },
        });
      }
    );
  }

  back() {
    this.router.navigate(['/seahr/']);
  }

  clearForm() {
    this.leaveDay = this.leaveDayView = null;
    this.leaveHours = this.leaveHoursView = null;
    this.addManuallyParam = {
      empNumber: null,
      leaveday: null,
      slCategoryPersonal: null,
      txtPersonalBeign: null,
      txtPersonalEnd: null,
      lunchTime: '0',
      lang: this.language,
      ipLocal: localStorage.getItem(LocalStorageConstants.IPLOCAL),
    };
    this.isLoading = false;
  }
  setOption() {
    this.options = this.functionUtility.getDateTimePickerOption();
    this.optionsTimeEnd = this.functionUtility.getDateTimePickerOption();

    this.options.locale = this.language;
    this.optionsTimeEnd.locale = this.language;
  }
}
