import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { KaizenRoutingModule } from './kaizen-routing.module';
import { KaizenmainComponent } from './kaizenmain/kaizenmain.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { CommonsModule } from '../../../../../commons/commons.module';


@NgModule({
  declarations: [
    KaizenmainComponent
  ],
  imports: [
    CommonModule,
    CommonsModule,
    KaizenRoutingModule,
    ModalModule.forRoot()
  ]
})
export class KaizenModule { }
