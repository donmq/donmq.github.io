import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CompanyManageRoutingModule } from './company-manage-routing.module';
import { CompanyMainComponent } from './company-main/company-main.component';
import { CompanyAddComponent } from './company-add/company-add.component';
import { CompanyEditComponent } from './company-edit/company-edit.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModalModule } from "ngx-bootstrap/modal";
import { TranslateModule } from '@ngx-translate/core';


@NgModule({
  declarations: [
    CompanyMainComponent,
    CompanyAddComponent,
    CompanyEditComponent
  ],
  imports: [
    CommonModule,
    CompanyManageRoutingModule,
    NgxSpinnerModule,
    FormsModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
    TranslateModule,
  ]
})
export class CompanyManageModule { }
