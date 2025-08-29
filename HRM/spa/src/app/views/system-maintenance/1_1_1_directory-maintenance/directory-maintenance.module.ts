import { TranslateModule } from '@ngx-translate/core';
import { FormComponent } from './form/form.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './main/main.component';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { DirectoryMaintenanceRoutingModule } from './directory-maintenance-routing.module';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';

@NgModule({
  declarations: [
    MainComponent,
    FormComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    PaginationModule.forRoot(),
    NgSelectModule,
    NgxMaskDirective,
    NgxMaskPipe,
    TranslateModule,
    DirectoryMaintenanceRoutingModule
  ]
})
export class DirectoryMaintenanceModule { }
