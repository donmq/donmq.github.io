import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { PageItemSettingResolver } from '../../../_core/_resolvers/production/T2/CTB/page-item-setting.resolver';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ModalModule } from 'ngx-bootstrap/modal';
import { T2MeetingTimeSettingRoutingModule } from './t2-metting-time-setting-routing.module';
import { MainComponent } from './main/main.component';
import { AddComponent } from './add/add.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TimepickerModule } from 'ngx-bootstrap/timepicker';

@NgModule({
  declarations: [
    MainComponent,
    AddComponent
  ],
  imports: [
    CommonModule,
    T2MeetingTimeSettingRoutingModule,
    FormsModule,
    NgSelectModule,
    PaginationModule.forRoot(),
    ModalModule.forRoot(),
    BsDatepickerModule.forRoot(),
    TimepickerModule.forRoot()
  ],
  providers: [
    PageItemSettingResolver
  ]
})
export class T2MeetingTimeSettingModule { }