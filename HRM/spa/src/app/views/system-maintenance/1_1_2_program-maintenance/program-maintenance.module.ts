import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProgramMaintenanceRoutingModule } from './program-maintenance-routing.module';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule } from '@angular/forms';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';

@NgModule({
  imports: [
    CommonModule,
    ProgramMaintenanceRoutingModule,
    PaginationModule.forRoot(),
    FormsModule,
    ModalModule.forRoot(),
    NgxMaskDirective,
    NgxMaskPipe,
    TranslateModule,
    NgSelectModule
  ],
  declarations: [
    MainComponent,
    FormComponent
  ]
})
export class ProgramMaintenanceModule { }
