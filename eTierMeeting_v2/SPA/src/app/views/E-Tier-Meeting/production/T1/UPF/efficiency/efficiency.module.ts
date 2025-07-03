import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EfficiencyRoutingModule } from './efficiency-routing.module';
import { EfficiencymainComponent } from './efficiencymain/efficiencymain.component';
import { CommonsModule } from '../../../../../commons/commons.module';

@NgModule({
  declarations: [
    EfficiencymainComponent
  ],
  imports: [
    CommonModule,
    CommonsModule,
    EfficiencyRoutingModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class EfficiencyModule { }
