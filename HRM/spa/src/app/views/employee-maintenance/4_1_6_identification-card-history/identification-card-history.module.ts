import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { IdentificationCardHistoryRoutingModule } from './identification-card-history-routing.module';
import { MainComponent } from './main/main.component';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { ModalComponent } from './modal/modal.component';

@NgModule({
  imports: [
    CommonModule,
    IdentificationCardHistoryRoutingModule,
    TranslateModule,
    NgSelectModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
    FormsModule,
    ModalModule.forRoot(),
    TypeaheadModule.forRoot()
  ],
  declarations: [MainComponent, ModalComponent]
})
export class IdentificationCardHistoryModule { }
