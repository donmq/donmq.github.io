import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { SelectlineRoutingModule } from './selectline-routing.module';
import { SelectlinemainComponent } from './selectlinemain/selectlinemain.component';


@NgModule({
  declarations: [
    SelectlinemainComponent
  ],
  imports: [
    CommonModule,
    SelectlineRoutingModule
  ]
})
export class SelectlineModule { }
