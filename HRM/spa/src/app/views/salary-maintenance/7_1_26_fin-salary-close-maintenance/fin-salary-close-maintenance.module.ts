import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FinSalaryCloseMaintenanceRoutingModule } from './fin-salary-close-maintenance-routing.module';
import { MainComponent } from './main/main.component';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { SharedModule } from '@views/_shared/shared.module';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { DragScrollComponent } from 'ngx-drag-scroll';
import { FormComponent } from './form/form.component';


@NgModule({
  declarations: [
    MainComponent,
    FormComponent
  ],
  imports: [
    CommonModule,
    FinSalaryCloseMaintenanceRoutingModule,
    FormsModule,
    TranslateModule,
    NgSelectModule,
    BsDatepickerModule.forRoot(),
    SharedModule,
    PaginationModule.forRoot(),
    NgxMaskDirective, NgxMaskPipe,
    CollapseModule.forRoot(),
    TypeaheadModule.forRoot(),
    DragScrollComponent
  ]
})
export class FinSalaryCloseMaintenanceModule { }
