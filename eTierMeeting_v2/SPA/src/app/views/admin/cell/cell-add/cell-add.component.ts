import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Select2OptionData } from 'ng-select2';
import { OperationResult } from '../../../../_core/_models/operation-result';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { BuildingService } from '../../../../_core/_services/building.service';
import { CellService } from '../../../../_core/_services/cell.service';
import { PdcService } from '../../../../_core/_services/pdc.service';
import { SweetAlertService } from '../../../../_core/_services/sweet-alert.service';

@Component({
  selector: 'app-cell-add',
  templateUrl: './cell-add.component.html',
  styleUrls: ['./cell-add.component.scss']
})
export class CellAddComponent implements OnInit {
  @Output() keyword = new EventEmitter<string>();
  cell: any = {
    cellID: 0,
    cellCode: null,
    cellName: null,
    pdcid: 0,
    pdcName: null,
    buildingID: 0,
    buildingName: null,
    updateBy: null,
    visible: true,
    updateTime: null,
  };

  checkCell = true;

  pdcs: Array<Select2OptionData>;
  buildings: Array<Select2OptionData>;

  lang: string;
  constructor(
    private cellService: CellService,
    private buildingService: BuildingService,
    private pdcService: PdcService,
    private sweetAlertifyService: SweetAlertService,
    private translate: TranslateService,
    private alertifyService: AlertifyService,
  ) { }
  ngOnInit() {
    this.getAllPdc();
    this.getAllBuilding();
  }

  addCell() {
    this.lang = localStorage.getItem('lang');
    if (this.cell.cellCode == null || this.cell.cellCode.trim() === '') {
      return this.sweetAlertifyService.warning('Error', this.translate.instant('alert.admin.please_enter_cell_name'));
    }
    if (this.cell.cellName == null || this.cell.cellName.trim() === '') {
      return this.sweetAlertifyService.warning('Error', this.translate.instant('alert.admin.please_enter_cell_code'));
    }
    if (this.cell.pdcid == 0) {
      return this.sweetAlertifyService.warning('Error', this.translate.instant('alert.admin.please_choose_pdc'));
    }
    if (this.cell.buildingID == 0) {
      return this.sweetAlertifyService.warning('Error', this.translate.instant('alert.admin.please_choose_building'));
    }

    this.cell.cellCode = this.cell.cellCode.trim();
    this.cell.cellName = this.cell.cellName.trim();
    this.cell.pdcid = +this.cell.pdcid;
    this.cell.buildingID = +this.cell.buildingID;

    this.cellService.addCell(this.cell, this.lang).subscribe((res: OperationResult) => {
      if (res.success) {
        this.sweetAlertifyService.success('Success', res.message);
        this.keyword.emit(this.cell.cellName);
        this.cancel();
      } else {
        this.sweetAlertifyService.error('Error', res.message);
      }
    }, error => {
      this.alertifyService.error(this.translate.instant('error.system_error'));
    });
  }

  cancel() {
    this.cell.cellCode = '';
    this.cell.cellName = '';
    this.cell.pdcid = 0;
    this.cell.buildingID = 0;
    this.checkCell = true;
  }

  getAllBuilding() {
    this.buildingService.getAllBuilding().subscribe(res => {
      this.buildings = res.map(item => {
        return { id: item.buildingID.toString(), text: item.buildingName };
      });
      this.buildings.unshift({ id: '0', text: this.translate.instant('admin.admincell.all_building') });
    });
  }

  getAllPdc() {
    this.pdcService.getAllPdc().subscribe(res => {
      this.pdcs = res.map(item => {
        return { id: item.pdcid.toString(), text: item.pdcName };
      });
      this.pdcs.unshift({ id: '0', text: this.translate.instant('admin.admincell.all_set') });
    });
  }

  getDataCell() {
    if (this.cell.cellCode != null) {
      this.cellService.getDataCell(this.cell.cellCode).subscribe((res) => {
        if (res) {
          this.cell.cellCode = res.cellCode;
          this.cell.cellName = res.cellName;
          this.cell.pdcid = res.pdcid;
          this.checkCell = false;
        }
      });
    }
  }


  checkEmpty() {
    if (this.cell.cellCode == null || this.cell.cellCode === '') {
      this.cell.cellName = '';
      this.cell.pdcid = 0;
      this.cell.buildingID = 0;
    }
    this.checkCell = true;
  }

}
