import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MonthlyEmployeeStatusChangesSheetByReasonForResignationRoutingModule } from './monthly-employee-status-changes-sheet-by-reason-for-resignation-routing.module';
import { MainComponent } from './main/main.component';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

@NgModule({
  declarations: [
    MainComponent
  ],
  imports: [
    CommonModule,
    MonthlyEmployeeStatusChangesSheetByReasonForResignationRoutingModule,
    FormsModule,
    TranslateModule,
    NgSelectModule,
    BsDatepickerModule.forRoot(),
  ]
})
export class MonthlyEmployeeStatusChangesSheetByReasonForResignationModule { }
