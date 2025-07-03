import { NgModule, NO_ERRORS_SCHEMA } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { ModalModule } from 'ngx-bootstrap/modal';
import { AlertModule } from 'ngx-bootstrap/alert';
import { UploadT1safetyRoutingModule } from './upload-t1safety-routing.module';
import { UplaodT1safetyComponent } from './uplaod-t1safety/uplaod-t1safety.component';
import { UploadT1safetyAddComponent } from './upload-t1safety-add/upload-t1safety-add.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { HttpClientModule } from '@angular/common/http';
import { NgxSpinnerModule } from 'ngx-spinner';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { BatchDeleteComponent } from './batch-delete/batch-delete.component';
@NgModule({
  declarations: [
    UplaodT1safetyComponent,
    UploadT1safetyAddComponent,
    BatchDeleteComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
    PaginationModule,
    HttpClientModule,
    NgxSpinnerModule,
    TooltipModule.forRoot(),
    BsDatepickerModule.forRoot(),
    ModalModule.forRoot(),
    AlertModule.forRoot(),
    UploadT1safetyRoutingModule
  ],
  schemas: [
    NO_ERRORS_SCHEMA
]
})
export class UploadT1safetyModule { }
