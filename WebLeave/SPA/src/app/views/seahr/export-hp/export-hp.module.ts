import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ExportHpRoutingModule } from './export-hp-routing.module';
import { ExportHpMainComponent } from './export-hp-main/export-hp-main.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ModalModule } from "ngx-bootstrap/modal";
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { NgSelectModule } from '@ng-select/ng-select';
import { PaginationModule } from "ngx-bootstrap/pagination";
import { AlertModule } from 'ngx-bootstrap/alert';
import { TranslateModule } from '@ngx-translate/core';
@NgModule({
  declarations: [
    ExportHpMainComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    ModalModule.forRoot(),
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
    AlertModule.forRoot(),
    NgSelectModule,
    TranslateModule,
    ExportHpRoutingModule
  ]
})
export class ExportHpModule { }
