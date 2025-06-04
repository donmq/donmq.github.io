import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BuildingManageRoutingModule } from './building-manage-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgxSpinnerModule } from 'ngx-spinner';
import { ModalModule } from 'ngx-bootstrap/modal';
import { BuildingMainComponent } from './building-main/building-main.component';
import { BuildingAddComponent } from './building-add/building-add.component';
import { BuildingEditComponent } from './building-edit/building-edit.component';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';

@NgModule({
  declarations: [
    BuildingMainComponent,
    BuildingAddComponent,
    BuildingEditComponent
  ],
  imports: [
    FormsModule,
    CommonModule,
    ReactiveFormsModule,
    NgxSpinnerModule,
    ModalModule.forRoot(),
    TranslateModule,
    NgSelectModule,
    BuildingManageRoutingModule
  ]
})
export class BuildingManageModule { }
