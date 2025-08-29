import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';
import { SalaryAdditionsAndDeductionsInputRoutingModule } from './7_19_salary-additions-and-deductions-input-routing.module';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { DragScrollComponent } from 'ngx-drag-scroll';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';

@NgModule({
  imports: [
    CommonModule,
    SalaryAdditionsAndDeductionsInputRoutingModule,
    FormsModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
    TranslateModule,
    DragScrollComponent,
    NgSelectModule,
    NgxMaskDirective,
    NgxMaskPipe,
    TypeaheadModule.forRoot()
  ],
  declarations: [MainComponent, FormComponent]
})
export class SalaryAdditionsAndDeductionsInputModule { }
