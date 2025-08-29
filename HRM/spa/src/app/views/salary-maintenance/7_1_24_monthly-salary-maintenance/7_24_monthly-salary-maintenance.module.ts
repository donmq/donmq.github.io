import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MonthlySalaryMaintenanceRoutingModule } from './7_24_monthly-salary-maintenance-routing.module';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';
import { DragScrollComponent } from 'ngx-drag-scroll';

@NgModule({
  imports: [
    CommonModule,
    MonthlySalaryMaintenanceRoutingModule,
    FormsModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
    TranslateModule,
    NgSelectModule,
    TypeaheadModule.forRoot(),
    CollapseModule.forRoot(),
    NgxMaskDirective,
    NgxMaskPipe,
    DragScrollComponent
  ],
  declarations: [MainComponent, FormComponent]
})
export class MonthlySalaryMaintenanceModule { }
