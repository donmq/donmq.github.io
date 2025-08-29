import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';

import { MonthlySalaryMasterFileBackupQueryRoutingModule } from './monthly-salary-master-file-backup-query-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';

import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';
import { SharedModule } from '@views/_shared/shared.module';

@NgModule({
  declarations: [
    MainComponent,
    FormComponent
  ],
  imports: [
    CommonModule,
    MonthlySalaryMasterFileBackupQueryRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    PaginationModule.forRoot(),
    TranslateModule,
    NgSelectModule,
    BsDatepickerModule.forRoot(),
    NgxMaskDirective,
    NgxMaskPipe,
    SharedModule
  ]
})
export class MonthlySalaryMasterFileBackupQueryModule { }
