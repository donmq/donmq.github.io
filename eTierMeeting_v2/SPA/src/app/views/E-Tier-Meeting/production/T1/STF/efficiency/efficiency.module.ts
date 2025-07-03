import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EfficiencyRoutingModule } from './efficiency-routing.module';
import { EfficiencymainComponent } from './efficiencymain/efficiencymain.component';
import { CommonsModule } from '../../../../../commons/commons.module';
import { NgChartsModule } from 'ng2-charts';


@NgModule({
  declarations: [
    EfficiencymainComponent
  ],
  imports: [
    CommonModule,
    CommonsModule,
    EfficiencyRoutingModule,
    NgChartsModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class EfficiencyModule { }
