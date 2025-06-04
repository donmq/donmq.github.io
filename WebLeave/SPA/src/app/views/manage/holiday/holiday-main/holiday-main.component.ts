import { Component, OnInit, TemplateRef } from '@angular/core';
import { CommonConstants } from '@constants/common.constants';
import { LangConstants } from '@constants/lang.constants';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { HolidayAndUserDto } from '@models/manage/holiday/holiday-and-user';
import { DestroyService } from '@services/destroy.service';
import { HolidayService } from '@services/manage/holiday.service';
import { InjectBase } from '@utilities/inject-base-app';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { takeUntil } from 'rxjs';

@Component({
  selector: 'app-holiday-main',
  templateUrl: './holiday-main.component.html',
  styleUrls: ['./holiday-main.component.scss'],
  providers: [DestroyService]
})
export class HolidayMainComponent extends InjectBase implements OnInit {
  listHoliday: HolidayAndUserDto[] = [];
  addHoliday: HolidayAndUserDto = <HolidayAndUserDto>{};
  _addHoliday: HolidayAndUserDto;

  lang: string = localStorage.getItem(LocalStorageConstants.LANG) ?? LangConstants.VN;

  modalRef?: BsModalRef;

  bsConfig = Object.assign(
    {
      containerClass: "theme-dark-blue",
      isAnimated: true,
      dateInputFormat: "DD/MM/YYYY",
    }
  );

  descriptionArea: string = "";
  holidayTime: Date | any = null;

  commonConstants: typeof CommonConstants = CommonConstants;
  langConstants: typeof LangConstants = LangConstants;

  constructor(
    private holidayService: HolidayService,
    private modalService: BsModalService,
  ) {
    super();
  }

  ngOnInit(): void {
    this.getData();
    this.lang = localStorage.getItem(LocalStorageConstants.LANG);
    this.translateService.onLangChange.pipe(takeUntil(this.destroyService.destroys$)).subscribe((event) => {
      this.lang = event.lang;
    })
  }

  getData() {
    this.spinnerService.show();
    this.holidayService.getHolidayData().subscribe({
      next: (res) => {
        this.listHoliday = res;
        this.spinnerService.hide();
      },
      error: () => {
        this.spinnerService.hide();
        this.snotifyService.error(this.translateService.instant('System.Message.Nodata'), this.translateService.instant('System.Caption.Error'))
      }
    });
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  clearAdd() {
    this.addHoliday = { ...this._addHoliday };
  }

  addData() {
    this.spinnerService.show();
    this.addHoliday.date = this.addHoliday.date.toStringDate();

    this.holidayService.addHoliday(this.addHoliday).subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.spinnerService.hide();
          this.snotifyService.success(this.translateService.instant('System.Message.UpdateOKMsg'), this.translateService.instant('System.Caption.Success'));
          this.modalRef?.hide();
          this.clearAdd();
          this.getData()
        }
        else {
          this.spinnerService.hide();
          this.clearAdd();
          this.snotifyService.error(this.translateService.instant('System.Message.CreateErrorMsg'), this.translateService.instant('System.Caption.Error'));
        }
      },
      error: () => {
        this.spinnerService.hide();
        this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
      }
    })
  }

  remove(IDHoliday: number) {
    this.snotifyService.confirm(this.translateService.instant('System.Message.ConfirmDelete'), this.translateService.instant('System.Action.Delete'), () => {
      this.spinnerService.show();
      this.holidayService.removeHoliday(IDHoliday).subscribe({
        next: (res) => {
          if (res) {
            this.spinnerService.hide();
            this.snotifyService.success(this.translateService.instant('System.Message.DeleteOKMsg'), this.translateService.instant('System.Caption.Success'));
            this.getData();
          }
        },
        error: () => {
          this.spinnerService.hide();
          this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
        }
      });
    });
  }

  resetAdd() {
    this.addHoliday = { ...this._addHoliday };
  }
}
