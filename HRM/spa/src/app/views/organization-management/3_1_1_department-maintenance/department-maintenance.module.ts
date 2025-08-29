import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DepartmentMaintenanceRoutingModule } from './department-maintenance-routing.module';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgSelectModule } from '@ng-select/ng-select';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { FormLanguageComponent } from './form-language/form-language.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgxMaskDirective, NgxMaskPipe, provideNgxMask } from 'ngx-mask';
import { TranslateModule } from '@ngx-translate/core';


@NgModule({
  declarations: [MainComponent, FormComponent, FormLanguageComponent],
  imports: [
  CommonModule,
    DepartmentMaintenanceRoutingModule,
    FormsModule,
    PaginationModule.forRoot(),
    NgSelectModule,
    BsDatepickerModule.forRoot(),
    ModalModule.forRoot(),
    TranslateModule,
    NgSelectModule,
    NgxMaskDirective, NgxMaskPipe
  ],
  providers: [
    provideNgxMask(),
  ]
})
export class DepartmentMaintenanceModule { }
