import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { OrganizationChartRoutingModule } from './organization-chart-routing.module';
import { MainComponent } from './main/main.component';
import { ModalComponent } from './modal/modal.component';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TreeviewModule } from '@ash-mezdo/ngx-treeview';
import { NgSelectModule } from '@ng-select/ng-select';
import { NgxOrgChartModule } from '@tots/ngx-org-chart';
import { TranslateModule } from '@ngx-translate/core';
import { NgxPanZoomModule, PanZoomComponent } from 'ngx-panzoom';


@NgModule({
  declarations: [
    MainComponent, ModalComponent
  ],
  imports: [
    CommonModule,
    OrganizationChartRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    NgxOrgChartModule,
    PaginationModule.forRoot(),
    ModalModule.forRoot(),
    TreeviewModule.forRoot(),
    TranslateModule,
    NgSelectModule,
    NgxPanZoomModule,
    PanZoomComponent
  ]
})
export class OrganizationChartModule { }
