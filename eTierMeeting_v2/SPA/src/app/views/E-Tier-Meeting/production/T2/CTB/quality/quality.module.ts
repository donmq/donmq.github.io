import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { QualityRoutingModule } from './quality-routing.module';
import { QualityMainComponent } from './quality-main/quality-main.component';
import { NgChartsModule } from 'ng2-charts';
import { CommonsModule } from '../../../../../commons/commons.module';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    QualityMainComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    CommonsModule,
    QualityRoutingModule,
    NgChartsModule
  ]
})
export class QualityModule { }
