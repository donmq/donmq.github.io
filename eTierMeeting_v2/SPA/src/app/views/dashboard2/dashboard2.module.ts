import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgChartsModule } from 'ng2-charts';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { ButtonsModule } from 'ngx-bootstrap/buttons';

import { Dashboard2RoutingModule } from './dashboard2-routing.module';
import { Dashboard2Component } from './dashboard2/dashboard2.component';

@NgModule({
  imports: [
    FormsModule,
    Dashboard2RoutingModule,
    NgChartsModule,
    BsDropdownModule,
    ButtonsModule.forRoot()
  ],
  declarations: [ Dashboard2Component ]
})
export class Dashboard2Module { }
