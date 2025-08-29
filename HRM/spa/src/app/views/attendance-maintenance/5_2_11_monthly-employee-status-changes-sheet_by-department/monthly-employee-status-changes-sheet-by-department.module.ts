import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MonthlyEmployeeStatusChangesSheetByDepartmentRoutingModule } from './monthly-employee-status-changes-sheet-by-department-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { MainComponent } from './main/main.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

@NgModule({
  declarations: [
    MainComponent
  ],
  imports: [
    CommonModule,
    MonthlyEmployeeStatusChangesSheetByDepartmentRoutingModule,
    FormsModule,
    TranslateModule,
    NgSelectModule,
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(),
  ]
})
export class MonthlyEmployeeStatusChangesSheetByDepartmentModule { }
