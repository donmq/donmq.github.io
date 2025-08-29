import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';

import { CodeMaintenanceRoutingModule } from './code-maintenance-routing.module';
import { FormComponent } from './form/form.component';
import { MainComponent } from './main/main.component';

import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';


@NgModule({
  declarations: [
    MainComponent,
    FormComponent
  ],
  imports: [
    CommonModule,
    CodeMaintenanceRoutingModule,
    FormsModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
    TranslateModule,
    NgxMaskDirective,
    NgxMaskPipe,
    NgSelectModule,
  ],
})
export class CodeMaintenanceModule { }
