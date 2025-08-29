import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PregnancyandMaternityDataMaintenanceRoutingModule } from './pregnancy-and-maternity-data-maintenance-routing.module';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { DragScrollComponent, DragScrollItemDirective } from 'ngx-drag-scroll';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';


@NgModule({
  declarations: [
    MainComponent,
    FormComponent
  ],
  imports: [
    CommonModule,
    PregnancyandMaternityDataMaintenanceRoutingModule,
    FormsModule,
    PaginationModule.forRoot(),
    TranslateModule,
    NgSelectModule,
    TypeaheadModule.forRoot(),
    BsDatepickerModule.forRoot(),
    DragScrollComponent,
    DragScrollItemDirective,
    NgxMaskDirective,
    NgxMaskPipe
  ]
})
export class PregnancyandMaternityDataMaintenanceModule { }
