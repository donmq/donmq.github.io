import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ReportInventoryBuildingComponent } from './report-inventory-building/report-inventory-building.component';
import { ReportInventoryLineComponent } from './report-inventory-line/report-inventory-line.component';
import { ReportInventoryRoutingModule } from './report-inventory-routing.module';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        ReportInventoryRoutingModule,
        NgxSpinnerModule,
        BsDatepickerModule.forRoot(),
        TranslateModule
    ],
    declarations: [
        ReportInventoryBuildingComponent,
        ReportInventoryLineComponent
    ]
})
export class ReportInventoryModule { }
