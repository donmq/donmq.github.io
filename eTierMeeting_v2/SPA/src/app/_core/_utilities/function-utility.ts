import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../../environments/environment";
import { NgSnotifyService } from "../_services/ng-snotify.service";
import { NgxSpinnerService } from "ngx-spinner";

@Injectable({
    providedIn: "root",
})

export class FunctionUtility {
  constructor(
    private http: HttpClient,
    private snotify: NgSnotifyService,
    private spinnerService: NgxSpinnerService
  ) { }
    /**
   *Trả ra ngày với tham số truyền vào là ngày muốn format, chỉ lấy năm tháng ngày: yyyy/MM/dd
   */
  getDateFormat(date: Date) {
    return (date.getFullYear()) + '/' +
      (date.getMonth() + 1 < 10 ? ('0' + (date.getMonth() + 1)) : (date.getMonth() + 1)) + '/' +
      (date.getDate() < 10 ? ('0' + date.getDate()) : date.getDate());
  }

  getUTCDate(d?: Date) {
    let date = d ? d : new Date();
    return new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate(), date.getHours(), date.getMinutes(), date.getSeconds()));
  }

   /**
   * Check 1 string có phải empty hoặc null hoặc undefined ko.
   */
    checkEmpty(str: string) {
        return !str || /^\s*$/.test(str);
      }


    
  /**
   * Convert today to string format
   * @returns yyyy/MM/dd
   */
  getToDay() {
    const date = new Date();
    return `${date.getFullYear()}/${date.getMonth() + 1}/${date.getDate()}`;
  }

  /**
   * Convert input date to short date string format
   * @param date
   * @returns yyyyMM
   */
  getFullYearAndMonthToString(date: Date) {
    return `${date.getFullYear()}${date.getMonth() + 1 < 10 ? '0' + (date.getMonth() + 1) : (date.getMonth() + 1)}`;
  }

  /**
   * Convert to short month string format
   * @param month
   * @returns MMM.
   */
  getMonthInCharacter(month: string) {
    const months = ['Jan.', 'Feb.', 'Mar.', 'Apr.', 'May', 'Jun.', 'Jul.', 'Aug.', 'Sep.', 'Oct.', 'Nov.', 'Dec.'];
    return months[Number(month) - 1];
  }

  /**
   * Check if folder name contains special characters
   * @param folderName
   * @returns true | false
   */
  checkCreatFolder(folderName: string) {
    const charCheck = /[\\/,:*?"|<>]+/;
    if (charCheck.test(folderName)) {
      return true;
    }
  }

  /**
   * Return the first date of current month
   * @returns Date
   */
  getFirstDateOfCurrentMonth() {
    const today = new Date();
    return new Date(today.getFullYear() + '/' + (today.getMonth() + 1) + '/' + '01');
  }

  /**
   * Convert input date to date without time
   * @param date
   * @returns Date
   */
  returnDayNotTime(date: Date) {
    return new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate(), 0, 0, 0));
  }

  // Converting "String" to javascript "File Object"
  convertToFile(listFile: string[], urlFolder: string, file: File[]) {
    // ***Here is the code for converting "String" to "Base64".***
    listFile.forEach(element => {
      if (element !== '') {
        let url = urlFolder + element;
        const toDataURL = url => fetch(url)
          .then(response => response.blob())
          .then(blob => new Promise((resolve, reject) => {
            const reader = new FileReader();
            reader.onloadend = () => resolve(reader.result);
            reader.onerror = reject;
            reader.readAsDataURL(blob);
          }));

        // *** Calling both function ***
        toDataURL(url).then(dataUrl => {
          let fileData = dataURLtoFile(dataUrl, element);
          file.push(fileData);
        });
        // ***Here is code for converting "Base64" to javascript "File Object".***
        function dataURLtoFile(dataurl, filename) {
          let arr = dataurl.split(','), mime = arr[0].match(/:(.*?);/)[1],
            bstr = atob(arr[1]),
            n = bstr.length,
            u8arr = new Uint8Array(n);
          while (n--) {
            u8arr[n] = bstr.charCodeAt(n);
          }
          return new File([u8arr], filename, { type: mime });
        }
      }
    });
  }
  // End Converting "String" to javascript "File Object"

  exportExcel(result: Blob, fileName: string) {
    if (result.size == 0) {
      this.spinnerService.hide();
      return this.snotify.warning('No Data', "Warning")
    }
    if (result.type !== 'application/xlsx') {
      this.spinnerService.hide();
      return this.snotify.error(result.type.toString(), "Error");
    }
    const blob = new Blob([result]);
    const url = window.URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.setAttribute('download', `${fileName}.xlsx`);
    document.body.appendChild(link);
    link.click();
  }
  
}