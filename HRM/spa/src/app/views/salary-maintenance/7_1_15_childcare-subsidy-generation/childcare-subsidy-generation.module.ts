import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ChildcareSubsidyGenerationRoutingModule } from './childcare-subsidy-generation-routing.module';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { Tab2Component } from './tab-2/tab-2.component';
import { Tab1Component } from './tab-1/tab-1.component';
import { MainComponent } from './main/main.component';
import { SharedModule } from '@views/_shared/shared.module';


@NgModule({
  declarations: [MainComponent, Tab1Component, Tab2Component],
  imports: [
    CommonModule,
    ChildcareSubsidyGenerationRoutingModule,
    FormsModule,
    PaginationModule.forRoot(),
    TranslateModule,
    NgSelectModule,
    BsDatepickerModule.forRoot(),
    SharedModule
  ]
})
export class ChildcareSubsidyGenerationModule { }
