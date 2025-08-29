import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EmployeeOvertimeDataSheetRoutingModule } from './employee-over-time-data-sheet-routing.module';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { MainComponent } from './main/main.component';



@NgModule({
  declarations: [
    MainComponent
  ],
  imports: [
    CommonModule,
    EmployeeOvertimeDataSheetRoutingModule,
    FormsModule,
    TranslateModule,
    NgSelectModule,
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
  ]
})
export class EmployeeOvertimeDataSheetModule { }
