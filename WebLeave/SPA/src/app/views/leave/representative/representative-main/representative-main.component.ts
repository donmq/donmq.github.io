import { Component, OnInit, TemplateRef } from '@angular/core';
import { LeaveCommonService } from '@services/common/leave-common.service';
import { LeaveRepresentativeService } from '@services/leave/leave-representative.service';
import { KeyValuePair } from '@utilities/key-value-pair';
import { LeavePersonal } from '@models/leave/personal/leavePersonal';
import { EmployeeInfo } from '@models/leave/representative/employeeInfo';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { RepresentativeDataViewModel } from '@models/leave/representative/representativeDataViewModel';
import { LeaveDataViewModel } from '@models/leave/leaveDataViewModel';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Holiday } from '@models/leave/personal/holiday';
import { CalculatorLeaveHoursUtility } from '@utilities/calculator-leave-hours-utility';
import { of, takeUntil } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { DestroyService } from '@services/destroy.service';
import { DateTimePickerOption } from '@models/leave/personal/dateTimePickerOption';
import { InjectBase } from '@utilities/inject-base-app';
import { LunchBreak } from '@models/common/lunch-break';
import { LunchBreakService } from '@services/manage/lunch-break.service';

@Component({
  selector: 'app-representative-main',
  templateUrl: './representative-main.component.html',
  styleUrls: ['./representative-main.component.css'],
  providers: [DestroyService]
})
export class RepresentativeMainComponent extends InjectBase implements OnInit {
  leavePersonal: LeavePersonal = <LeavePersonal>{};
  listCategory: KeyValuePair[] = [];
  listLunchTime: LunchBreak[] = [];
  leaveHours: number = null;
  leaveHoursView: string = null;
  leaveDay: number = null;
  leaveDayView: string = null;
  leaveData: RepresentativeDataViewModel[] = [];
  countTotal: number = 0;
  employeeInfo: EmployeeInfo = <EmployeeInfo>{};
  leaveRestAgent: number | null = null;
  leaveGranted: boolean = true;
  checkError: any = '';
  stepMinute: number = 1;
  startAt: Date = new Date();
  endAt: Date = new Date();
  language: string = localStorage.getItem(LocalStorageConstants.LANG)?.toLowerCase();
  checkboxAll: boolean = false;
  listOnTime: LeaveDataViewModel[] = [];
  modalRef?: BsModalRef;
  employeeViewOnTime: RepresentativeDataViewModel = <RepresentativeDataViewModel>{};
  message: string = '';
  holidays: Holiday[] = [];
  filterDay: number[] = [];
  check: boolean;
  // Time Start
  options: DateTimePickerOption = <DateTimePickerOption>{};
  // Time End
  optionsTimeEnd: DateTimePickerOption = <DateTimePickerOption>{};

  isLoading: boolean = false;
  isRequiredComment: boolean = false;

  constructor(
    private calculatorLeaveHoursUtility: CalculatorLeaveHoursUtility,
    private leaveCommonService: LeaveCommonService,
    private modalService: BsModalService,
    private leaveRepresentativeService: LeaveRepresentativeService,
    private lunchBreakService: LunchBreakService
  ) {
    super();
  }

  ngOnInit(): void {
    this.getAllCategory();
    this.getListLunchBreak();
    this.getHoliday();
    this.getDataLeave();
    this.setOption();
    this.refreshForm();
    this.checkData();
    this.options.locale = this.language;
    this.optionsTimeEnd.locale = this.language;
    this.translateService.onLangChange.pipe(takeUntil(this.destroyService.destroys$)).subscribe(event => {
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
    this.leaveCommonService.getAllCategory(this.language)
      .subscribe({
        next: (res) => { this.listCategory = res },
        error: () => {
          this.snotifyService.error(
            this.translateService.instant('System.Message.UnknowError'),
            this.translateService.instant('System.Caption.Error')
          );
        }
      });
  }

  getHoliday() {
    this.leaveCommonService.getListHoliday()
      .subscribe({
        next: (res) => { this.holidays = res },
        error: () => {
          this.snotifyService.error(
            this.translateService.instant('System.Message.UnknowError'),
            this.translateService.instant('System.Caption.Error')
          );
        }
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
    let optionCate = this.listCategory.find(x => x.key == this.leavePersonal.cateID).value.split(" - ")[0];
    // Set required Comment (Ghi chú) khi Loại phép công tác
    this.isRequiredComment = (optionCate == 'D');
    this.leavePersonal.time_Start = null
    this.leavePersonal.time_End = null
    if (optionCate == "A" || optionCate == "7" || optionCate == "B") this.stepMinute = 5;
    else this.stepMinute = 15;
    this.startAt = this.calculatorLeaveHoursUtility.checkTime(this.stepMinute, this.leavePersonal.time_Start as string);
  }

  validateTime(propName: string) {
    if (this.leavePersonal[propName]) {
      const leaveTime = new Date(this.leavePersonal[propName]);
      const option = this.leavePersonal.time_Lunch;

      if (option !== "C" && leaveTime.getDay() === 0 && this.employeeInfo.isSun != true) {
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
    // Kiểm tra nếu employeeInfo.isSun = true hoặc optionLunch == C thì không vô hiệu hóa Chủ nhật
    this.filterDay = optionLunch === 'C' || this.employeeInfo.isSun === true ? [] : [0];


    this.options.daysOfWeekDisabled = this.filterDay;
    this.optionsTimeEnd.daysOfWeekDisabled = this.filterDay;
  }

  changeDate() {
    this.startAt = this.calculatorLeaveHoursUtility.checkTime(this.stepMinute, this.leavePersonal.time_Start as string);
    this.endAt = new Date(this.startAt.setMinutes(this.startAt.getMinutes() + this.stepMinute));

    this.optionsTimeEnd.minDate = this.startAt;
    this.optionsTimeEnd.stepping = this.stepMinute;
    this.options.stepping = this.stepMinute;

    this.leavePersonal.time_End = null;
    this.leaveDay = this.leaveDayView = null;
    this.leaveHours = this.leaveHoursView = null;
  }

  getLeaveHourAndDay() {
    if (this.leavePersonal.time_Start != null && this.leavePersonal.time_End != null) {
      let shift = this.leavePersonal.time_Lunch;
      let dateRanger = this.calculatorLeaveHoursUtility.rangerDate(shift, new Date(this.leavePersonal.time_Start), new Date(this.leavePersonal.time_End), this.holidays, this.employeeInfo.isSun);
      this.leaveHours = this.calculatorLeaveHoursUtility.sumTotalLeave(dateRanger, shift);
      this.leaveHoursView = this.leaveHours.toFixed(5).replace(/\.?0*$/g, '');
      this.leaveDay = this.calculatorLeaveHoursUtility.convertHourToDay(this.leaveHours);
      this.leaveDayView = this.leaveDay.toFixed(5).replace(/\.?0*$/g, '');
    }
  }

  getDataLeave() {
    this.spinnerService.show();
    this.leaveRepresentativeService.getDataLeave()
      .subscribe({
        next: (res) => {
          this.leaveData = res;
          this.spinnerService.hide();
        }, error: () => {
          this.snotifyService.error(
            this.translateService.instant('System.Message.UnknowError'),
            this.translateService.instant('System.Caption.Error')
          );
          this.spinnerService.hide();
        }
      });
  }

  getEmployeeInfo() {
    if (this.leavePersonal.empNumber.length == 5) {
      this.spinnerService.show();
      this.leavePersonal.time_Lunch = '0'
      this.leaveRepresentativeService.getEmployeeInfo(this.leavePersonal.empNumber)
        .subscribe({
          next: (res) => {
            if (res.isSuccess && res.data.empname != null)
              this.employeeInfo = <EmployeeInfo>{
                cagent: +res.data.cagent?.toFixed(5).replace(/\.?0*$/g, ''),
                cagentHour: (res.data.cagent * 8)?.toFixed(5).replace(/\.?0*$/g, ''),
                carrange: +res.data.carrange?.toFixed(5).replace(/\.?0*$/g, ''),
                carrangeHour: (res.data.carrange * 8)?.toFixed(5).replace(/\.?0*$/g, ''),
                empid: res.data.empid,
                empname: res.data.empname,
                isSun: res.data.isSun,
                restedleave: +res.data.restedleave?.toFixed(5).replace(/\.?0*$/g, ''),
                remainingAnnual: +(res.data.totalleave - res.data.counttotal)?.toFixed(5).replace(/\.?0*$/g, ''),
                remainingAnnualHour: ((res.data.totalleave - res.data.counttotal) * 8)?.toFixed(5).replace(/\.?0*$/g, '')
              };
            else
              this.employeeInfo = <EmployeeInfo>{};

            this.spinnerService.hide();
          }, error: () => {
            this.snotifyService.error(
              this.translateService.instant('System.Message.UnknowError'),
              this.translateService.instant('System.Caption.Error')
            );
            this.spinnerService.hide();
          }
        });
    }
    else if (this.leavePersonal.empNumber.length <= 4) {
      this.employeeInfo = <EmployeeInfo>{
        empname: null
      }
    }
  }

  getListOnTime(item: LeaveDataViewModel) {
    this.spinnerService.show();
    this.leaveRepresentativeService.getListOnTime(item.leaveID)
      .subscribe({
        next: (res) => {
          this.listOnTime = res;
          this.spinnerService.hide();
        }, error: () => {
          this.snotifyService.error(
            this.translateService.instant('System.Message.UnknowError'),
            this.translateService.instant('System.Caption.Error')
          );
          this.spinnerService.hide();
        }
      });
  }
  async checkData() {
    await this.leaveCommonService.checkData().subscribe({
      next: res => {
        this.check = res
      }
    })
  }
  async saveAddLeave() {
    this.isLoading = true;
    await this.checkData();

    if (this.leaveDay <= 0) {
      this.isLoading = false;
      return this.snotifyService.error(this.translateService.instant('Leave.ErrorMessage.DateError'), this.translateService.instant('System.Caption.Error'));
    }

    let minutes: number = Math.round(this.calculatorLeaveHoursUtility.getMinutes(this.leavePersonal.time_Start as string, this.leavePersonal.time_End as string));
    let optionCate: string = this.listCategory.find(x => x.key == this.leavePersonal.cateID).value.split(" - ")[0];

    // Kiểm tra nếu loại phép [D - Công tác] (Chưa nhập ghi chú)
    if (optionCate == 'D' && this.functionUtility.checkEmpty(this.leavePersonal.comment)) {
      // Trả về thông báo lỗi [Phép công tác bắt buộc phải nhập ghi chú]
      this.isLoading = false;
      return this.snotifyService.warning(
        this.translateService.instant('Leave.ErrorMessage.OfficialLeaveMustHaveComment'),
        this.translateService.instant('System.Caption.Warning')
      );
    }

    if (minutes < 15 && optionCate == "J") {
      this.isLoading = false;
      return this.snotifyService.error(this.translateService.instant('Leave.ErrorMessage.AnnualLeaveUnit'), this.translateService.instant('System.Caption.Error'));
    }

    else if (minutes < 5 && (optionCate == "A" || optionCate == "7" || optionCate == "B")) {
      this.isLoading = false;
      return this.snotifyService.error(this.translateService.instant('Leave.ErrorMessage.PersonalLeaveUnit'), this.translateService.instant('System.Caption.Error'));
    }

    await this.getCountRestAgent();

    //Nếu leaveRestAgent == 0 và biến leaveGranted = false nghĩa là chọn xin phép năm mà chưa được nhân sự cấp phép thì không xin được
    if ((this.leaveRestAgent == 0 || this.leaveRestAgent == null) && this.leaveGranted == false) {
      this.isLoading = false;
      return this.snotifyService.error(this.translateService.instant('Leave.ErrorMessage.NotGranted'), this.translateService.instant('System.Caption.Error'));
    }

    //nếu phép cá nhân chưa nghỉ nhỏ hơn hoặc bằng 0 và chọn phép năm
    if (this.leaveRestAgent <= 0 && optionCate == "J") {
      this.isLoading = false;
      return this.snotifyService.error(this.translateService.instant('Leave.ErrorMessage.AnnualLeave'), this.translateService.instant('System.Caption.Error'));
    }

    // Phép năm k còn đủ, phải chọn loại phép khác hoặc sử dụng đúng thời gian còn lại của phép năm
    if (optionCate == "J" && this.leaveRestAgent < this.leaveDay) {
      this.isLoading = false;
      return this.snotifyService.error(this.translateService.instant('Leave.ErrorMessage.AnnualLeaveNotEnough'), this.translateService.instant('System.Caption.Error'));
    }

    //Nếu check = true là nhân sự đang bật chức năng bắt buộc sử dụng hết phép năm
    if (this.check == false) {
      //Nếu là việc riêng, Sl phép năm còn lại phải nhỏ hơn 1
      if (optionCate == "A" && this.leavePersonal.cateID == 2 && this.leaveRestAgent >= 1) {
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
    if (optionCate == "J" && (new Date(this.leavePersonal.time_Start)).getFullYear() != (new Date(this.leavePersonal.time_End)).getFullYear()) {
      this.isLoading = false;
      return this.snotifyService.error(this.translateService.instant('Leave.ErrorMessage.AnotherYear'), this.translateService.instant('System.Caption.Error'));
    }

    //Kiểm tra xem ngày nghỉ đang chọn có trùng với những ngày nghỉ đã đăng ký trước đó không
    await this.checkDateLeave();
    if (this.checkError == "EXISTLEAVEWAITLOCK") {
      this.isLoading = false;
      return this.snotifyService.error(this.translateService.instant('Leave.ErrorMessage.ExistLeaveWaitLock'), this.translateService.instant('System.Caption.Error'));
    }
    else if (this.checkError == "ERROR") {
      this.isLoading = false;
      return this.snotifyService.error(this.translateService.instant('Leave.ErrorMessage.LeaveDuplicate'), this.translateService.instant('System.Caption.Error'));
    }

    this.spinnerService.show();
    this.leavePersonal.empID = this.employeeInfo.empid;
    this.leavePersonal.leaveDay = this.leaveDay.toString();
    this.leavePersonal.ipLocal = localStorage.getItem(LocalStorageConstants.IPLOCAL);
    this.leaveRepresentativeService.addLeaveData(this.leavePersonal)
      .subscribe({
        next: (res) => {
          if (res) {
            this.getDataLeave();
            this.refreshForm();
          }
          else
            this.snotifyService.error(
              this.translateService.instant('System.Message.CreateErrorMsg'),
              this.translateService.instant('System.Caption.Error')
            );
          this.spinnerService.hide();
          this.isLoading = false;
        }, error: () => {
          this.snotifyService.error(
            this.translateService.instant('System.Message.UnknowError'),
            this.translateService.instant('System.Caption.Error')
          );
          this.spinnerService.hide();
          this.isLoading = false;
        }
      });
  }

  async checkDateLeave() {
    await this.leaveCommonService
      .checkDateLeave(
        this.functionUtility.getDateTimeFormat(new Date(this.leavePersonal.time_Start)),
        this.functionUtility.getDateTimeFormat(new Date(this.leavePersonal.time_End)),
        this.employeeInfo.empid
      )
      .pipe(tap(res => {
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
      ).toPromise();
  }

  async getCountRestAgent() {
    await this.leaveCommonService.getCountRestAgent(this.employeeInfo.empid, new Date(this.leavePersonal.time_Start).getFullYear())
      .pipe(tap(res => {
        if (res != null)
          this.leaveRestAgent = res;
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
      ).toPromise();
  }

  deleteLeave() {
    const listDelete = this.leaveData.filter(item => item.leaveDataViewModel.checked);

    if (listDelete.length === 0)
      return this.snotifyService.error(this.translateService.instant('Leave.ErrorMessage.DeleteMultiLeave'), this.translateService.instant('System.Caption.Error'));

    this.snotifyService.confirm(this.translateService.instant('Leave.LeaveRepresentative.ConfirmDelete'), this.translateService.instant('System.Caption.Warning'), async () => {
      this.spinnerService.show();
      this.leaveRepresentativeService.deleteLeave(listDelete)
        .subscribe({
          next: (res) => {
            if (res) {
              this.checkboxAll = false;
              this.getDataLeave();
              if (this.employeeInfo.empname !== null)
                this.getEmployeeInfo();
            }
            else
              this.snotifyService.error(
                this.translateService.instant('System.Message.DeleteErrorMsg'),
                this.translateService.instant('System.Caption.Error')
              );
            this.spinnerService.hide();
          }, error: () => {
            this.snotifyService.error(
              this.translateService.instant('System.Message.UnknowError'),
              this.translateService.instant('System.Caption.Error')
            );
            this.spinnerService.hide();
          }
        });
    })
  }

  openModal(template: TemplateRef<any>, item: RepresentativeDataViewModel) {
    this.modalRef = this.modalService.show(template, { class: 'modal-xl' });
    this.employeeViewOnTime = item;
  }

  refreshForm() {
    this.leavePersonal = <LeavePersonal>{
      cateID: null,
      comment: '',
      leaveDay: null,
      time_End: null,
      time_Lunch: '0',
      time_Start: null,
      empNumber: null
    };
    this.leaveDay = this.leaveDayView = null;
    this.leaveHours = this.leaveHoursView = null;
    this.employeeInfo = <EmployeeInfo>{
      empname: null
    }
    this.isLoading = false;
    this.isRequiredComment = false;
  }

  checkAll() {
    this.leaveData.forEach(element => element.leaveDataViewModel.checked = this.checkboxAll);
  }

  checkElement() {
    this.checkboxAll = this.leaveData.filter(x => x.leaveDataViewModel.checked !== true).length === 0 ? true : false;
  }

  setOption() {
    this.options = this.functionUtility.getDateTimePickerOption();
    this.optionsTimeEnd = this.functionUtility.getDateTimePickerOption();

    this.options.locale = this.language;
    this.optionsTimeEnd.locale = this.language;
  }
}
