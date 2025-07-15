import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { ChartsModule } from 'ng2-charts';
import { HistoryMoveComponent } from './history-move/history-move.component';
import { MainRoutingModule } from './main-routing.module';
import { MoveComponent } from './move/move.component';
import { HighchartsChartModule } from 'highcharts-angular';
import { NgSelect2Module } from 'ng-select2';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { ErrorComponent } from './error/error.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TranslateModule } from '@ngx-translate/core';
import { ScanModalComponent } from './scan-modal/scan-modal.component';
import { MoveMachineBlockedComponent } from './move-machine-blocked/move-machine-blocked.component';
import { defineLocale, idLocale, viLocale, zhCnLocale } from 'ngx-bootstrap/chronos';
import { ReportMachineListComponent } from './report-machine/report-machine-list/report-machine-list.component';
import { ReportMachinePieChartComponent } from './report-machine/report-machine-pie-chart/report-machine-pie-chart.component';
import { ZXingScannerModule } from '@zxing/ngx-scanner';
import { UploadDataHpA15Component } from './upload-data-hp-a15/upload-data-hp-a15.component';
import { Error403Component } from './error403/error403.component';
defineLocale('vi', viLocale);
defineLocale('zh', zhCnLocale);
defineLocale('id', idLocale);

@NgModule({
  imports: [
    CommonModule,
    MainRoutingModule,
    ChartsModule,
    HighchartsChartModule,
    NgSelect2Module,
    NgxSpinnerModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
    ModalModule.forRoot(),
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
    TranslateModule,
    ZXingScannerModule
  ],
  declarations: [
    MoveComponent,
    HistoryMoveComponent,
    ReportMachineListComponent,
    ReportMachinePieChartComponent,
    ErrorComponent,
    Error403Component,
    ScanModalComponent,
    MoveMachineBlockedComponent,
    UploadDataHpA15Component
  ],
  exports: [ScanModalComponent]
})
export class MainModule { }
