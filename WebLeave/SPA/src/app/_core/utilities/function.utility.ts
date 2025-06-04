import { Injectable } from "@angular/core";
import { HttpParams } from "@angular/common/http";
import { Pagination } from "./pagination-utility";
import { firstValueFrom } from "rxjs";
import { NgSnotifyService } from "../services/ng-snotify.service";
import { NgxSpinnerService } from "ngx-spinner";
import { TranslateService } from "@ngx-translate/core";
import { DateTimePickerOption } from "@models/leave/personal/dateTimePickerOption";

@Injectable({
  providedIn: "root",
})
export class FunctionUtility {
  /**
   *Hàm tiện ích
   */

  constructor(
    private snotify: NgSnotifyService,
    private spinnerService: NgxSpinnerService,
    private translateService: TranslateService,

  ) { }

  /**
   *Trả ra ngày hiện tại, chỉ lấy năm tháng ngày: yyyy/MM/dd
   */
  getToDay() {
    const toDay =
      new Date().getFullYear().toString() +
      "/" +
      (new Date().getMonth() + 1).toString() +
      "/" +
      new Date().getDate().toString();
    return toDay;
  }

  /**
   *Trả ra ngày với tham số truyền vào là ngày muốn format, chỉ lấy năm tháng ngày: yyyy/MM/dd
   */
  getDateFormat(date: Date) {
    return (
      date.getFullYear() +
      "/" +
      (date.getMonth() + 1 < 10
        ? "0" + (date.getMonth() + 1)
        : date.getMonth() + 1) +
      "/" +
      (date.getDate() < 10 ? "0" + date.getDate() : date.getDate())
    );
  }

  /**
   *Trả ra ngày với tham số truyền vào là ngày muốn format string: yyyy/MM/dd HH:mm:ss
   */
  getDateTimeFormat(date: Date) {
    return (
      date.getFullYear() +
      "/" +
      (date.getMonth() + 1 < 10
        ? "0" + (date.getMonth() + 1)
        : date.getMonth() + 1) +
      "/" +
      (date.getDate() < 10 ? "0" + date.getDate() : date.getDate()) +
      " " +
      (date.getHours() < 10 ? "0" + date.getHours() : date.getHours()) +
      ":" +
      (date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes()) +
      ":" +
      (date.getSeconds() < 10 ? "0" + date.getSeconds() : date.getSeconds())
    );
  }

  getUTCDate(d?: Date) {
    let date = d ? d : new Date();
    return new Date(
      Date.UTC(
        date.getFullYear(),
        date.getMonth(),
        date.getDate(),
        date.getHours(),
        date.getMinutes(),
        date.getSeconds()
      )
    );
  }

  /**
   * Check 1 string có phải empty hoặc null hoặc undefined ko.
   */
  checkEmpty(str) {
    return !str || /^\s*$/.test(str);
  }

  /**
   * Kiểm tra số lượng phần ở trang hiện tại, nếu bằng 1 thì cho pageNumber lùi 1 trang
   * @param pagination
   */
  calculatePagination(pagination: Pagination) {
    // Kiểm tra trang hiện tại phải là trang cuối không và trang hiện tại không phải là trang 1
    if (
      pagination.pageNumber === pagination.totalPage &&
      pagination.pageNumber !== 1
    ) {
      // Lấy ra số lượng phần tử hiện tại của trang
      let currentItemQty =
        pagination.totalCount -
        (pagination.pageNumber - 1) * pagination.pageSize;

      // Nếu bằng 1 thì lùi 1 trang
      if (currentItemQty === 1) {
        pagination.pageNumber--;
      }
    }
  }

  /**
   * Thêm hoặc xóa class tác động vào id element trên DOM
   * * @param id
   * * @param className
   * * @param type => Value bằng true thì add class. Value bằng false thì xóa class
   */
  changeDomClassList(id: string, className: string, type: boolean) {
    type
      ? document.getElementById(id).classList.add(className)
      : document.getElementById(id).classList.remove(className);
  }

  /**
   * Append property FormData
   * If property type Date => Convert value to String
   * * @param formValue
   */
  ToFormData(formValue: any) {
    const formData = new FormData();
    for (const key of Object.keys(formValue)) {
      const value = formValue[key];
      formData.append(key, value);
    }
    return formData;
  }

  /**
   * Append property HttpParams
   * * @param formValue
   */
  ToParams(formValue: any) {
    let params = new HttpParams();
    for (const key of Object.keys(formValue)) {
      const value = formValue[key];
      params = params.append(key, value);
    }
    return params;
  }

  exportExcel(result: Blob | string, fileName: string, type?: string) {
    if (typeof result === "string") {
      let byteCharacters = atob(result);
      let byteArrays = [];
      for (let offset = 0; offset < byteCharacters.length; offset += 512) {
        let slice = byteCharacters.slice(offset, offset + 512);
        let byteNumbers = new Array(slice.length);
        for (var i = 0; i < slice.length; i++) {
          byteNumbers[i] = slice.charCodeAt(i);
        }
        let byteArray = new Uint8Array(byteNumbers);
        byteArrays.push(byteArray);
      }
      if (!type) type = 'xlsx';
      result = new Blob(byteArrays, { type: `application/${type}` });
    }

    if (result.size == 0) {
      this.spinnerService.hide();
      return this.snotify.warning('No Data', 'Warning');
    }
    if (result.type !== `application/${type}`) {
      this.spinnerService.hide();
      return this.snotify.error(result.type.toString(), 'Error');
    }
    const blob = new Blob([result]);
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', `${fileName}.${type}`);
    document.body.appendChild(link);
    link.click();
  }

  exportCSV(result: string, fileName: string) {
    let byteCharacters = atob(result);
    let byteArrays = [];
    for (let offset = 0; offset < byteCharacters.length; offset += 512) {
      let slice = byteCharacters.slice(offset, offset + 512);
      let byteNumbers = new Array(slice.length);
      for (var i = 0; i < slice.length; i++) {
        byteNumbers[i] = slice.charCodeAt(i);
      }
      let byteArray = new Uint8Array(byteNumbers);
      byteArrays.push(byteArray);
    }

    let blob = new Blob(byteArrays, { type: 'text/csv' });
    if (blob.size == 0) {
      this.spinnerService.hide();
      return this.snotify.warning('No Data', 'Warning');
    }

    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', `${fileName}.csv`);
    document.body.appendChild(link);
    link.click();
  }

  async changeCommentLanguage(comment: string) {
    let _comment = comment;
    _comment = _comment.replace(/themtaitrangcanhan/g, await firstValueFrom(this.translateService.get('Leave.LeaveDetail.textcmt1')));
    _comment = _comment.replace(/themtutrangdaidien/g, await firstValueFrom(this.translateService.get('Leave.LeaveDetail.textcmt2')));
    _comment = _comment.replace(/duocduyetboi/g, await firstValueFrom(this.translateService.get('Leave.LeaveDetail.textcmt3')));
    _comment = _comment.replace(/tuchoiboi/g, await firstValueFrom(this.translateService.get('Leave.LeaveDetail.textcmt4')));
    _comment = _comment.replace(/themnangcao/g, await firstValueFrom(this.translateService.get('Leave.LeaveDetail.textcmt5')));
    _comment = _comment.replace(/duocluutru/g, await firstValueFrom(this.translateService.get('Leave.LeaveDetail.textcmt6')));
    _comment = _comment.replace(/suadoichoduyet/g, await firstValueFrom(this.translateService.get('Leave.LeaveDetail.textcmt7')));
    _comment = _comment.replace(/suadoichapnhan/g, await firstValueFrom(this.translateService.get('Leave.LeaveDetail.textcmt8')));
    _comment = _comment.replace(/suadoituchoi/g, await firstValueFrom(this.translateService.get('Leave.LeaveDetail.textcmt9')));
    _comment = _comment.replace(/dasuachua/g, await firstValueFrom(this.translateService.get('Leave.LeaveDetail.textcmt15')));
    _comment = _comment.replace(/ycsuachua/g, await firstValueFrom(this.translateService.get('Leave.LeaveDetail.ycsuachua')));
    _comment = _comment.replace(/lydo/g, await firstValueFrom(this.translateService.get('Leave.LeaveDetail.Reason')));
    _comment = _comment.replace(/buphep/g, await firstValueFrom(this.translateService.get('Leave.LeaveDetail.buphep')));
    _comment = _comment.replace(/themphepthucong/g, await firstValueFrom(this.translateService.get('Leave.LeaveDetail.themphepthucong')));
    _comment = _comment.replace(/datuchoi,vuilonglienhenhansu/g, await firstValueFrom(this.translateService.get('Leave.LeaveDetail.HrRejected')));
    _comment = _comment.replace(/Đã xóa/g, await firstValueFrom(this.translateService.get('Leave.LeaveDetail.Deleted')));
    return _comment;
  }

  getDateTimePickerOption(language: string = '') {
    let options: DateTimePickerOption = <DateTimePickerOption>{
      format: 'DD/MM/YYYY HH:mm',
      stepping: 5,
      locale: language,
      sideBySide: true,
      allowInputToggle: true,
      daysOfWeekDisabled: [], // Set ngày ẩn ngày chủ nhật
      useCurrent: false,
      calendarWeeks: true,
      icons: {
        time: 'fa fa-clock-o',
        date: 'fa fa-calendar',
        up: 'fa fa-chevron-up',
        down: 'fa fa-chevron-down',
        previous: 'fa fa-chevron-left',
        next: 'fa fa-chevron-right',
        today: 'fa fa-screenshot',
        clear: 'fa fa-trash',
        close: 'fa fa-remove',
      },
      showClear: true,
      keyBinds: {}
    }

    return options;
  }
}
