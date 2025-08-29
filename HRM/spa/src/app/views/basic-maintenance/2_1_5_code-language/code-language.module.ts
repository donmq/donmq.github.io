import { TranslateModule } from '@ngx-translate/core';
import { FormComponent } from './form/form.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MainComponent } from './main/main.component';
import { NgModule } from '@angular/core';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { CodeLanguageRoutingModule } from './code-language-routing.module';
import { NgSelectModule } from '@ng-select/ng-select';

@NgModule({
  declarations: [
    MainComponent,
    FormComponent
  ],
  imports: [
    FormsModule,
    CommonModule,
    NgSelectModule,
    ModalModule.forRoot(),
    PaginationModule.forRoot(),
    TranslateModule,
    CodeLanguageRoutingModule
  ]
})
export class CodeLanguageModule { }
