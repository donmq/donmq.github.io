import { TranslateModule } from '@ngx-translate/core';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgSelectModule } from '@ng-select/ng-select';
import { ModalModule } from 'ngx-bootstrap/modal';
import { FormsModule } from '@angular/forms';
import { SystemLanguageSettingRoutingModule } from './system-language-setting-routing.module'
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { MainComponent } from './main/main.component';
import { FormComponent } from './form/form.component'
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    PaginationModule.forRoot(),
    ModalModule.forRoot(),
    TranslateModule,
    NgSelectModule,
    NgxMaskDirective,
    NgxMaskPipe,
    SystemLanguageSettingRoutingModule
  ],
  declarations: [
    MainComponent,
    FormComponent
  ]
})
export class SystemLanguageSettingModule { }
