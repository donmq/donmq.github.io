import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ExitEmployeeMasterFileHistoricalDataRoutingModule } from './exit-employee-master-file-historical-data-routing.module';
import { MainComponent } from './main/main.component';
import { QueryComponent } from './query/query.component';


@NgModule({
  declarations: [MainComponent, QueryComponent],
  imports: [
    CommonModule,
    ExitEmployeeMasterFileHistoricalDataRoutingModule,
    FormsModule,
    TranslateModule,
    NgSelectModule,
    ReactiveFormsModule,
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
  ]
})
export class ExitEmployeeMasterFileHistoricalDataModule { }
