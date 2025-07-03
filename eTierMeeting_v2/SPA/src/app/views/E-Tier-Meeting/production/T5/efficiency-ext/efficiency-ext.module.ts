import { CUSTOM_ELEMENTS_SCHEMA, NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { EfficiencyExtRoutingModule } from './efficiency-ext-routing.module';
import { EfficiencymainExtComponent } from './efficiencymain-ext/efficiencymain-ext.component';
import { FormsModule } from '@angular/forms';
import { FusionChartsModule } from 'angular-fusioncharts';
import { ButtonsModule } from 'ngx-bootstrap/buttons';
import { CarouselModule } from 'ngx-bootstrap/carousel';

// Import FusionCharts library and chart modules
import * as FusionCharts from 'fusioncharts';
import * as Charts from 'fusioncharts/fusioncharts.charts';
import * as FusionTheme from 'fusioncharts/themes/fusioncharts.theme.fusion';
import * as Candy from 'fusioncharts/themes/fusioncharts.theme.candy';

// Pass the fusioncharts library and chart modules
FusionChartsModule.fcRoot(FusionCharts, Charts, Candy, FusionTheme);
@NgModule({
  declarations: [
    EfficiencymainExtComponent
  ],
  imports: [
    CommonModule,
    EfficiencyExtRoutingModule,
    FormsModule,
    FusionChartsModule,
    CarouselModule.forRoot(),
    ButtonsModule.forRoot(),
  ],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class EfficiencyExtModule { }
