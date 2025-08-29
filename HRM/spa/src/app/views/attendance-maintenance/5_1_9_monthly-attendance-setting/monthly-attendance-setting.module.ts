import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { MonthlyAttendanceSettingRoutingModule } from './monthly-attendance-setting-routing.module';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';
import { SharedModule } from '@views/_shared/shared.module';

@NgModule({
  imports: [
    CommonModule,
    TranslateModule,
    MonthlyAttendanceSettingRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
    ModalModule.forRoot(),
    SharedModule
  ],
  declarations: [
    MainComponent,
    FormComponent
  ],
  exports: []
})
export class MonthlyAttendanceSettingModule { }
