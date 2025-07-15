import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelect2Module } from 'ng-select2';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxSpinnerModule } from 'ngx-spinner';
import { MainModule } from '../main.module';
import { HistoryInventoryHomeComponent } from './history-inventory-home/history-inventory-home.component';
import { HistoryInventoryRoutingModule } from './history-inventory-routing.module';
import { HistoryInventoryViewComponent } from './history-inventory-view/history-inventory-view.component';

@NgModule({
    imports: [
        CommonModule,
        HistoryInventoryRoutingModule,
        NgSelect2Module,
        NgxSpinnerModule,
        FormsModule,
        ReactiveFormsModule,
        NgSelectModule,
        MainModule,
        TranslateModule,
        BsDatepickerModule.forRoot(),
        PaginationModule.forRoot(),
    ],
    declarations: [
        HistoryInventoryHomeComponent,
        HistoryInventoryViewComponent,
    ]
})
export class HistoryInventoryModule { }