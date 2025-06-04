import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { CommonsModule } from '../../commons/commons.module';
import { ReportShowRoutingModule } from './reportshow-routing.module';
import { ReportshowComponent } from './reportshow/report-show.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PopupReportGridDetailComponent } from './popup-report-grid-detail/popupReportGridDetail.component';
import { PopupReportDateDetailComponent } from './popup-report-date-detail/popup-report-date-detail.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  imports: [
    CommonModule,
    CommonsModule,
    TranslateModule,
    ReportShowRoutingModule,
    BsDatepickerModule.forRoot(),
    FormsModule
  ],
  exports: [],
  declarations: [
    ReportshowComponent,
    PopupReportGridDetailComponent,
    PopupReportDateDetailComponent,
  ],
  providers: [],
})
export class ReportShowModule {}
