import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { PositionManageRoutingModule } from './position-manage-routing.module';
import { PositionMainComponent } from './position-main/position-main.component';
import { PositionAddComponent } from './position-add/position-add.component';
import { PositionEditComponent } from './position-edit/position-edit.component';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TranslateModule } from '@ngx-translate/core';


@NgModule({
  declarations: [
    PositionMainComponent,
    PositionAddComponent,
    PositionEditComponent
  ],
  imports: [
    CommonModule,
    PositionManageRoutingModule,
    NgxSpinnerModule,
    FormsModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
    TranslateModule,
    PaginationModule.forRoot(),
  ]
})
export class PositionManageModule { }
