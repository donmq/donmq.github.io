import { Component, OnInit, effect } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { IconButton } from '@constants/common.constants';
import { LangConstants } from '@constants/lang-constants';
import { LocalStorageConstants } from '@constants/local-storage.constants';
import {
  Org_Direct_DepartmentParam,
  Org_Direct_DepartmentResult,
} from '@models/organization-management/3_1_4-direct-department-setting';
import { LangChangeEvent } from '@ngx-translate/core';
import { S_3_1_4_DirectDepartmentSettingService } from '@services/organization-management/s_3_1_4_direct-department-setting.service';
import { InjectBase } from '@utilities/inject-base-app';
import { KeyValuePair } from '@utilities/key-value-pair';
@Component({
  selector: 'app-form',
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.css'],
})
export class FormComponent extends InjectBase implements OnInit {
  isDuplicate: boolean = false;
  listDivision: KeyValuePair[] = [];
  listFactory: KeyValuePair[] = [];
  listDepartment: KeyValuePair[] = [];
  listLine: KeyValuePair[] = [];
  listDirectDepartmentAttribute: KeyValuePair[] = [];
  iconButton = IconButton;
  action: string;
  param: Org_Direct_DepartmentParam = <Org_Direct_DepartmentParam>{};
  paramArray: Org_Direct_DepartmentResult[] = [];
  title: string = '';
  url: string = '';
  isChange: boolean;
  formType: string = ''
  constructor(private service: S_3_1_4_DirectDepartmentSettingService) {
    super();
    this.translateService.onLangChange
      .pipe(takeUntilDestroyed())
      .subscribe((event: LangChangeEvent) => {
        this.title = this.functionUtility.getTitle(this.route.snapshot.data['program'])
        this.isChange = false;
        this.getListDivision();
        this.getListFactory();
        this.getListDepartment();
      });
    this.getDataFromSource();
  }
  ngOnInit(): void {
    this.title = this.functionUtility.getTitle(this.route.snapshot.data['program']);
    this.url = this.functionUtility.getRootUrl(this.router.routerState.snapshot.url);
    this.getListDivision();
    this.getListFactory();
    this.getListDirectDepartmentAttribute();
    this.route.data.subscribe(
      (res) => {
        this.formType = res['title']
        this.action = `System.Action.${this.formType}`
      }
    );
  }
  getDataFromSource() {
    effect(() => {
      let source = this.service.paramSearch();
      this.param = { ...source.selectedData };
      if (this.formType == 'Edit') {
        if (Object.keys(this.param).length == 0)
          this.back();
        else {
          this.getListDepartment();
          this.getListLine();
          this.service.getdetail(this.param).subscribe({
            next: (res) => {
              this.paramArray = res;
            },
          });
        }
      }
      else
        this.paramArray.push(<Org_Direct_DepartmentResult>{
          line_Code: null,
          direct_Department_Attribute: null,
        });
    });
  }
  back = () => this.router.navigate([this.url]);

  checkParam() {
    return (
      this.functionUtility.checkEmpty(this.param.division) ||
      this.functionUtility.checkEmpty(this.param.factory) ||
      this.functionUtility.checkEmpty(this.param.department_Code) ||
      this.paramArray.some(
        (x) => x.line_Code == null || x.direct_Department_Attribute == null
      ) ||
      this.paramArray.length < 1 ||
      this.isDuplicate
    );
  }

  onLineCodeChange(data: Org_Direct_DepartmentResult) {
    data.line_Code != null ? this.checkDuplicate() : (this.isDuplicate = false);
  }
  getListDivision() {
    this.spinnerService.show();
    this.service.getListDivision().subscribe({
      next: (res) => {
        this.listDivision = res;
        this.spinnerService.hide();
      }
    });
  }
  clear() {
    if (this.formType == 'Add' && this.isChange)
      this.deleteProperty('department_Code');

    if (this.paramArray.length > 0) {
      this.paramArray.forEach((item) => {
        item.line_Code = null;
      });
    }
    this.getListDepartment();
    this.getListLine();
  }
  getListDepartment() {
    this.spinnerService.show();
    this.isChange = true;
    this.service
      .getListDepartment()
      .subscribe({
        next: (res) => {
          this.listDepartment = res;
          this.spinnerService.hide();
        }
      });
  }
  getListFactory() {
    this.spinnerService.show();
    if (this.formType == 'Add' && this.isChange) {
      this.deleteProperty('factory');
      this.deleteProperty('department_Code');
    }
    this.service.getListFactory(this.param.division).subscribe({
      next: (res) => {
        this.listFactory = res;
        this.spinnerService.hide();
      }
    });
  }
  getListLine() {
    this.spinnerService.show();
    this.paramArray.forEach((item) => {
      item.line_Code = null;
    });
    this.service
      .getListLine(this.param.division, this.param.factory)
      .subscribe({
        next: (res) => {
          this.listLine = res;
          this.spinnerService.hide();
        },
        error: () => { },
      });
  }
  deleteItem(index: number) {
    this.paramArray.splice(index, 1);
    this.checkDuplicate();
  }

  AddItem() {
    if (this.paramArray.filter((x) => x.line_Code == null || x.direct_Department_Attribute == null).length > 0)
      return this.functionUtility.snotifySuccessError(false, 'OrganizationManagement.DirectDepartmentSetting.EmptyRow')

    if (this.isDuplicate)
      return this.functionUtility.snotifySuccessError(false, 'OrganizationManagement.DirectDepartmentSetting.RepeatedData')

    this.paramArray.push(<Org_Direct_DepartmentResult>{
      line_Code: null,
      direct_Department_Attribute: null,
    });
  }
  getListDirectDepartmentAttribute() {
    this.spinnerService.show();
    this.service.GetListDirectDepartmentAttribute().subscribe({
      next: (res) => {
        this.listDirectDepartmentAttribute = res;
        this.spinnerService.hide();
      }
    });
  }
  saveChange() {
    const uniqueKeys = new Set();

    for (const item of this.paramArray) {
      const key = `${item.line_Code}-${item.direct_Department_Attribute}`;
      if (uniqueKeys.has(key)) {
        return this.snotifyService.warning(
          `${this.translateService.instant(
            'OrganizationManagement.DirectDepartmentSetting.LineCode'
          )} : ${key.split('-')[0]} & ${this.translateService.instant(
            'OrganizationManagement.DirectDepartmentSetting.SectionCode'
          )} : ${key.split('-')[1]} is exist`,
          this.translateService.instant('System.Caption.Warning')
        );
      }
      uniqueKeys.add(key);
    }

    this.paramArray.forEach((item) => {
      item.factory = this.param.factory;
      item.department_Code = this.param.department_Code;
      item.division = this.param.division;
    });

    this.spinnerService.show();
    this.service[this.formType == 'Add' ? 'addNew' : 'edit'](this.paramArray).subscribe({
      next: (result) => {
        this.functionUtility.snotifySuccessError(result.isSuccess,
          result.isSuccess ? (this.formType == 'Add' ? 'System.Message.CreateOKMsg' : 'System.Message.UpdateOKMsg') : result.error,
          result.isSuccess)
        if (result.isSuccess) this.back();
      },
      error: () => this.functionUtility.snotifySystemError(),
      complete: () => this.spinnerService.hide(),
    });
  }

  deleteProperty = (name: string) => delete this.param[name];

  checkDuplicate() {
    if (this.paramArray.filter((x) => x.line_Code == null).length == 0) {
      this.spinnerService.show();
      const lookup = this.paramArray.reduce((temp, val) => {
        temp[val.line_Code] = ++temp[val.line_Code] || 0;
        return temp;
      }, {});
      const duplicate = this.paramArray.filter((val) => lookup[val.line_Code]);
      this.isDuplicate = duplicate.length > 0 ? true : false;
      if (!this.isDuplicate) {
        if (this.formType == 'Add') {
          this.paramArray.forEach((item) => {
            item.factory = this.param.factory;
            item.department_Code = this.param.department_Code;
            item.division = this.param.division;
          });
          this.service.checkDuplicate(this.paramArray).subscribe({
            next: (result) => {
              this.spinnerService.hide();
              if (result.isSuccess) this.isDuplicate = false;
              else {
                this.isDuplicate = true;
                this.functionUtility.snotifySuccessError(false, 'OrganizationManagement.DirectDepartmentSetting.RepeatedData')
              }
            },
            error: () => this.functionUtility.snotifySystemError()
          });
        } else this.spinnerService.hide();
      }
      else {
        this.spinnerService.hide();
        this.functionUtility.snotifySuccessError(false, 'OrganizationManagement.DirectDepartmentSetting.RepeatedData')
      }
    }
  }
}
