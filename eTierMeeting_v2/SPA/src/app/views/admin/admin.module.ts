import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AdminRoutingModule } from './admin-routing.module';
import { ModalModule } from 'ngx-bootstrap/modal';
import { InventoryCalendarComponent } from './inventory-calendar/inventory-calendar.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgSelect2Module } from 'ng-select2';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { UserAddComponent } from './user/user-add/user-add.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { EmployeeAddComponent } from './employee/employee-add/employee-add.component';
import { AdminHomeComponent } from './admin-home/admin-home.component';
import { LogoutComponent } from './logout/logout.component';
import { ErrorAdminComponent } from './error-admin/error-admin.component';
import { UserHomeComponent } from './user/user-home/user-home.component';
import { EmployyeeHomeComponent } from './employee/employyee-home/employyee-home.component';
import { PdcHomeComponent } from './pdc/pdc-home/pdc-home.component';
import { PdcAddComponent } from './pdc/pdc-add/pdc-add.component';
import { BuildingHomeComponent } from './building/building-home/building-home.component';
import { BuildingAddComponent } from './building/building-add/building-add.component';
import { CellplnoHomeComponent } from './cellplno/cellplno-home/cellplno-home.component';
import { CellplnoAddComponent } from './cellplno/cellplno-add/cellplno-add.component';
import { CellHomeComponent } from './cell/cell-home/cell-home.component';
import { CellAddComponent } from './cell/cell-add/cell-add.component';
import { DateTimeAdapter, OwlDateTimeModule, OwlNativeDateTimeModule, OWL_DATE_TIME_FORMATS, OWL_DATE_TIME_LOCALE } from 'ng-pick-datetime';
import { MomentDateTimeAdapter } from 'ng-pick-datetime-moment';
import { PreliminaryAddComponent } from './preliminary/preliminary-add/preliminary-add.component';
import { PreliminaryHomeComponent } from './preliminary/preliminary-home/preliminary-home.component';
import { PreliminaryEditComponent } from './preliminary/preliminary-edit/preliminary-edit.component';

export const MY_CUSTOM_FORMATS = {
  parseInput: 'yyyy/MM/DD HH:mm',
  fullPickerInput: 'YYYY/MM/DD HH:mm',
  datePickerInput: 'YYYY/MM/DD',
  timePickerInput: ' HH:mm',
  monthYearLabel: 'YYYY MMM',
  dateA11yLabel: 'LL',
  monthYearA11yLabel: 'YYYY MMMM',
};

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    AdminRoutingModule,
    ModalModule.forRoot(),
    NgxSpinnerModule,
    PaginationModule.forRoot(),
    ReactiveFormsModule,
    NgSelect2Module,
    NgSelectModule,
    TranslateModule,
    BsDatepickerModule.forRoot(),
    OwlDateTimeModule,
    OwlNativeDateTimeModule,
  ],
  declarations: [
    InventoryCalendarComponent,
    EmployyeeHomeComponent,
    UserHomeComponent,
    BuildingHomeComponent,
    BuildingAddComponent,
    CellHomeComponent,
    CellAddComponent,
    CellplnoHomeComponent,
    CellplnoAddComponent,
    PdcHomeComponent,
    UserAddComponent,
    EmployeeAddComponent,
    AdminHomeComponent,
    LogoutComponent,
    ErrorAdminComponent,
    PdcAddComponent,
    PreliminaryAddComponent,
    PreliminaryHomeComponent,
    PreliminaryEditComponent
  ],
  providers: [
    { provide: DateTimeAdapter, useClass: MomentDateTimeAdapter },
    { provide: OWL_DATE_TIME_FORMATS, useValue: MY_CUSTOM_FORMATS }
  ]
})
export class AdminModule { }
