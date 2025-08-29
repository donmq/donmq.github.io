import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MonthlySalaryGenerationExitedEmployeesRoutingModule } from './monthly-salary-generation-exited-employees-routing.module';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { MainComponent } from './main/main.component';
import { SharedModule } from '@views/_shared/shared.module';


@NgModule({
  declarations: [MainComponent],
  imports: [
    CommonModule,
    MonthlySalaryGenerationExitedEmployeesRoutingModule,
    FormsModule,
    PaginationModule.forRoot(),
    TranslateModule,
    NgSelectModule,
    BsDatepickerModule.forRoot(),
    SharedModule
  ]
})
export class MonthlySalaryGenerationExitedEmployeesModule { }
