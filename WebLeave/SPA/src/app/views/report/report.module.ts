import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ReportRoutingModule } from './report-routing.module';
import { ReportMainComponent } from './report-main/report-main.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { NgxSpinnerModule } from 'ngx-spinner';
import { OrganizationChartModule } from 'primeng/organizationchart';
import { PanelModule } from 'primeng/panel';

@NgModule({
  declarations: [ReportMainComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    TranslateModule,
    OrganizationChartModule,
    PanelModule,
    ReportRoutingModule,
  ],
})
export class ReportModule {}
