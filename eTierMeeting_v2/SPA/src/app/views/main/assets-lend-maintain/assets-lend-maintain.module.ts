import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AssetsLendMaintainRoutingModule } from './assets-lend-maintain-routing.module';
import { NgxSpinnerModule } from 'ngx-spinner';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { MainModule } from '../main.module';
import { TranslateModule } from '@ngx-translate/core';
import { MainComponent } from './main/main.component';
import { EditComponent } from './edit/edit.component';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { PaginationModule } from 'ngx-bootstrap/pagination';


@NgModule({
  declarations: [
    MainComponent,
    EditComponent
  ],
  imports: [
    CommonModule,
    AssetsLendMaintainRoutingModule,
    NgxSpinnerModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
    MainModule,
    TranslateModule,
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
  ]
})
export class AssetsLendMaintainModule { }
