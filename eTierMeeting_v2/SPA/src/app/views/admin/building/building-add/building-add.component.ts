import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { Building } from '../../../../_core/_models/building';
import { OperationResult } from '../../../../_core/_models/operation-result';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { BuildingService } from '../../../../_core/_services/building.service';
import { SweetAlertService } from '../../../../_core/_services/sweet-alert.service';

@Component({
  selector: 'app-building-add',
  templateUrl: './building-add.component.html',
  styleUrls: ['./building-add.component.scss']
})
export class BuildingAddComponent implements OnInit {
  @Output() keyword = new EventEmitter<string>();
  building: Building = {
    buildingID: 0,
    buildingCode: null,
    buildingName: null,
    visible: true
  };
  lang: string;
  constructor(
    private buildingService: BuildingService,
    private sweetAlertifyService: SweetAlertService,
    private translate: TranslateService,
    private alertifyService: AlertifyService,
  ) { }
  ngOnInit() {
  }

  addBuilding() {
    this.lang = localStorage.getItem('lang');
    if (this.building.buildingName == null || this.building.buildingName.trim() === '') {
      return this.sweetAlertifyService.warning('Error', this.translate.instant('alert.admin.please_enter_building_name'));
    }
    if (this.building.buildingCode == null || this.building.buildingCode.trim() === '') {
      return this.sweetAlertifyService.warning('Error', this.translate.instant('alert.admin.please_enter_building_code'));
    }
    this.building.buildingName = this.building.buildingName.trim();
    this.building.buildingCode = this.building.buildingCode.trim();
    this.buildingService.addBuilding(this.building, this.lang).subscribe((res: OperationResult) => {
      if (res.success) {
        this.sweetAlertifyService.success('Success', res.message);
        this.keyword.emit(this.building.buildingName);
        this.cancel();
      } else {
        this.sweetAlertifyService.error('Error', res.message);
      }
    }, error => {
      this.alertifyService.error(this.translate.instant('error.system_error'));
    });
  }

  cancel() {
    this.building.buildingName = '';
    this.building.buildingCode = '';
  }
}
