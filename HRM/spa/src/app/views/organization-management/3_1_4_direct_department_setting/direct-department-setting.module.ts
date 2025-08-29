import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DirectDepartmentSettingRoutingModule } from './direct-department-setting-routing.module';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule } from '@angular/forms';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  imports: [
    CommonModule,
    DirectDepartmentSettingRoutingModule,
    PaginationModule.forRoot(),
    FormsModule,
    ModalModule.forRoot(),
    TranslateModule,
    NgSelectModule
  ],
  declarations: [
    MainComponent,
    FormComponent
  ]
})
export class DirectDepartmentSettingModule { }
