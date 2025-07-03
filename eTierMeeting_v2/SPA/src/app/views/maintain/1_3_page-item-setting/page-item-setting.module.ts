import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PageItemSettingRoutingModule } from './page-item-setting-routing.module';
import { MainComponent } from './main/main.component';
import { AddComponent } from './add/add.component';
import { EditComponent } from './edit/edit.component';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { PageItemSettingResolver } from '../../../_core/_resolvers/production/T2/CTB/page-item-setting.resolver';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ModalModule } from 'ngx-bootstrap/modal';

@NgModule({
  declarations: [
    MainComponent,
    AddComponent,
    EditComponent
  ],
  imports: [
    CommonModule,
    PageItemSettingRoutingModule,
    FormsModule,
    NgSelectModule,
    PaginationModule.forRoot(),
    ModalModule.forRoot()
  ],
  providers: [
    PageItemSettingResolver
  ]
})
export class PageItemSettingModule { }