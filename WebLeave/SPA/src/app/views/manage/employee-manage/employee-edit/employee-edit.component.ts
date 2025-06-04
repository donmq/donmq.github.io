import { Component, OnInit } from '@angular/core';
import { Employee } from '@models/manage/employee-manage/employee';
import { EmployeeService } from '@services/manage/employee.service';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { DestroyService } from '@services/destroy.service';
import { takeUntil } from 'rxjs';
import { InjectBase } from '@utilities/inject-base-app';


@Component({
  selector: 'app-employee-edit',
  templateUrl: './employee-edit.component.html',
  styleUrls: ['./employee-edit.component.scss'],
  providers: [DestroyService]
})
export class EmployeeEditComponent extends InjectBase implements OnInit {
  listGroupBase = [];
  listPositionID = [];
  listPartID = [];
  listDeptID = [];

  acceptUpdate: boolean = false;
  lang: string = localStorage.getItem(LocalStorageConstants.LANG).toLowerCase();
  public visibleData = [
    {
      key: true,
      value: true
    },
    {
      key: false,
      value: false
    }
  ]
  editEmployee: Employee = <Employee>{
  };
  _data: Employee;
  visible: string = '';

  constructor(
    private employeeService: EmployeeService,
  ) {
    super();
  }


  ngOnInit(): void {
    //Có 2 cách để gửi data qua
    //Cách 1: Gửi từ trang List qua
    // const data = this.employeeService.currentemployee.subscribe((res: any) => {
    //   if(res == null){
    //     this.backList();
    //   }
    //   // this.editEmployee = res;
    // })

    //Cách 2: Gửi qua router
    this.editEmployee.empID = Number(this.route.snapshot.paramMap.get('id'));
    this._data = this.editEmployee;
    this.getDetail(this.editEmployee.empID, this.lang);
    this.getListGroupBase();
    this.getListPositionID();
    this.getListDeptID();
    this.translateService.onLangChange.pipe(takeUntil(this.destroyService.destroys$)).subscribe({
      next: (event) => {
        this.lang = event.lang === 'zh' ? 'zh-TW' : event.lang;
        this.getDetail(this.editEmployee.empID, this.lang);
      }

    })

  }


  getDetail(empID: number, lang: string) {
    this.spinnerService.show();
    this.employeeService.getDetail(empID, lang).subscribe((item: any) => {
      this.editEmployee.empNumber = item.numberID
      this.editEmployee.empName = item.fullname
      this.editEmployee.deptID = item.deptID
      this.editEmployee.partID = item.partID
      this.editEmployee.dateIn = item.date_In_Edit
      this.editEmployee.positionID = item.positionID
      this.editEmployee.gbid = item.gbid
      this.editEmployee.visible = item.voHieuBoolean

      //this.editEmployee.visible = parseInt(item.voHieu) === 1 ? true : false

      this.getListPartID(this.editEmployee.deptID);
      this._data = this.editEmployee
      this.spinnerService.hide();
    });
  }

  onChecked(event: boolean) {
    this.editEmployee.visible = event;
  }

  getListDeptID() {
    this.employeeService.getListDeptID().subscribe({
      next: (res) => {
        this.listDeptID = res.map(item => {
          return {
            key: parseInt(item.key),
            value: item.value
          }
        })
        this.listDeptID.unshift({ key: 0, value: '' });
      }
    })
  }

  getListPartID(deptID: number) {
    this.spinnerService.show();
    this.employeeService.getListPartID(deptID).subscribe({
      next: (res) => {
        this.listPartID = res.map(item => {
          return { key: parseInt(item.key), value: item.value }
        })
        //  Nếu list có thì gán bằng giá trị đầu tiên
        this.listPartID.unshift({ key: 0, value: '' });
        this.spinnerService.hide();
      }
    });
  }
  //Khi Chọn cột DeptID thì PartID thay đổi theo
  changeDept(args) {
    // Set value của PartID về giá trị mặc định == 0
    if (this.editEmployee.deptID == args) {
      this.editEmployee.partID = 0;
      this.getListPartID(args);
    }

  }

  getListPositionID() {
    this.spinnerService.show();
    this.employeeService.getListPositionID().subscribe({
      next: (res) => {
        this.listPositionID = res.map(item => {
          return {
            key: parseInt(item.key),
            value: item.value
          }
        })
        this.spinnerService.hide();
      }
    });
  }

  getListGroupBase() {
    this.spinnerService.show();
    this.employeeService.getListGroupBase().subscribe({
      next: (res) => {
        this.listGroupBase = res.map(item => {
          return {
            key: parseInt(item.key),
            value: item.value
          }
        })
        this.spinnerService.hide();
      }
    });
  }

  backList() {
    this.router.navigateByUrl('/manage/employee');
  }
  reset() {
    this.editEmployee = { ...this._data };
    this.getListPartID(this.editEmployee.deptID)
  }

  save() {
    //this.editEmployee.dateIn = this.datePipe.transform(this.editEmployee.dateIn,'YYYY-MM-dd HH:mm:ss ')
    // this.editEmployee.visible = this.visible === "true" ? true : false;
    //  Convert string to Number
    this.editEmployee.dateIn = JSON.parse(JSON.stringify(this.editEmployee.dateIn));
    this.editEmployee.deptID = Number.parseInt(this.editEmployee.deptID.toString());

    this.spinnerService.show();
    this.employeeService.updateEmployee(this.editEmployee).subscribe({
      next: (res) => {
        this.spinnerService.hide();
        if (res.isSuccess) {
          this.snotifyService.success(this.translateService.instant('System.Message.UpdateOKMsg'), this.translateService.instant('System.Caption.Success'));
          this.backList();
        }
        else this.snotifyService.error(this.translateService.instant('System.Message.UpdateErrorMsg'), this.translateService.instant('System.Caption.Error'));
      },
      error: () => {
        this.snotifyService.error(this.translateService.instant('System.Message.UpdateErrorMsg'), this.translateService.instant('System.Caption.Error'));
        this.spinnerService.hide();
      }
    })
  }

}
