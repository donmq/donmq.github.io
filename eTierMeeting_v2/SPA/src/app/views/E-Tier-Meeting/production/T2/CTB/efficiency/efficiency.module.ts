import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { SnotifyModule } from 'ng-alt-snotify';
import { NgChartsModule } from 'ng2-charts';
import { NgxSpinnerModule } from 'ngx-spinner';
import { CommonsModule } from '../../../../../commons/commons.module';
import { EfficiencyRoutingModule } from './efficiency-routing.module';
import { EfficiencyMainComponent } from './efficiencymain/efficiencymain.component';
import { Efficiencymain_page2Component } from './efficiencymain_page2/efficiencymain_page2/efficiencymain_page2.component';


@NgModule({
    imports: [
        CommonModule,
        CommonsModule,
        EfficiencyRoutingModule,
        NgChartsModule,
        FormsModule,
        NgxSpinnerModule,
        SnotifyModule
    ],
    exports: [],
    declarations: [EfficiencyMainComponent,Efficiencymain_page2Component],
    providers: [],
})
export class EfficiencyModule { }
