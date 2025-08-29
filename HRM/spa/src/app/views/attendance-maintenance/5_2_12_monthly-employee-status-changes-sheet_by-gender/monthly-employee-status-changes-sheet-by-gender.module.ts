import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MonthlyEmployeeStatusChangesSheetByGenderRoutingModule } from './monthly-employee-status-changes-sheet-by-gender-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { MainComponent } from './main/main.component';


@NgModule({
  declarations: [
    MainComponent
  ],
  imports: [
    CommonModule,
    MonthlyEmployeeStatusChangesSheetByGenderRoutingModule,
    FormsModule,
    TranslateModule,
    NgSelectModule,
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(),
  ]
})
export class MonthlyEmployeeStatusChangesSheetByGenderModule { }
