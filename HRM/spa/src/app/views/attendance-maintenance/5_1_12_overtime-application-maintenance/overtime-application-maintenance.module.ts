import { ModalComponent } from './modal/modal.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { MainComponent } from './main/main.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PipesModule } from 'src/app/_core/pipes/pipes.module';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { OvertimeApplicationMaintenanceRoutingModule } from './overtime-application-maintenance-routing.module';
import { TimepickerModule } from 'ngx-bootstrap/timepicker';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PopoverModule } from 'ngx-bootstrap/popover';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';
@NgModule({
  declarations: [
    MainComponent,
    ModalComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    PipesModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
    ModalModule.forRoot(),
    TimepickerModule.forRoot(),
    TypeaheadModule.forRoot(),
    PopoverModule.forRoot(),
    TranslateModule,
    NgSelectModule,
    NgxMaskDirective,
    NgxMaskPipe,
    OvertimeApplicationMaintenanceRoutingModule
  ]
})
export class OvertimeApplicationMaintenanceModule { }
