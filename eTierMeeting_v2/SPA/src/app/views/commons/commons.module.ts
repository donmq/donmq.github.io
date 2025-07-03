import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { AnimatedCountComponent } from './animated-count/animated-count.component';
import { NextPreButtonsComponent } from './next-pre-buttons/next-pre-buttons.component';
import { ChartLegendComponent } from './chart-legend/chart-legend.component';
import { CommonComponent } from './common/common';

@NgModule({
  imports: [
    CommonModule
  ],
  exports: [
    NextPreButtonsComponent,
    AnimatedCountComponent,
    ChartLegendComponent
  ],
  declarations: [
    NextPreButtonsComponent,
    AnimatedCountComponent,
    ChartLegendComponent,
    CommonComponent
  ]
})
export class CommonsModule { }
