import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ModelPreparationRoutingModule } from './model-preparation-routing.module';
import { CommonsModule } from '../../../../../commons/commons.module';
import { ModalModule } from 'ngx-bootstrap/modal';
import { ModelpreparationMainComponent } from './modelpreparationmain/modelpreparationmain.component';


@NgModule({
  declarations: [
    ModelpreparationMainComponent
  ],
  imports: [
    CommonModule,
    ModelPreparationRoutingModule,
    CommonsModule,
    ModalModule.forRoot()
  ]
})
export class ModelPreparationModule { }
