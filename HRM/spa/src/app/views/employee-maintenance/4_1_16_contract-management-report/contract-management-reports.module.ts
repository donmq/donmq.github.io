import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ContractManagementReportsRoutingModule } from './contract-management-reports-routing.module';
import { MainComponent } from './main/main.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TranslateModule } from '@ngx-translate/core';
import { NgxMaskDirective, NgxMaskPipe, provideNgxMask } from 'ngx-mask';


@NgModule({
  declarations: [
    MainComponent
  ],
  imports: [
    CommonModule,
    ContractManagementReportsRoutingModule,
    FormsModule,
    PaginationModule.forRoot(),
    NgSelectModule,
    BsDatepickerModule.forRoot(),
    TranslateModule,
    NgxMaskDirective, NgxMaskPipe,
    ReactiveFormsModule
  ],
  providers: [
    provideNgxMask(),
  ]
})
export class ContractManagementReportsModule { }
