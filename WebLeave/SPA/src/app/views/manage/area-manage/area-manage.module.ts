import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AreaManageRoutingModule } from './area-manage-routing.module';
import { AreaMainComponent } from './area-main/area-main.component';
import { AreaAddComponent } from './area-add/area-add.component';
import { AreaEditComponent } from './area-edit/area-edit.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ModalModule } from "ngx-bootstrap/modal";
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
@NgModule({
  declarations: [
    AreaMainComponent,
    AreaAddComponent,
    AreaEditComponent
  ],
  imports: [
    FormsModule,
    CommonModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    NgSelectModule,
    ModalModule.forRoot(),
    TranslateModule,

    AreaManageRoutingModule
  ],
  providers: [
  ],
  schemas: [NO_ERRORS_SCHEMA]
})
export class AreaManageModule { }
