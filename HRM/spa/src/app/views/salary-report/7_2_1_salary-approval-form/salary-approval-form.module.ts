import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SalaryApprovalFormRoutingModule } from './salary-approval-form-routing.module';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { MainComponent } from './main/main.component';


@NgModule({
  declarations: [
      MainComponent
    ],
  imports: [
    CommonModule,
    SalaryApprovalFormRoutingModule,
    FormsModule,
    TranslateModule,
    NgSelectModule,
    BsDatepickerModule.forRoot(),
  ]
})
export class SalaryApprovalFormModule { }
