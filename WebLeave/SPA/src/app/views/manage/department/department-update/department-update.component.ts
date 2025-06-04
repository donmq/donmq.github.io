import { Component, OnInit } from '@angular/core';
import { Departments } from '@models/manage/departments';
import { DepartmentLangParam } from '@params/manage/departmentLangParam';
import { DepartmentService } from '@services/manage/Department.service';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-department-update',
  templateUrl: './department-update.component.html',
  styleUrls: ['./department-update.component.scss'],
})
export class DepartmentUpdateComponent extends InjectBase implements OnInit {
  areaList: any;
  buildingList: any;
  editDepartment: Departments = <Departments>{};
  detpLang: DepartmentLangParam = <DepartmentLangParam>{};
  constructor(
    private _service: DepartmentService,
  ) {
    super();
  }

  ngOnInit(): void {
    this.getData();
    this.getAllAreas();
    this.getAllBuldings();
  }
  getData() {
    this._service.currentDepartmentSource.subscribe({
      next: (res) => {
        this.editDepartment = res;
      },
    });
  }
  getAllAreas() {
    this._service.getAllAreas().subscribe({
      next: (res) => {
        this.areaList = res;
      },
    });
  }
  getAllBuldings() {
    this._service.getAllBuildings().subscribe({
      next: (res) => {
        this.buildingList = res;
      },
    });
  }
  save() {
    this.spinnerService.show();
    this._service.update(this.editDepartment).subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.snotifyService.success(this.translateService.instant('System.Message.UpdateOKMsg'), this.translateService.instant('System.Caption.Success'));
        } else {
          this.snotifyService.error(this.translateService.instant('System.Message.UpdateErrorMsg'), this.translateService.instant('System.Caption.Error'));
        }
        this.spinnerService.hide();
      },
      error: () => {
        this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
      },
      complete: () => {
        this.spinnerService.hide();
        this.router.navigate(['manage/department']);
      }
    });
  }
}
