import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { QualityRoutingModule } from './quality-routing.module';
import { QualitymainComponent } from './qualitymain/qualitymain.component';
import { NgChartsModule } from 'ng2-charts';
import { CommonsModule } from '../../../../../commons/commons.module';


@NgModule({
  declarations: [
    QualitymainComponent
  ],
  imports: [
    CommonModule,
    CommonsModule,
    QualityRoutingModule,
    NgChartsModule
  ]
})
export class QualityModule { }
