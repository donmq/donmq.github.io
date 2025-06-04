import { ListPosition } from '@models/seahr/listPosition';
import { ListGroupBase } from '@models/seahr/listGroupBase';
import { LangConstants } from '@constants/lang.constants';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { SeaHrAddEmployeeService } from '@services/seahr/sea-hr-add-employee.service';
import { Employee, ResultDataUploadEmp } from '@models/seahr/employee';
import { Component, OnInit } from '@angular/core';
import { KeyValuePair } from '@utilities/key-value-pair';
import { DestroyService } from '@services/destroy.service';
import { takeUntil } from 'rxjs';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-seahr-add',
  templateUrl: './seahr-add.component.html',
  styleUrls: ['./seahr-add.component.scss'],
  providers: [DestroyService]
})
export class SeahrAddComponent extends InjectBase implements OnInit {

  departmentList: KeyValuePair[] = [];
  positionList: KeyValuePair[] = [];
  partList: KeyValuePair[] = [];
  groupBaseList: KeyValuePair[] = [];
  departmenId: number;
  flag: boolean = true;
  accept: string = '.xls, .xlsm, .xlsx';
  fileImportExcel: File = null;
  department: string = "";
  part: string = "";
  position: string = "";
  groupBase: string = "";
  listPosition: ListPosition[] = [];
  ListGroupBase: ListGroupBase[] = [];
  lang: string = localStorage.getItem(LocalStorageConstants.LANG) ?? LangConstants.VN;
  local: string = LangConstants.VN;
  employee: Employee = {} as Employee;
  isNotification: boolean = false;
  resultUpload: ResultDataUploadEmp[] = [];

  constructor(
    private service: SeaHrAddEmployeeService,
  ) {
    super();
    this.translateService.addLangs(["vi", "zh", "en"]);
    this.translateService.setDefaultLang("vi");
  }

  ngOnInit() {
    this.translateService.onLangChange.pipe(takeUntil(this.destroyService.destroys$)).subscribe(res => {
      this.lang = res.lang;
      this.getAllPosition();
      this.getAllGroupBase();
    });
    this.getAllDepartment();
    this.getAllPosition();
    this.getAllGroupBase();
  }

  getAllDepartment() {
    this.service.getAllDepartment().subscribe({
      next: (res: KeyValuePair[]) => {
        this.departmentList = res.map((item) => {
          return { key: item.key, value: item.value };
        });
      },
      error: () => {
        this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
      },
      complete: () => { }
    })
  }

  changeDepartment(event) {
    if (event != '' && event != null) {
      this.departmenId = event;
      this.getAllpart();
      this.flag = false;
    }
  }

  getAllPosition() {
    this.service.getAllPosition().subscribe({
      next: (res) => {
        this.listPosition = res;
        this.positionList = res.filter(item => item.languageID == this.lang).map((item) => {
          return { key: item.positionID, value: item.positionName };
        })
      },
      error: () => { this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error')); },
      complete: () => { }
    })
  }

  async getAllpart() {
    if (this.departmentList == null) {
      this.flag == true
      this.partList = null;
    }
    this.service.getAllPart(this.departmenId, this.lang).subscribe({
      next: (res) => {
        this.partList = res.map((item) => {
          this.employee.partID = item.key;
          return { key: item.key, value: item.value };
        });
      },
      error: () => { this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error')); },
      complete: () => { }
    })
  }

  getAllGroupBase() {
    this.service.getAllGroupBase().subscribe({
      next: (res) => {
        this.ListGroupBase = res;
        this.groupBaseList = res.filter(item => item.languageID == this.lang).map((item) => {
          return { key: item.gbid, value: item.baseName };
        })
      },
      error: () => { this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error')); },
      complete: () => { }
    })
  }

  async addEmployee() {
    this.spinnerService.show();

    this.employee.dateIn = this.employee.dateIn.toStringDate();
    this.service.addEmployee(this.employee).subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.snotifyService.success(this.translateService.instant('System.Message.CreateOKMsg'), this.translateService.instant('System.Caption.Success'));
          this.refresh();
          this.spinnerService.hide();
        }
        else {
          this.snotifyService.error(this.translateService.instant('System.Message.CreateErrorMsg'), this.translateService.instant('System.Caption.Error'));
          if (res.isSuccess) {
            this.spinnerService.hide();
            this.refresh();
          }
        }
      }, error: (error) => {
        this.spinnerService.hide();
        this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
      },
      complete: () => { }
    });
  }

  refresh() {
    this.employee = {} as Employee;
    this.partList = null;
    this.departmenId = null;

    this.flag = true;
  }

  onSelectFile(event: any) {
    if (event.target.files && event.target.files[0]) {
      // check file name extension
      const fileNameExtension = event.target.files[0].name.split('.').pop();
      if (!this.accept.includes(fileNameExtension)) {
        return this.snotifyService.warning(this.translateService.instant('System.Message.AllowExcelFile'),
          this.translateService.instant('System.Caption.Warning'));
      }
      this.fileImportExcel = event.target.files[0];
    }
  }

  onFileUploadInit() {
    let inputLabel = document.getElementById('labelFile');
    document.getElementById('input_uploadFile').addEventListener('change', function (e: any) {
      if (e.target.files && e.target.files.length > 0) {
        inputLabel.innerHTML = e.target.files[0].name;
      } else {
        inputLabel.innerHTML = 'Choose file...';
      }
    })
  }

  onRemoveFile() {
    (<HTMLInputElement>document.getElementById("input_uploadFile")).value = null;
    // document.getElementById('labelFile').innerHTML = 'Choose file...';
    this.fileImportExcel = null;
  }

  uploadFile() {
    if (this.fileImportExcel == null) {
      return this.snotifyService.warning(this.translateService.instant('System.Message.InvalidFile'),
        this.translateService.instant('System.Caption.Warning'));
    }
    this.spinnerService.show();
    this.service.uploadExcel(this.fileImportExcel).subscribe({
      next: (res) => {
        if (res.isSuccess) {
          this.isNotification = true;
          this.resultUpload = res.data
          this.snotifyService.success(
            this.translateService.instant('System.Message.UploadOKMsg'),
            this.translateService.instant('System.Caption.Success')
          );
          this.spinnerService.hide();
        } else {
          this.spinnerService.hide();
          this.isNotification = false;
          this.snotifyService.error(
            this.translateService.instant('System.Message.UploadErrorMsg'),
            this.translateService.instant('System.Caption.Error')
          );
        }
      }, error: () => {
        this.spinnerService.hide();
        this.snotifyService.error(
          this.translateService.instant('System.Message.UnknowError'),
          this.translateService.instant('System.Caption.Error')
        );
      },
      complete: () => { }
    });
    this.onRemoveFile();
  }

  exportExcel() {
    this.service.exportExcel();
  }

  back() {
    if (this.isNotification == true)
      this.isNotification = false;
    else
      this.router.navigate(['/seahr']);
  }

  checkValue(event: any) {
    // Prevent invalid characters
    if (event.key === '-' || event.key === '+' || event.key === 'e' || event.key === 'E' || event.key === '.') {
      event.preventDefault();
    }
  }

}
