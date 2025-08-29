import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdditionDeductionItemandAmountSettingsRoutingModule } from './addition-deduction-item-and-amount-settings-routing.module';
import { MainComponent } from "./main/main.component";
import { FormComponent } from "./form/form.component";
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { DragScrollComponent, DragScrollItemDirective } from 'ngx-drag-scroll';
import { PipesModule } from 'src/app/_core/pipes/pipes.module';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';

@NgModule({
  declarations: [
    MainComponent,
    FormComponent
  ],
  imports: [
    CommonModule,
    AdditionDeductionItemandAmountSettingsRoutingModule,
    TranslateModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
    DragScrollComponent,
    DragScrollItemDirective,
    PipesModule,
    NgxMaskDirective,
    NgxMaskPipe
  ]
})
export class AdditionDeductionItemandAmountSettingsModule { }
