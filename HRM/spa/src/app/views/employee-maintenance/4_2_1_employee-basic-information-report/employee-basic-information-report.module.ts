import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';

import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { EmployeeBasicInformationReportRoutingModule } from './employee-basic-information-report-routing.module';
import { MainComponent } from './main/main.component';


@NgModule({
  declarations: [
    MainComponent
  ],
  imports: [
    CommonModule,
    EmployeeBasicInformationReportRoutingModule,
    FormsModule,
    TranslateModule,
    NgSelectModule,
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
  ]
})
export class EmployeeBasicInformationReportModule { }
