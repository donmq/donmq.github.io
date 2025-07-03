import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EfficiencyRoutingModule } from './efficiency-routing.module';
import { EfficiencymainComponent } from './efficiencymain/efficiencymain.component';
import { CarouselModule } from 'ngx-bootstrap/carousel';
import { FormsModule } from '@angular/forms';
import { ButtonsModule } from 'ngx-bootstrap/buttons';

import { FusionChartsModule } from 'angular-fusioncharts';

// Import FusionCharts library and chart modules
import * as FusionCharts from 'fusioncharts';
import * as Charts from 'fusioncharts/fusioncharts.charts';
import * as FusionTheme from 'fusioncharts/themes/fusioncharts.theme.fusion';
import * as Candy from 'fusioncharts/themes/fusioncharts.theme.candy';

// Pass the fusioncharts library and chart modules
FusionChartsModule.fcRoot(FusionCharts, Charts, Candy, FusionTheme);
@NgModule({
  declarations: [
    EfficiencymainComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    FusionChartsModule,
    CarouselModule.forRoot(),
    ButtonsModule.forRoot(),
    EfficiencyRoutingModule
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class EfficiencyModule { }
