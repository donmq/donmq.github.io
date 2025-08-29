import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule } from '@angular/forms';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ExitEmployeesBlacklistRoutingModule } from './exit-employees-blacklist-routing.module';
import { MainComponent } from './main/main.component';
import { ModalComponent } from './modal/modal.component';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';


@NgModule({
  declarations: [
    MainComponent,
    ModalComponent
  ],
  imports: [
    CommonModule,
    ExitEmployeesBlacklistRoutingModule,
    ModalModule.forRoot(),
    PaginationModule.forRoot(),
    BsDatepickerModule,
    FormsModule,
    TranslateModule,
    NgSelectModule,
    TypeaheadModule.forRoot(),
  ]
})
export class ExitEmployeesBlacklistModule { }
