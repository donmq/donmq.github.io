import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { KaizenRoutingModule } from './kaizen-routing.module';
import { KaizenmainComponent } from './kaizenmain/kaizenmain.component';
import { CommonsModule } from '../../../../../commons/commons.module';


@NgModule({
  declarations: [
    KaizenmainComponent
  ],
  imports: [
    CommonModule,
    KaizenRoutingModule,
    CommonsModule,
  ]
})
export class KaizenModule { }
