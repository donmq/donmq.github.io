import { Component, OnInit } from '@angular/core';
import { LeavePersonalService } from '@services/leave/leave-personal.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { of, takeUntil } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { EmployeeData } from '@models/leave/personal/employeeData';
import { HistoryEmployee } from '@models/leave/personal/historyEmployee';
import { LeaveDataViewModel } from '@models/leave/leaveDataViewModel';
import { LeavePersonal } from '@models/leave/personal/leavePersonal';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { LeaveCommonService } from '@services/common/leave-common.service';
import { Holiday } from '@models/leave/personal/holiday';
import { CalculatorLeaveHoursUtility } from '@utilities/calculator-leave-hours-utility';
import { DestroyService } from '@services/destroy.service';
import { DateTimePickerOption } from '@models/leave/personal/dateTimePickerOption';
import { InjectBase } from '@utilities/inject-base-app';
import { LunchBreakService } from '@services/manage/lunch-break.service';
import { LunchBreak } from '@models/common/lunch-break';

@Component({
  selector: 'app-personal-main',
  templateUrl: './personal-main.component.html',
  styleUrls: ['./personal-main.component.scss'],
  providers: [DestroyService],
})
export class PersonalMainComponent extends InjectBase implements OnInit {
  leavePersonal: LeavePersonal = <LeavePersonal>{};
  listCategory: KeyValuePair[] = [];
  employee: EmployeeData = <EmployeeData>{};
  leaveData: LeaveDataViewModel[] = [];
  history: HistoryEmployee = <HistoryEmployee>{};
  holidays: Holiday[] = [];
  listLunchTime: LunchBreak[] = [];
  leaveHours: number = null;
  leaveHoursView: string = null;
  leaveDay: number = null;
  leaveDayView: string = null;
  leaveRestAgent: number | null = null;
  leaveGranted: boolean = true;
  check: boolean;
  checkError: string = '';
  stepMinute: number = 1;
  startAt: Date = new Date();
  endAt: Date = new Date();
  language: string = localStorage
    .getItem(LocalStorageConstants.LANG)
    ?.toLowerCase();
  filterDay: number[] = [];

  // Time Start
  options: DateTimePickerOption = <DateTimePickerOption>{};
  // Time End
  optionsTimeEnd: DateTimePickerOption = <DateTimePickerOption>{};

  isLoading: boolean = false;
  isRequiredComment: boolean = false;

  constructor(
    private leavePersonalService: LeavePersonalService,
    private calculatorLeaveHoursUtility: CalculatorLeaveHoursUtility,
    private leaveCommonService: LeaveCommonService,
    private lunchBreakService: LunchBreakService
  ) {
    super();
  }

  ngOnInit(): void {
    this.getAllCategory();
    this.getListLunchBreak();
    this.getHoliday();
    this.getData();
    this.setOption();
    this.refreshForm();
    this.checkData();

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

  back() {
    this.router.navigate(['leave']);
  }

  getAllCategory() {
    this.leaveCommonService.getAllCategory(this.language).subscribe({
      next: (res) => {
        this.listCategory = res;
      },
      error: () => {
        this.snotifyService.error(
          this.translateService.instant('System.Message.UnknowError'),
          this.translateService.instant('System.Caption.Error')
        );
      },
    });
  }

  getHoliday() {
    this.leaveCommonService.getListHoliday().subscribe({
      next: (res) => {
        this.holidays = res;
      },
      error: () => {
        this.snotifyService.error(
          this.translateService.instant('System.Message.UnknowError'),
          this.translateService.instant('System.Caption.Error')
        );
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
    let optionCate: string = this.listCategory.find((x) => x.key == this.leavePersonal.cateID).value.split(' - ')[0];

    // Set required Comment (Ghi chú) khi Loại phép công tác
    this.isRequiredComment = (optionCate == 'D');

    this.leavePersonal.time_Start = null
    this.leavePersonal.time_End = null

    if (optionCate == 'A' || optionCate == '7' || optionCate == 'B')
      this.stepMinute = 5;
    else this.stepMinute = 15;
    this.startAt = this.calculatorLeaveHoursUtility.checkTime(
      this.stepMinute,
      this.leavePersonal.time_Start as string
    );

  }

  validateTime(propName: string) {
    if (this.leavePersonal[propName]) {
      const leaveTime = new Date(this.leavePersonal[propName]);
      const option = this.leavePersonal.time_Lunch;

      // Không phải [Ca đêm] và ngày hiện tại là [chủ nhật]
      if (option !== "C" && leaveTime.getDay() === 0 && this.employee.isSun != true) {
        this.leavePersonal[propName] = null;
        this.leaveDayView = null;
        this.leaveHoursView = null;
      } else {
        this.leavePersonal[propName] = leaveTime
      }
    }
  }

  changeTimeLunch() {
    this.leavePersonal.comment = '';
    this.leavePersonal.time_End = null;
    this.leavePersonal.time_Start = null;
    this.leaveDay = this.leaveDayView = null;
    this.leaveHours = this.leaveHoursView = null;

    let optionLunch: string = this.leavePersonal.time_Lunch;
    this.filterDay = optionLunch === 'C' || this.employee.isSun === true ? [] : [0]; // Nếu là ca đêm thì cho phép xin chủ nhật, ngược lại thì k cho xin chủ nhật

    this.options.daysOfWeekDisabled = this.filterDay;
    this.optionsTimeEnd.daysOfWeekDisabled = this.filterDay;
  }

  changeDate() {
    this.startAt = this.calculatorLeaveHoursUtility.checkTime(
      this.stepMinute,
      this.leavePersonal.time_Start as string
    );
    this.endAt = new Date(
      this.startAt.setMinutes(this.startAt.getMinutes() + this.stepMinute)
    );

    this.optionsTimeEnd.minDate = this.startAt;
    this.optionsTimeEnd.stepping = this.stepMinute;
    this.options.stepping = this.stepMinute;

    this.leavePersonal.time_End = null;
    this.leaveHours = this.leaveHoursView = null;
    this.leaveDay = this.leaveDayView = null;
  }

  getLeaveHourAndDay() {
    if (
      this.leavePersonal.time_Start != null &&
      this.leavePersonal.time_End != null
    ) {
      let shift = this.leavePersonal.time_Lunch;
      let dateRanger = this.calculatorLeaveHoursUtility.rangerDate(
        shift,
        new Date(this.leavePersonal.time_Start),
        new Date(this.leavePersonal.time_End),
        this.holidays,
        this.employee.isSun
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

  getData() {
    this.spinnerService.show();
    this.leavePersonalService.getData().subscribe({
      next: (res) => {
        this.spinnerService.hide();
        if (res == null)
          this.router.navigate(['/login']);
        else {
          this.employee = res.employee;
          this.leaveData = res.leaveDataViewModel;
          this.history = <HistoryEmployee>{
            totalDay: (+res.history.totalDay)?.toFixed(5).replace(/\.?0*$/g, ''),
            countAgent: `${(+res.history.countAgent.split('/')[0])
              ?.toFixed(5)
              .replace(/\.?0*$/g, '')} / ${res.history.countAgent.split('/')[1]}`,
            countArran: `${(+res.history.countArran.split('/')[0])
              ?.toFixed(5)
              .replace(/\.?0*$/g, '')} / ${res.history.countArran.split('/')[1]}`,
            countLeave: (+res.history.countLeave)
              ?.toFixed(5)
              .replace(/\.?0*$/g, ''),
            countTotal: (+res.history.countTotal)
              ?.toFixed(5)
              .replace(/\.?0*$/g, ''),
            countRestAgent: (+res.history.countRestAgent)
              ?.toFixed(5)
              .replace(/\.?0*$/g, ''),
            countRestArran: (+res.history.countRestArran)
              ?.toFixed(5)
              .replace(/\.?0*$/g, ''),
          };
        }
      },
      error: () => {
        this.spinnerService.hide();
        this.snotifyService.error(
          this.translateService.instant('System.Message.UnknowError'),
          this.translateService.instant('System.Caption.Error')
        );
      },
    });
  }
  async checkData() {
    await this.leaveCommonService.checkData().subscribe({
      next: res => {
        this.check = res
      }
    })
  }

  validateSave() {
    return (this.leavePersonal.time_End == null
      || (this.leaveDayView == '0' && this.leaveHoursView == '0')
      // || (this.leavePersonal.cateID == 3 && this.functionUtility.checkEmpty(this.leavePersonal.comment)) // Loại công tác
      || this.isLoading)
  }

  async saveAddLeave() {
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
      this.leavePersonal.time_Start as string,
      this.leavePersonal.time_End as string
    ));

    let optionCate: string = this.listCategory
      .find((x) => x.key == this.leavePersonal.cateID)
      .value.split(' - ')[0];

    // Kiểm tra nếu loại phép [D - Công tác] (Chưa nhập ghi chú)
    if (optionCate == 'D' && this.functionUtility.checkEmpty(this.leavePersonal.comment)) {
      // Trả về thông báo lỗi [Phép công tác bắt buộc phải nhập ghi chú]
      this.isLoading = false;
      return this.snotifyService.warning(
        this.translateService.instant('Leave.ErrorMessage.OfficialLeaveMustHaveComment'),
        this.translateService.instant('System.Caption.Warning')
      );
    }

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
        this.translateService.instant(
          'Leave.ErrorMessage.PersonalLeaveUnit'
        ),
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
        this.translateService.instant(
          'Leave.ErrorMessage.AnnualLeaveNotEnough'
        ),
        this.translateService.instant('System.Caption.Error')
      );
    }

    //Nếu check = true là nhân sự đang bật chức năng bắt buộc sử dụng hết phép năm
    if (this.check == false) {
      //Nếu là việc riêng, Sl phép năm còn lại phải nhỏ hơn 1
      if (optionCate == 'A' && this.leavePersonal.cateID == 2 && this.leaveRestAgent >= 1) {
        this.isLoading = false;
        return this.snotifyService.error(this.translateService.instant('Leave.ErrorMessage.AnnualLeaveEnough'), this.translateService.instant('System.Caption.Error'));
      }
    } else {
      // khi check = false là nhân sự đang tắt chức năng cho phép xin VR khi còn PN, ko bắt buộc sử dụng hết phép năm rồi mới được xin phép VR
      if (optionCate == 'A' && this.leavePersonal.cateID == 2 && this.leaveRestAgent >= 0.03125) {
        this.isLoading = false;
        return this.snotifyService.error(this.translateService.instant('Leave.ErrorMessage.AnnualLeaveEnough'), this.translateService.instant('System.Caption.Error'));
      }
    }

    //Nếu chọn phép năm thì chỉ được xin trong 1 năm, không được xin năm này sang năm khác, chức năng xin phép năm từ năm này sang năm khác
    if (
      optionCate == 'J' &&
      new Date(this.leavePersonal.time_Start).getFullYear() !=
      new Date(this.leavePersonal.time_End).getFullYear()
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
        this.translateService.instant(
          'Leave.ErrorMessage.ExistLeaveWaitLock'
        ),
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
    this.leavePersonal.empID = this.employee.empID;
    this.leavePersonal.leaveDay = this.leaveDay.toString();
    this.leavePersonal.ipLocal = localStorage.getItem(LocalStorageConstants.IPLOCAL);
    this.leavePersonalService.addLeaveDataPersonal(this.leavePersonal).subscribe({
      next: (res) => {
        if (res) {
          this.getData();
          this.refreshForm();
        } else
          this.snotifyService.error(
            this.translateService.instant('System.Message.CreateErrorMsg'),
            this.translateService.instant('System.Caption.Error')
          );
        this.spinnerService.hide();
        this.isLoading = false;
      },
      error: () => {
        this.isLoading = false;

        this.snotifyService.error(
          this.translateService.instant('System.Message.UnknowError'),
          this.translateService.instant('System.Caption.Error')
        );
        this.spinnerService.hide();
      },
    });
  }

  async checkDateLeave() {
    await this.leaveCommonService
      .checkDateLeave(
        this.functionUtility.getDateTimeFormat(
          new Date(this.leavePersonal.time_Start)
        ),
        this.functionUtility.getDateTimeFormat(
          new Date(this.leavePersonal.time_End)
        ),
        this.employee.empID
      )
      .pipe(
        tap((res) => {
          this.spinnerService.hide();
          this.checkError = res.result;
        }),
        catchError(() => {
          this.snotifyService.error(
            this.translateService.instant('System.Message.UnknowError'),
            this.translateService.instant('System.Caption.Error')
          );
          this.spinnerService.hide();

          this.isLoading = false;
          return of(null);
        })
      )
      .toPromise();
  }

  async getCountRestAgent() {
    this.leaveGranted = true;
    await this.leaveCommonService
      .getCountRestAgent(
        this.employee.empID,
        new Date(this.leavePersonal.time_Start).getFullYear()
      )
      .pipe(
        tap((res) => {
          if (res != null) this.leaveRestAgent = res;
          else {
            if (this.leavePersonal.cateID == 1) {
              this.leaveGranted = false;
            }
          }
          this.spinnerService.hide();
        }),
        catchError(() => {
          this.snotifyService.error(
            this.translateService.instant('System.Message.UnknowError'),
            this.translateService.instant('System.Caption.Error')
          );
          this.spinnerService.hide();
          this.isLoading = false;
          return of(null);
        })
      )
      .toPromise();
  }

  delete(item: LeaveDataViewModel) {
    this.snotifyService.confirm(
      this.translateService.instant('System.Message.ConfirmDeleteMsg'),
      this.translateService.instant('System.Caption.Confirm'),
      () => {
        this.spinnerService.show();
        this.leavePersonalService
          .deleteLeaveDataPerson(item.leaveID, item.empID)
          .subscribe({
            next: (res) => {
              if (res) this.getData();
              else
                this.snotifyService.error(
                  this.translateService.instant('System.Message.DeleteErrorMsg'),
                  this.translateService.instant('System.Caption.Error')
                );
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
    );
  }

  refreshForm() {
    this.leavePersonal = <LeavePersonal>{
      cateID: null,
      comment: '',
      leaveDay: null,
      time_End: null,
      time_Lunch: '0',
      time_Start: null,
    };
    this.leaveDay = this.leaveDayView = null;
    this.leaveHours = this.leaveHoursView = null;
    this.stepMinute = null;
    this.isLoading = false;
    this.isRequiredComment = false;
  }

  setOption() {
    this.options = this.functionUtility.getDateTimePickerOption();
    this.optionsTimeEnd = this.functionUtility.getDateTimePickerOption();

    this.options.locale = this.language;
    this.optionsTimeEnd.locale = this.language;
  }
}

