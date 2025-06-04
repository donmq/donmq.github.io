import { Component, OnInit } from '@angular/core';
import { BuildingInformation } from '@models/manage/building-manage/building-information';
import { BuildingManageService } from '@services/manage/building-manage.service';
import { InjectBase } from '@utilities/inject-base-app';
import { KeyValuePair } from '@utilities/key-value-pair';

@Component({
  selector: 'app-building-add',
  templateUrl: './building-add.component.html',
  styleUrls: ['./building-add.component.scss']
})
export class BuildingAddComponent extends InjectBase implements OnInit {
  building: BuildingInformation = <BuildingInformation>{
    visible: null
  };
  areas: KeyValuePair[] = [];
  constructor(
    private buildingManageService: BuildingManageService,
  ) {
    super()
  }

  ngOnInit() {
    this.getListArea();
  }

  getListArea() {
    this.buildingManageService.getListArea().subscribe({
      next: (res) => {
        this.areas = res;
        this.building.areaID = this.areas.find(x => x.value.includes('NONE')).key;
      },
      error: (err) => {
        this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
      }
    });
  }

  clear() {
    this.building = <BuildingInformation>{
      areaID: this.areas.find(x => x.value.includes('NONE')).key,
      visible: null
    };
  }
  saveAdd() {
    this.building.buildingName = `${this.building.buildingNameVi.trim()}-${this.building.buildingNameZh}`;
    this.buildingManageService.addBuilding(this.building).subscribe({
      next: (res) => {
        this.snotifyService.success(this.translateService.instant('System.Message.CreateOKMsg'), this.translateService.instant('System.Caption.Success'));
        this.router.navigate(['/manage/building']);
      },
      error: (err) => {
        this.snotifyService.error(this.translateService.instant('System.Message.CreateErrorMsg'), this.translateService.instant('System.Caption.Error'));
      },
      complete: () => {
        this.router.navigate(['/manage/building']);
      }
    })
  }
}
