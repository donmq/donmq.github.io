import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { EmployeeBasicInformationMaintenanceRoutingModule } from './employee-basic-information-maintenance-routing.module';

import { SharedModule } from '@views/_shared/shared.module';
import { ModalModule } from 'ngx-bootstrap/modal';

import { FormComponent411 } from './form/form.component';
import { MainPageComponent411 } from './main-page/main-page.component';
import { MainComponent411 } from './main/main.component';
import { ModalComponent411 } from './modal/modal.component';
import { UploadFormComponent411 } from './upload-form/upload-form.component';
import { ModalComponent412 } from '../4_1_2_employee-emergency-contacts/modal/modal.component';
import { MainComponent412 } from '../4_1_2_employee-emergency-contacts/main/main.component';
import { ModalFormComponent413 } from '../4_1_3_education/modal-form/modal-form.component';
import { ModalUploadComponent413 } from '../4_1_3_education/modal-upload/modal-upload.component';
import { MainComponent413 } from '../4_1_3_education/main/main.component';
import { ModalComponent414 } from '../4_1_4_dependent-information/modal/modal.component';
import { MainComponent414 } from '../4_1_4_dependent-information/main/main.component';
import { ModalComponent415 } from '../4_1_5_external-experience/modal/modal.component';
import { MainComponent415 } from '../4_1_5_external-experience/main/main.component';

@NgModule({
  declarations: [
    MainComponent411,
    FormComponent411,
    MainPageComponent411,
    UploadFormComponent411,
    ModalComponent411,
    ModalComponent412,
    MainComponent412,
    ModalFormComponent413,
    ModalUploadComponent413,
    MainComponent413,
    ModalComponent414,
    MainComponent414,
    ModalComponent415,
    MainComponent415
  ],
  imports: [
    CommonModule,
    EmployeeBasicInformationMaintenanceRoutingModule,
    FormsModule,
    TranslateModule,
    NgSelectModule,
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(),
    TabsModule.forRoot(),
    PaginationModule.forRoot(),
    ModalModule.forRoot(),
    CollapseModule.forRoot(),
    SharedModule
  ]
})
export class EmployeeBasicInformationMaintenanceModule { }
