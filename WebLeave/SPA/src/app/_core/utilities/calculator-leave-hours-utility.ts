import { Injectable } from '@angular/core';
import { environment } from '@env/environment';
import { WorkShift } from '@models/common/lunch-break';
import { dateOBJ } from '@models/leave/dateObj';
import { Holiday } from '@models/leave/personal/holiday';
import { LeaveCommonService } from '@services/common/leave-common.service';
import { firstValueFrom } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class CalculatorLeaveHoursUtility {
  apiUrl = environment.apiUrl;
  workShift: WorkShift = <WorkShift>{};
  workShiftList: WorkShift[] = [];
  constructor(private readonly leaveCommonService: LeaveCommonService) { this.shiftWorkList(); }

  rangerDate(shift: string, timeLeaveStart: Date, timeLeaveEnd: Date, holidays: Holiday[], IsSun: boolean): dateOBJ[] {
    var dateRanger: dateOBJ[] = []; //mảng đối tượng chứa thời gian bắt đầu và thời gian kết thúc của từng ngày
    //Nếu là ca C thì giảm giờ bắt đầu xin phép và giờ kết thúc xin phép đi nửa ngày để giờ trong 1 ngày tính cho dễ, còn ca khác thì giữ nguyên không giảm đi nửa ngày
    if (shift == 'C') {
      timeLeaveStart = this.decreaseDayHalf(timeLeaveStart); //giảm giờ bắt đầu xin phép đi nửa ngày có sử dụng hàm tiện ích decreaseDayHalf() đơn vị là (datetime)
      timeLeaveEnd = this.decreaseDayHalf(timeLeaveEnd);     //giảm giờ kết thúc xin phép đi nửa ngày có sử dụng hàm tiện ích decreaseDayHalf() đơn vị là (datetime)
    }
    //set lại giờ bắt đầu xin phép (currentDate) với giờ kết thúc xin phép (dateTimeLeave) là vẫn giữ nguyên ngày mà chỉ set giờ phút giây về  0 0 0 hết để so sánh
    let currentDate = new Date(timeLeaveStart.getFullYear(), timeLeaveStart.getMonth(), timeLeaveStart.getDate(), 0, 0, 0); //đơn vị là(datetime)
    let dateTimeLeave = new Date(timeLeaveEnd.getFullYear(), timeLeaveEnd.getMonth(), timeLeaveEnd.getDate(), 0, 0, 0);     //đơn vị là(datetime)
    //khi mà ngày bắt đầu xin phép (currentDate) vẫn còn nhỏ hơn hoặc bằng ngày kết thúc xin phép (dateTimeLeave)
    this.workShift = this.workShiftList.find(x => x.key == shift);
    while (currentDate <= dateTimeLeave) {
      let checkHoliday = holidays.some(x => x.day == currentDate.getDate() && x.month == (currentDate.getMonth() + 1) && x.year == currentDate.getFullYear())
      //nếu ngày hiện tại có khác chủ nhật và ngày lễ hay không(hàm checkHoliday là kiểm tra có phải là ngày lễ hay không)
      if ((IsSun || currentDate.getDay() != 0) && !checkHoliday) {
        let dateObj: dateOBJ = {
          timeStartDay: currentDate.getDate(),                                 //Ngày bắt đầu bằng ngày hiện tại
          timeStartMonth: currentDate.getMonth(),                              //Tháng bắt đầu bằng tháng hiện tại
          timeStartYear: currentDate.getFullYear(),                            //Năm bắt đầu bằng năm hiện tại
          timeStartHour: this.workShift?.workTimeStart?.hours,                 //Giờ bắt đầu bằng giờ bắt đầu làm của ca làm việc
          timeStartMinute: this.workShift?.workTimeStart?.minutes,             //Phút bắt đầu bằng phút kết thúc của ca làm việc

          timeEndDay: currentDate.getDate(),                                   //Ngày kết thúc bằng ngày hiện tại
          timeEndMonth: currentDate.getMonth(),                                //Tháng kết thúc bằng tháng hiện tại
          timeEndYear: currentDate.getFullYear(),                              //Năm kết thúc bằng năm hiện tại
          timeEndHour: this.workShift?.workTimeEnd?.hours,                     //Giờ kết thúc bằng giờ kết thúc làm của ca làm việc
          timeEndMinute: this.workShift?.workTimeEnd?.minutes,                 //Phút kết thúc bằng phút kết thúc của ca làm việc

          shift: shift,                                                        //Ca làm việc
          timeLunch: 0
        }
        dateRanger.push(dateObj); //thêm 1 đối tượng dateObj vào mảng dateRanger vào cuối mảng
      }
      currentDate = this.addDay(currentDate, 1); //ngày bắt đầu xin phép tăng lên 1
    }

    //Duyệt từng thằng trong mảng giờ xin phép dateRanger để sửa lại theo cho đúng
    dateRanger.forEach((value, index) => {
      //Modify lại thằng đầu tiên trong mảng nếu mảng có 1 phần tử
      if (dateRanger.length == 1) {
        value.timeStartHour = timeLeaveStart.getHours();      //giờ giờ bắt đầu bằng giờ bắt đầu xin phép (giờ)
        value.timeStartMinute = timeLeaveStart.getMinutes();  //phút bắt đầu bằng  phút bắt đầu xin phép (phút)

        value.timeEndHour = timeLeaveEnd.getHours();     //giờ kết thúc bằng giờ kết thúc xin phép (giờ)
        value.timeEndMinute = timeLeaveEnd.getMinutes(); //phút kết thúc bằng phút kết thúc xin phép (phút)

        value.timeLunch = this.totalLunchByDay(value.shift, value.timeStartDay, value.timeEndMonth, value.timeEndYear, value.timeStartHour, value.timeStartMinute, value.timeEndHour, value.timeEndMinute) //Lấy ra thời gian nghỉ trưa theo từng ngày (giờ) hàm totalLunchByDay tính thời gian ăn trưa theo từng ngày

        dateRanger[0] = value;  //gán thằng đầu tiên bằng value mình modify ở trên : có thay đổi thời gian đầu và thời gian kết thúc
      }
      //Modify lại thằng đầu tiên trong mảng và thằng cuối cùng trong mảng nếu mảng có nhiều hơn 1 phần tử
      if (dateRanger.length > 1) {
        //Modify lại thằng đầu tiên trong mảng
        if (index == 0) {
          value.timeStartHour = timeLeaveStart.getHours();       //giờ giờ bắt đầu bằng giờ bắt đầu xin phép (giờ)
          value.timeStartMinute = timeLeaveStart.getMinutes();   //phút bắt đầu bằng  phút bắt đầu xin phép (phút)

          value.timeLunch = this.totalLunchByDay(value.shift, value.timeStartDay, value.timeEndMonth, value.timeEndYear, value.timeStartHour, value.timeStartMinute, value.timeEndHour, value.timeEndMinute)  //Lấy ra thời gian nghỉ trưa theo từng ngày (giờ) hàm totalLunchByDay tính thời gian ăn trưa theo từng ngày, nếu không hiểu thì tìm xuốn dưới đọc comment hàm
          dateRanger[0] = value; //gán thằng đầu tiên bằng value mình modify ở trên: chỉ thay đổi thời gian đầu còn thời gian kết thúc không thay đổi
        }
        //Modify lại thằng cuối trong mảng
        else if (dateRanger.length == index + 1) {
          value.timeEndHour = timeLeaveEnd.getHours();     //giờ kết thúc bằng giờ kết thúc xin phép (giờ)
          value.timeEndMinute = timeLeaveEnd.getMinutes(); //phút kết thúc bằng phút kết thúc xin phép (phút)

          value.timeLunch = this.totalLunchByDay(value.shift, value.timeStartDay, value.timeEndMonth, value.timeEndYear, value.timeStartHour, value.timeStartMinute, value.timeEndHour, value.timeEndMinute)  //Lấy ra thời gian nghỉ trưa theo từng ngày(giờ) hàm totalLunchByDay tính thời gian ăn trưa theo từng ngày, nếu không hiểu thì tìm xuốn dưới đọc comment hàm
          dateRanger[index] = value;  //gán thằng cuối cùng bằng value mình modify ở trên: không thay đổi thời gian đầu còn thời gian kết thúc có thay đổi
        }
        //Ngược lại mấy thằng ở giữa là không Modiffy
        else {
          value.timeLunch = this.totalLunchByDay(value.shift, value.timeStartDay, value.timeEndMonth, value.timeEndYear, value.timeStartHour, value.timeStartMinute, value.timeEndHour, value.timeEndMinute);  //Lấy ra thời gian nghỉ trưa theo từng ngày  (giờ) hàm totalLunchByDay tính thời gian ăn trưa theo từng ngày, nếu không hiểu thì tìm xuốn dưới đọc comment hàm
          dateRanger[index] = value;  //gán thằng cuối cùng bằng value mình modify ở trên: trường hợp này không thay đổi gì hết
        }
      }
    });
    return dateRanger;
  }

  shiftWorkList() {
    this.leaveCommonService.getWorkShifts()
    .subscribe({
      next: (res) => {
        this.workShiftList = res;
      }
    });
  }
  async shiftWork(shift: string) {
    this.workShift = await firstValueFrom(this.leaveCommonService.getWorkShift(shift));
  }

  sumTotalLeave(rangerDay: dateOBJ[], shift: string): number {
    let totalLeave = rangerDay.reduce((totalLeave, item) => {
      let timeStartDay = new Date(item.timeStartYear, item.timeStartMonth, item.timeStartDay, item.timeStartHour, item.timeStartMinute);//lấy ra thời gian bắt đầu trong từng ngày
      let timeEndDay = new Date(item.timeEndYear, item.timeEndMonth, item.timeEndDay, item.timeEndHour, item.timeEndMinute);//lấy ra thời gian kết thúc trong từng ngày

      let total = (timeEndDay.getTime() - timeStartDay.getTime()) / 1000 / 60 / 60;//tổng thời gian theo từng ngày là bằng thời gian kết thúc trừ thời gian đầu tiên

      //nếu là ca 'A' hoặc ca 'B' hoặc ca 'C' hoặc 'C5' thì cộng hết lại không trừ thời gian nghỉ trưa theo nhân sự
      if (shift == 'A' || shift == 'B' || shift == 'C' || shift == 'C5') {
        return totalLeave + total;
      }
      //nếu là các ca khác thì trừ thời gian ăn trưa ra theo nhân sự
      else
        return totalLeave + total - item.timeLunch;
    }, 0)

    return totalLeave;//trả ra tổng số giờ nghỉ đơn gị là (giờ)

  }

  totalLunchByDay(shift: string, day: number, month: number, year: number, hoursStart: number, minuteStart: number, hoursEnd: number, minuteEnd: number): number {
    let dayStart: Date = new Date(year, month, day, hoursStart, minuteStart);     //khai báo thời gian bắt đầu nghỉ trong từng ngày (rangerDate)
    let dayEnd: Date = new Date(year, month, day, hoursEnd, minuteEnd);           //khai báo thời gian kết thúc nghỉ trong từng ngày (rangerDate)

    this.workShift = this.workShiftList.find(x => x.key == shift);
    let dayShiftStart: Date = new Date(year, month, day, this.workShift?.lunchTimeStart?.hours, this.workShift?.lunchTimeStart?.minutes); //thời gian bắt đầu nghỉ trưa theo ca
    let dayShiftEnd: Date = new Date(year, month, day, this.workShift?.lunchTimeEnd?.hours, this.workShift?.lunchTimeEnd?.minutes);       //thời gian kết thúc nghỉ trưa theo ca

    let lunchTime: number = 0;//thời gian nghỉ trưa theo từng ngày (đơn vị là giờ)

    //nếu thời gian bắt đầu nghỉ trong từng ngày nhỏ hơn thời gian bắt đầu nghỉ trưa theo ca (dayStart <= dayShiftStart) và thời gian kết thúc nghỉ trong từng ngày nhỏ hơn thời gian kết thúc nghỉ trưa theo ca (dayEnd <= dayShiftStart)
    //có nghĩa là thời gian nghỉ nằm trước khoảng thời gian nghỉ trưa theo ca ==> thời gian nghỉ trưa của ngày đó là bằng 0(lunchTime = 0)
    if (dayStart <= dayShiftStart && dayEnd <= dayShiftStart) {
      lunchTime = 0;  //đơn vị (giờ)
    }

    //nếu thời gian bắt đầu nghỉ trong từng ngày lớn hơn hoặc bằng thời gian kết thúc nghỉ trưa theo ca (dayStart >= dayShiftEnd) và thời gian kết thúc nghỉ trong từng này lớn hơn hoặc bằng thời gian kết thúc nghỉ trưa theo ca (dayEnd >= dayShiftEnd)
    //có nghĩa là thời gian nghỉ nằm sau khoảng thời gian nghỉ trưa theo ca ==> thời gian nghỉ trưa của ngày đó là bằng 0(lunchTime = 0)
    else if (dayStart >= dayShiftEnd && dayEnd >= dayShiftEnd) {
      lunchTime = 0;  //đơn vị (giờ)
    }

    //nếu thời gian bắt đầu nghỉ trong từng ngày lớn hơn hoặc bằng thời gian bắt đầu nghỉ trưa (dayStart >= dayShiftStart) và thời gian kết thúc nghỉ trong từng ngày nhỏ hơn hoặc bằng thời gian kết thúc nghỉ trưa theo ca (dayEnd <= dayShiftEnd)
    //có nghĩa là thời gian nghỉ nằm trong khoảng thời gian nghỉ trưa theo ca ==> thời gian nghỉ trưa của ngày đó là bằng thời gian kết thúc xin phép trừ thời gian bắt đầu xin phép dayEnd - dayStart(xin phép trong thời gian ăn trưa suy ra là 0 ngày, chỉ xin phép trong 1 ngày và chọn trong thời gian ăn trưa mới có trường hợp này)
    else if (dayStart >= dayShiftStart && dayEnd <= dayShiftEnd) {
      lunchTime = (dayEnd.getTime() - dayStart.getTime()) / 1000 / 60 / 60;   //đơn vị (giờ)
    }

    //nếu thời gian bắt đầu nghỉ trong từng ngày nhỏ hơn hoặc bằng thời gian bắt đầu nghỉ trưa (dayStart <= dayShiftStart) và thời gian kết thúc nghỉ lớn hơn hoặc bằng thời gian kết thúc nghỉ trưa theo từng ca (dayEnd >= dayShiftEnd)
    //có nghĩa là thời gian nghỉ bao gồm cả thời gian nghỉ trưa ==> lunchTime bằng tổng thời gian nghỉ trưa theo từng ngày
    else if (dayStart <= dayShiftStart && dayEnd >= dayShiftEnd) {
      lunchTime = (dayShiftEnd.getTime() - dayShiftStart.getTime()) / 1000 / 60 / 60;  //đơn vị (giờ)
    }

    //nếu thời gian bắt đầu nghỉ trong từng ngày nhỏ hơn hoặc bằng thời gian bắt đầu nghỉ trưa (dayStart <= dayShiftStart) và thời gian kết thúc nghỉ lớn hơn hoặc bằng thời gian bắt đầu nghỉ trưa theo từng ca (dayEnd >= dayShiftStart) và thời gian kết thúc nghỉ nhỏ hơn hoặc bằng thời gian kết thúc nghỉ trưa theo từng ca (dayEnd <= dayShiftEnd)
    //có nghĩa là thời gian bắt đầu nghỉ nhỏ hơn khoảng thời gian nghỉ trưa và thời gian kết thúc nghỉ nằm trong khoảng thời gian nghỉ trưa ==> lunchTime ở dưới
    else if (dayStart <= dayShiftStart && dayEnd >= dayShiftStart && dayEnd <= dayShiftEnd) {
      lunchTime = (dayEnd.getTime() - dayShiftStart.getTime()) / 1000 / 60 / 60;        //đơn vị (giờ)
    }

    //nếu thời gian bắt đầu nghỉ trong từng ngày lớn hơn hoặc bằng thời gian bắt đầu nghỉ trưa (dayStart >= dayShiftStart) và thời gian bắt đầu nghỉ nhỏ hơn hoặc bằng thời gian kết thúc nghỉ trưa theo từng ca (dayStart <= dayShiftEnd) và thời gian kết thúc nghỉ lớn hơn hoặc bằng thời gian kết thúc nghỉ trưa theo từng ca (dayEnd >= dayShiftEnd)
    //có nghĩa là thời gian bắt đầu nghỉ nằm trong khoảng thời gian nghỉ trưa và thời gian kết thúc nghỉ lớn hơn khoảng thời gian nghỉ trưa ==> lunchTime ở dưới
    else if (dayStart >= dayShiftStart && dayStart <= dayShiftEnd && dayEnd >= dayShiftEnd) {
      lunchTime = (dayShiftEnd.getTime() - dayStart.getTime()) / 1000 / 60 / 60;          //đơn vị (giờ)
    }
    else {
      lunchTime = 0;
    }
    return lunchTime;
  }

  convertHourToDay(hours: number): number {
    return hours / 8;
  }

  convertDateJson(date: string): Date {
    return new Date(parseInt(date.substring(6)));
  }

  addDay(date: Date, days: number): Date {
    return new Date(date.setDate(date.getDate() + days));
  }
  //decreaseDayHalf hàm tiện ích để giảm thời gian đi là nửa ngày(12h) dùng trong ca 'C'(shift == 'C') chỉ dùng trong ca 'C'
  //trả ra ngày đã được giảm đi là nửa ngày(12h)
  decreaseDayHalf(date: Date): Date {
    //khai báo 1 biến giờ được giảm đi là 12h
    let hour = date.getHours() - 12;
    //nếu mà giờ đã tăng lên mà nhỏ hơn hoặc bằng 0 (hour < 0) thì giảm ngày đi 1 (this.getDate() - 1) ngược lại ngày giữ nguyên (this.getDate())
    date = new Date(date.setDate((hour < 0) ? date.getDate() - 1 : date.getDate()));
    //nếu giờ đã tăng lên mà lớn hơn hoặc bằng 24 (hour >= 24) thì có nghĩa là qua ngày mới nên giờ bị trừ đi 24 (hour - 24) ngược lại thì chưa giảm qua tới ngày cũ nên giờ giữ nguyên (hour) còn phút với giây thì giữ nguyên
    date = new Date(date.setHours((hour < 0) ? hour + 24 : hour, date.getMinutes(), 0, 0));
    return date;
  }

  getMinutes(dateStart: string, dateEnd: string): number {
    let min = (new Date(dateEnd).getTime() - new Date(dateStart).getTime()) / 1000 / 60; // Đơn vị phút
    return min;
  }

  checkTime(step: number, start: string): Date {
    let date = start ? new Date(start) : new Date();
    let min: number = date.getMinutes();
    let minutes = 0;
    if (step == 5) {
      if (min < 5) minutes = 0;
      else if (min >= 5 && min < 10) minutes = 5;
      else if (min >= 10 && min < 15) minutes = 10;
      else if (min >= 15 && min < 20) minutes = 15;
      else if (min >= 20 && min < 25) minutes = 20;
      else if (min >= 25 && min < 30) minutes = 25;
      else if (min >= 30 && min < 35) minutes = 30;
      else if (min >= 35 && min < 40) minutes = 35;
      else if (min >= 40 && min < 45) minutes = 40;
      else if (min >= 45 && min < 50) minutes = 45;
      else if (min >= 50 && min < 55) minutes = 50;
      else if (min >= 55) minutes = 55;
    }
    else {
      if (min < 15) minutes = 0;
      else if (min >= 15 && min < 30) minutes = 15;
      else if (min >= 30 && min < 45) minutes = 30;
      else if (min >= 45) minutes = 45;
    }

    return new Date(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours(), minutes, date.getSeconds());
  }
}
