import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from "./main/main.component";
import { MonthlyAttendanceDataGenerationActiveEmployeesRoutingModule } from './monthly-attendance-data-generation-active-employees-routing.module';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { Tab1Component } from "./tab-1/tab-1.component";
import { Tab2Component } from "./tab-2/tab-2.component";
import { Tab3Component } from "./tab-3/tab-3.component";
import { SharedModule } from '@views/_shared/shared.module';

@NgModule({
  declarations: [
    MainComponent,
    Tab1Component,
    Tab2Component,
    Tab3Component
  ],
  imports: [
    CommonModule,
    MonthlyAttendanceDataGenerationActiveEmployeesRoutingModule,
    FormsModule,
    PaginationModule.forRoot(),
    TranslateModule,
    NgSelectModule,
    BsDatepickerModule.forRoot(),
    SharedModule
  ]
})
export class MonthlyAttendanceDataGenerationActiveEmployeesModule { }
