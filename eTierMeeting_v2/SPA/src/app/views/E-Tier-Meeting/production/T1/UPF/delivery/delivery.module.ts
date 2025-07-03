import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { DeliveryRoutingModule } from './delivery-routing.module';
import { DeliveryMainComponent } from './delivery-main/delivery-main.component';
import { FormsModule } from '@angular/forms';
import { CommonsModule } from '../../../../../commons/commons.module';
import { NgxSpinnerModule } from 'ngx-spinner';
import { SnotifyModule } from 'ng-alt-snotify';


@NgModule({
  declarations: [
    DeliveryMainComponent
  ],
  imports: [
    CommonModule,
    CommonsModule,
    FormsModule,
    DeliveryRoutingModule,
    NgxSpinnerModule,
    SnotifyModule
  ]
})
export class DeliveryModule { }
