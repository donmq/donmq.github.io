import { FormComponent } from './form/form.component';
import { ModalComponent } from './modal/modal.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { RoleSettingRoutingModule } from './role-setting-routing.module';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';

import { MainComponent } from './main/main.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { NgSelectModule } from '@ng-select/ng-select';
import { TreeviewModule } from '@ash-mezdo/ngx-treeview';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [MainComponent, FormComponent, ModalComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    PaginationModule.forRoot(),
    ModalModule.forRoot(),
    TreeviewModule.forRoot(),
    TranslateModule,
    NgSelectModule,
    RoleSettingRoutingModule
  ]
})
export class RoleSettingModule { }
