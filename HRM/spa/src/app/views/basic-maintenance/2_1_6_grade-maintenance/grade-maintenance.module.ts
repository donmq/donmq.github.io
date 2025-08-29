import { TranslateModule } from '@ngx-translate/core';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ModalModule } from 'ngx-bootstrap/modal';
import { GradeMaintenanceRoutingModule } from './grade-maintenance-routing.module';
import { FormComponent } from './form/form.component';
import { MainComponent } from './main/main.component';
@NgModule({
  declarations: [
    FormComponent,
    MainComponent
  ],
  imports: [
    CommonModule,
    GradeMaintenanceRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
    PaginationModule.forRoot(),
    ModalModule.forRoot(),
    TranslateModule,
  ],
  providers: [
  ]
})
export class GradeMaintenanceModule { }
