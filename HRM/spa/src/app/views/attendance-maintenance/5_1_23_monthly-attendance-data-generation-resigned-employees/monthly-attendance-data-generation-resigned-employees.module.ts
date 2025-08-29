import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MonthlyAttendanceDataGenerationResignedEmployeesRoutingModule } from './monthly-attendance-data-generation-resigned-employees-routing.module';
import { MainComponent } from './main/main.component';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { Tab1Component } from './tab-1/tab-1.component';
import { Tab2Component } from './tab-2/tab-2.component';
import { SharedModule } from '@views/_shared/shared.module';

@NgModule({
  declarations: [MainComponent, Tab1Component, Tab2Component],
  imports: [
    CommonModule,
    MonthlyAttendanceDataGenerationResignedEmployeesRoutingModule,
    FormsModule,
    PaginationModule.forRoot(),
    TranslateModule,
    NgSelectModule,
    BsDatepickerModule.forRoot(),
    SharedModule
  ],
})
export class MonthlyAttendanceDataGenerationResignedEmployeesModule {}
