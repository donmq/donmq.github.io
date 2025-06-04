import { Component, OnInit } from '@angular/core';
import { BuildingInformation } from '@models/manage/building-manage/building-information';
import { BuildingManageService } from '@services/manage/building-manage.service';
import { InjectBase } from '@utilities/inject-base-app';
import { KeyValuePair } from '@utilities/key-value-pair';

@Component({
  selector: 'app-building-edit',
  templateUrl: './building-edit.component.html',
  styleUrls: ['./building-edit.component.scss']
})
export class BuildingEditComponent extends InjectBase implements OnInit {
  buildingEdit: BuildingInformation;
  buildingConst: BuildingInformation;
  areas: KeyValuePair[] = [];
  constructor(
    private buildingManageService: BuildingManageService,
  ) {
    super();
  }

  ngOnInit() {
    this.buildingManageService.buildingSource.asObservable().subscribe(res => this.buildingEdit = res);
    if (this.buildingEdit == null) {
      this.router.navigate(['/manage/building']);
    }
    this.getListArea();
  }

  getListArea() {
    this.buildingManageService.getListArea().subscribe({
      next: (res) => {
        this.areas = res;
        if (!this.buildingEdit.areaID)
          this.buildingEdit.areaID = this.areas.find(x => x.value.includes('NONE')).key;

        this.buildingConst = { ...this.buildingEdit };
      },
      error: (err) => {
        this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
      }
    });
  }

  saveEdit() {
    this.buildingEdit.buildingName = `${this.buildingEdit.buildingNameVi.trim()}-${this.buildingEdit.buildingNameZh}`;
    this.buildingManageService.editBuilding(this.buildingEdit).subscribe({
      next: (res) => {
        if (res) {
          this.snotifyService.success(this.translateService.instant('System.Message.UpdateOKMsg'), this.translateService.instant('System.Caption.Success'));
          this.router.navigate(['/manage/building'])
        } else {
          this.snotifyService.success(this.translateService.instant('System.Message.UpdateErrorMsg'), this.translateService.instant('System.Caption.Error'));
        }
      },
      error: (error) => { this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error')); },
      complete: () => { this.router.navigate(['/manage/building']) }
    })
  }

  clear() {
    this.buildingEdit = { ...this.buildingConst };
  }
}
