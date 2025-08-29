import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CardSwipingDataFormatSettingRoutingModule } from './card-swiping-data-format-setting-routing.module';
import { MainComponent } from './main/main.component';
import { ModalComponent } from './modal/modal.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { ModalModule } from 'ngx-bootstrap/modal';

@NgModule({
  declarations: [MainComponent, ModalComponent],
  imports: [
    CommonModule,
    CardSwipingDataFormatSettingRoutingModule,
    FormsModule,
    ModalModule.forRoot(),
    TranslateModule,
    NgSelectModule,
    PaginationModule.forRoot(),
  ]
})
export class CardSwipingDataFormatSettingModule { }
