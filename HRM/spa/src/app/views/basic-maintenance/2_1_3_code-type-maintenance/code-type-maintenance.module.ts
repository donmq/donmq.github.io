import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalModule } from 'ngx-bootstrap/modal';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgSelectModule } from '@ng-select/ng-select';
import { CodeTypeMaintenanceRoutingModule } from './code-type-maintenance-routing.module';
import { MainComponent } from './main/main.component';
import { FormLanguageComponent } from './formlanguage/formlanguage.component';
import { FormtypecodeComponent } from './formtypecode/formtypecode.component';
import { TranslateModule } from '@ngx-translate/core';


@NgModule({
  declarations: [
    MainComponent,
    FormLanguageComponent,
    FormtypecodeComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    PaginationModule,
    PaginationModule.forRoot(),
    ModalModule.forRoot(),
    TranslateModule,
    NgSelectModule,
    CodeTypeMaintenanceRoutingModule
  ]
})
export class CodeTypeMaintenanceModule { }
