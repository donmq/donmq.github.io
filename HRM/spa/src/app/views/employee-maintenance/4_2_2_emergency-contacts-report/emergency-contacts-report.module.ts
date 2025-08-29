import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EmergencyContactsSheetReportRoutingModule } from './emergency-contacts-report-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { MainComponent } from './main/main.component';


@NgModule({
  declarations: [
    MainComponent
  ],
  imports: [
    CommonModule,
    EmergencyContactsSheetReportRoutingModule,
    FormsModule,
    TranslateModule,
    NgSelectModule,
    ReactiveFormsModule
  ]
})
export class EmergencyContactsSheetReportModule { }
