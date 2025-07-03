import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { MeetingAuditReportRoutingModule } from './meeting-audit-report-routing.module';
import { MainComponent } from './main/main.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { HttpClientModule } from '@angular/common/http';
import { TooltipModule } from 'ngx-bootstrap/tooltip';


@NgModule({
  declarations: [MainComponent],
  imports: [
    CommonModule,
    MeetingAuditReportRoutingModule,
    FormsModule,
    PaginationModule,
    HttpClientModule,
    NgxSpinnerModule,
    TooltipModule.forRoot(),
    BsDatepickerModule.forRoot(),
  ]
})
export class MeetingAuditReportModule { }
