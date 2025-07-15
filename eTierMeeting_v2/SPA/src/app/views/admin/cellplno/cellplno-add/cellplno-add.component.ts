import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Select2OptionData } from 'ng-select2';
import { CellPlno } from '../../../../_core/_models/cell-plno';
import { OperationResult } from '../../../../_core/_models/operation-result';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { CellService } from '../../../../_core/_services/cell.service';
import { CellPlnoService } from '../../../../_core/_services/cellplno.service';
import { SweetAlertService } from '../../../../_core/_services/sweet-alert.service';

@Component({
  selector: 'app-cellplno-add',
  templateUrl: './cellplno-add.component.html',
  styleUrls: ['./cellplno-add.component.scss']
})
export class CellplnoAddComponent implements OnInit {
  @Output() keyword = new EventEmitter<string>();

  cellPlno: CellPlno = {
    id: 0,
    plno: 'all',
    place: null,
    cellID: 0,
    cellCode: null,
    cellName: null,
    updateBy: null,
    updateTime: null,
  };

  cells: Array<Select2OptionData>;
  cell_Plnos: Array<Select2OptionData>;
  lang: string;
  constructor(
    private cellPlnoService: CellPlnoService,
    private cellService: CellService,
    private sweetAlertifyService: SweetAlertService,
    private translate: TranslateService,
    private alertifyService: AlertifyService,
  ) { }

  ngOnInit() {
    this.getAllCell();
    this.getAllCellPlno();
  }

  getAllCell() {
    this.cellService.getAllCellAdmin().subscribe(res => {
      this.cells = res.map(item => {
        return { id: item.cellID.toString(), text: item.cellCode + '-' + item.cellName + '-' + item.buildingCode };
      });
      this.cells.unshift({ id: '0', text: this.translate.instant('admin.admincellplno.all_unit') });
    });
  }

  getAllCellPlno() {
    this.cellPlnoService.getAllCellPlno().subscribe(res => {
      this.cell_Plnos = res.map(item => {
        return { id: item.plno, text: item.plno + '-' + item.place };
      });
      this.cell_Plnos.unshift({ id: 'all', text: this.translate.instant('admin.admincellplno.all_position') });
    });
  }

  addCellPlno() {
    this.lang = localStorage.getItem('lang');
    if (this.cellPlno.cellID == 0) {
      return this.sweetAlertifyService.warning('Error', this.translate.instant('alert.admin.please_select_the_unit'));
    }
    if (this.cellPlno.plno == 'all') {
      return this.sweetAlertifyService.warning('Error', this.translate.instant('alert.admin.please_select_a_location'));
    }

    this.cellPlno.cellID = +this.cellPlno.cellID;
    this.cellPlno.plno = this.cellPlno.plno;

    this.cellPlnoService.addCellPlno(this.cellPlno, this.lang).subscribe((res: OperationResult) => {
      if (res.success) {
        this.sweetAlertifyService.success('Success', res.message);
        this.keyword.emit(this.cellPlno.plno);
        this.cancel();
      } else {
        this.sweetAlertifyService.error('Error', res.message);
      }
    }, error => {
      this.alertifyService.error(this.translate.instant('error.system_error'));
    });
  }

  cancel() {
    this.cellPlno.cellID = 0;
    this.cellPlno.plno = 'all';
  }
}
