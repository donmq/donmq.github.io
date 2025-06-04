import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AllowLeaveSundayRoutingModule } from './allow-leave-sunday-routing.module';
import { FormComponent } from './form/form.component';
import { MainComponent } from './main/main.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonsModule } from '../../commons/commons.module';
import { A2Edatetimepicker } from 'ng2-eonasdan-datetimepicker';
import { SeaHRRoutingModule } from '../seahr-routing.module';
import { TranslateModule } from '@ngx-translate/core';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TooltipDirective } from '@directives/tooltip.directive';
import { NgxSpinnerModule } from 'ngx-spinner';
import { NgSelectModule } from '@ng-select/ng-select';


@NgModule({
  declarations: [
    MainComponent,
    FormComponent
  ],
  imports: [
    CommonModule,
    AllowLeaveSundayRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    CommonsModule,
    HttpClientModule,
    A2Edatetimepicker,
    SeaHRRoutingModule,
    TranslateModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
    TooltipDirective,
    NgxSpinnerModule,
    NgSelectModule,
  ]
})
export class AllowLeaveSundayModule { }
