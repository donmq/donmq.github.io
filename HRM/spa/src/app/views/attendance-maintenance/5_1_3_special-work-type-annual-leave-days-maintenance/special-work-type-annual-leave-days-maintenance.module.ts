import { TranslateModule } from '@ngx-translate/core';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgSelectModule } from '@ng-select/ng-select';
import { ModalModule } from 'ngx-bootstrap/modal';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component'
import { SpecialWorkTypeAnnualLeaveDaysMaintenanceRoutingModule } from './special-work-type-annual-leave-days-maintenance-routing.module';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';
@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    PaginationModule.forRoot(),
    SpecialWorkTypeAnnualLeaveDaysMaintenanceRoutingModule,
    ModalModule.forRoot(),
    NgxMaskDirective,
    NgxMaskPipe,
    TranslateModule,
    NgSelectModule
  ],
  declarations: [
    MainComponent, FormComponent]
})
export class SpecialWorkTypeAnnualLeaveDaysMaintenanceModule { }
