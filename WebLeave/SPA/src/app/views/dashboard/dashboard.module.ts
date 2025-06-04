import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { CommonsModule } from '../commons/commons.module';
import { TranslateModule } from '@ngx-translate/core';
import { DashboardRoutingModule } from './dashboard-routing.module';
import { DashboardComponent } from './dashboard.component';

@NgModule({
  imports: [
    CommonModule,
    CommonsModule,
    DashboardRoutingModule,
    TranslateModule
  ],
  exports: [],
  declarations: [
    DashboardComponent
  ],
  providers: [],
})
export class DashboardModule { }
