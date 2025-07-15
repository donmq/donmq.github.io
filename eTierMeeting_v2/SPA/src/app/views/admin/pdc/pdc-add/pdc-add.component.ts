import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { OperationResult } from '../../../../_core/_models/operation-result';
import { Pdc } from '../../../../_core/_models/pdc';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { PdcService } from '../../../../_core/_services/pdc.service';
import { SweetAlertService } from '../../../../_core/_services/sweet-alert.service';

@Component({
  selector: 'app-pdc-add',
  templateUrl: './pdc-add.component.html',
  styleUrls: ['./pdc-add.component.scss']
})
export class PdcAddComponent implements OnInit {
  @Output() keyword = new EventEmitter<string>();
  pdc: Pdc = {
    pdcid: 0,
    pdcName: null,
    pdcCode: null,
    visible: true
  };

  lang: string;
  constructor(
    private pdcService: PdcService,
    private sweetAlertService: SweetAlertService,
    private translate: TranslateService,
    private alertifyService: AlertifyService,
  ) { }

  ngOnInit() {
  }

  addPDC() {
    this.lang = localStorage.getItem('lang');
    if (this.pdc.pdcName === null || this.pdc.pdcName.trim() === '') {
      this.sweetAlertService.warning('Error', this.translate.instant('alert.admin.please_enter_pdc_name'));
      return;
    }
    if (this.pdc.pdcCode === null || this.pdc.pdcCode.trim() === '') {
      this.sweetAlertService.warning('Error', this.translate.instant('alert.admin.please_enter_pdc_code'));
      return;
    }
    this.pdc.pdcName = this.pdc.pdcName.trim();
    this.pdc.pdcCode = this.pdc.pdcCode.trim();
    this.pdcService.addPDC(this.pdc, this.lang).subscribe((res: OperationResult) => {
      if (res.success) {
        this.sweetAlertService.success('Success', res.message);
        this.keyword.emit(this.pdc.pdcCode);
        this.cancel();
      } else {
        this.sweetAlertService.error('Error', res.message);
      }
    }, error => {
      this.alertifyService.error(this.translate.instant('error.system_error'));
    });
  }

  cancel() {
    this.pdc.pdcName = '';
    this.pdc.pdcCode = '';
  }
}
