import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SafetyRoutingModule } from './safety-routing.module';
import { SafetymainComponent } from './safetymain/safetymain.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { CommonsModule } from '../../../../../commons/commons.module';


@NgModule({
  declarations: [
    SafetymainComponent
  ],
  imports: [
    CommonModule,
    CommonsModule,
    SafetyRoutingModule,
    ModalModule.forRoot()
  ]
})
export class SafetyModule { }
