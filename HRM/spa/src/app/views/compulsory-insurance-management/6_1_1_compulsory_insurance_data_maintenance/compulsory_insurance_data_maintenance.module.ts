import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { CompulsoryInsuranceDataMaintenanceRoutingModule } from './compulsory_insurance_data_maintenance-routing.module';
import { FormComponent } from './form/form.component';
import { MainComponent } from './main/main.component';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';

@NgModule({
  imports: [
    CommonModule,
    CompulsoryInsuranceDataMaintenanceRoutingModule,
    FormsModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
    TranslateModule,
    NgSelectModule,
    TypeaheadModule.forRoot(),
    NgxMaskDirective,
    NgxMaskPipe,
  ],
  declarations: [MainComponent, FormComponent]
})
export class CompulsoryInsuranceDataMaintenanceModule { }
