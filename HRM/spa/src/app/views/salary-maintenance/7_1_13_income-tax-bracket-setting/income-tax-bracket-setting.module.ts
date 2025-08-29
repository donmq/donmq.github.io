import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { IncomeTaxBracketSettingRoutingModule } from './income-tax-bracket-setting-routing.module';
import { MainComponent } from "./main/main.component";
import { FormComponent } from "./form/form.component";
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { DragScrollComponent, DragScrollItemDirective } from 'ngx-drag-scroll';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';

@NgModule({
  declarations: [
    MainComponent,
    FormComponent
  ],
  imports: [
    CommonModule,
    IncomeTaxBracketSettingRoutingModule,
    TranslateModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
    DragScrollComponent,
    DragScrollItemDirective,
    NgxMaskDirective,
    NgxMaskPipe,
  ]
})
export class IncomeTaxBracketSettingModule { }
