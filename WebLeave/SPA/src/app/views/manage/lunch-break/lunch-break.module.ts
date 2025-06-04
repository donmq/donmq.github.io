import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { LunchBreakRoutingModule } from './lunch-break-routing.module';
import { MainComponent } from './main/main.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TranslateModule } from '@ngx-translate/core';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule } from '@angular/forms';
import { FormComponent } from './form/form.component';
import { A2Edatetimepicker } from 'ng2-eonasdan-datetimepicker';


@NgModule({
  declarations: [
    MainComponent,
    FormComponent
  ],
  imports: [
    CommonModule,
    LunchBreakRoutingModule,
    FormsModule,
    PaginationModule,
    ModalModule.forRoot(),
    TranslateModule,
    A2Edatetimepicker
  ]
})
export class LunchBreakModule { }
