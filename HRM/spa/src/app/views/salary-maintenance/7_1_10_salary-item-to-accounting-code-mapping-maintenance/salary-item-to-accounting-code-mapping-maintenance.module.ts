import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SalaryItemtoAccountingCodeMappingMaintenanceRoutingModule } from './salary-item-to-accounting-code-mapping-maintenance-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TranslateModule } from '@ngx-translate/core';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';

@NgModule({
  declarations: [
    MainComponent,
    FormComponent
  ],
  imports: [
    CommonModule,
    SalaryItemtoAccountingCodeMappingMaintenanceRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
    PaginationModule.forRoot(),
    ModalModule.forRoot(),
    TranslateModule,
  ]
})
export class SalaryItemtoAccountingCodeMappingMaintenanceModule { }
