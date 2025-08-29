import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ContractTypeSetupRoutingModule } from './contract-type-setup-routing.module';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';


@NgModule({
  declarations: [
    MainComponent,
    FormComponent
  ],
  imports: [
    CommonModule,
    ContractTypeSetupRoutingModule,
    ModalModule.forRoot(),
    PaginationModule.forRoot(),
    BsDatepickerModule,
    FormsModule,
    TranslateModule,
    NgSelectModule
  ]
})
export class ContractTypeSetupModule { }
