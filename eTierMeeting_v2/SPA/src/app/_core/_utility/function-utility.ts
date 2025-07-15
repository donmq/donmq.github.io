import { Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { AlertifyService } from '../_services/alertify.service';

@Injectable({
  providedIn: 'root',
})
export class FunctionUtility {
  /**
     *Hàm tiện ích
     Function Utility
     */

  constructor(private _spinnerService: NgxSpinnerService,
    private _alertifyService: AlertifyService,) { }

  /**
     *Trả ra ngày hiện tại, chỉ lấy năm tháng ngày: yyyy/MM/dd
     Return today, show year/month/day:  yyyy/MM/dd
     */
  getToDay() {
    const toDay =
      new Date().getFullYear().toString() +
      '/' +
      (new Date().getMonth() + 1).toString() +
      '/' +
      new Date().getDate().toString();
    return toDay;
  }

  /**
     *Trả ra ngày với tham số truyền vào là ngày muốn format, chỉ lấy năm tháng ngày: yyyy/MM/dd
     input Date => output: string yyyy/MM/dd
     */
  getDateFormat(day: Date) {
    if (day !== null) {

      const dateFormat =
        day.getFullYear().toString() +
        '/' +
        (day.getMonth() + 1).toString() +
        '/' +
        day.getDate().toString();
      return dateFormat;
    }
    else
      return "";

  }

  /**
   *Trả ra chuỗi gồm năm với tháng kiểu: yyyyMM
   * @param day datatypes Date
   * @returns string yyyyMM
   */
  getFullYearAndMonthToString(day: Date) {
    const result =
      day.getFullYear().toString() +
      (day.getMonth() + 1 < 10
        ? '0' + (day.getMonth() + 1)
        : day.getMonth() + 1
      ).toString();
    return result;
  }
  /**
   *Trả ra chuỗi tên tháng
   * @param day datatypes string
   * @returns string month
   */
  getMonthInCharacter(month: string) {
    const monthNames = [
      'Jan.',
      'Feb.',
      'Mar.',
      'Apr.',
      'May',
      'Jun.',
      'Jul.',
      'Aug.',
      'Sep.',
      'Oct.',
      'Nov.',
      'Dec.',
    ];

    const d = Number(month);
    return monthNames[d - 1];
  }

  checkCreatFolder(folderName: string) {
    const charCheck = /[\\/,:*?"|<>]+/;
    if (charCheck.test(folderName)) {
      return true;
    }
  }

  getFirstDateOfCurrentMonth() {
    const today = new Date();
    return new Date(today.getFullYear() + '/' + (today.getMonth() + 1) + '/' + '01');
  }
  /**
     * Nhận vào DateTime
     * Trả ra ngày không lấy giờ
     */
  returnDayNotTime(date: Date) {
    const tmp = Date.UTC(date.getFullYear(), date.getMonth(), date.getDate(), 0, 0, 0);
    return new Date(tmp);
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
      result = new Blob(byteArrays, { type: `application/xlsx` });
    }
    if (!type) type = 'xlsx';
    if (result.size == 0) {
      this._spinnerService.hide();
      return this._alertifyService.warning('No Data');
    }
    if (result.type !== `application/${type}`) {
      this._spinnerService.hide();
      return this._alertifyService.error(result.type.toString());
    }
    const blob = new Blob([result]);
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', `${fileName}.${type}`);
    document.body.appendChild(link);
    link.click();
  }

  /**
  * Convert base64 string to Blob
  * @param base64 Base64 string
  * @param mimeType MIME type (e.g., 'image/jpeg')
  */
  base64ToBlob(base64: string, mimeType: string): Blob {
    const byteString = atob(base64.split(',')[1]);
    const ab = new ArrayBuffer(byteString.length);
    const ia = new Uint8Array(ab);
    for (let i = 0; i < byteString.length; i++) {
      ia[i] = byteString.charCodeAt(i);
    }
    return new Blob([ab], { type: mimeType });
  }

}
