import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelect2Module } from 'ng-select2';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxSpinnerModule } from 'ngx-spinner';
import { MainModule } from '../main.module';
import { HistoryCheckMachineHomeComponent } from './history-check-machine-home/history-check-machine-home.component';
import { HistoryCheckMachineRoutingModule } from './history-check-machine-routing.module';
import { HistoryCheckMachineViewComponent } from './history-check-machine-view/history-check-machine-view.component';

@NgModule({
    imports: [
        CommonModule,
        HistoryCheckMachineRoutingModule,
        NgSelect2Module,
        NgxSpinnerModule,
        FormsModule,
        NgSelectModule,
        MainModule,
        TranslateModule,
        BsDatepickerModule.forRoot(),
        PaginationModule.forRoot(),
    ],
    declarations: [
        HistoryCheckMachineHomeComponent,
        HistoryCheckMachineViewComponent,
    ]
})
export class  HistoryCheckMachineModule { }