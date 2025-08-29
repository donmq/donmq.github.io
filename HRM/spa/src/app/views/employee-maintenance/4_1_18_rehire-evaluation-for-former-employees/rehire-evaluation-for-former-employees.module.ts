import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './main/main.component';
import { RehireEvaluationforFormerEmployeesRoutingModule } from './rehire-evaluation-for-former-employees-routing.module';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { FormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap/modal';
import { EditComponent } from './edit/edit.component';
import { AddComponent } from './add/add.component';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';

@NgModule({
  imports: [
    CommonModule,
    RehireEvaluationforFormerEmployeesRoutingModule,
    TranslateModule,
    NgSelectModule,
    PaginationModule.forRoot(),
    BsDatepickerModule.forRoot(),
    FormsModule,
    ModalModule.forRoot(),
    TypeaheadModule.forRoot()
  ],
  declarations: [MainComponent,EditComponent, AddComponent]
})
export class RehireEvaluationforFormerEmployeesModule { }
