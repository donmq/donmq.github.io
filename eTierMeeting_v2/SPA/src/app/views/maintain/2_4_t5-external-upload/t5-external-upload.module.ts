import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { T5ExternalUploadRoutingModule } from './t5-external-upload-routing.module';
import { MainComponent } from './main/main.component';

@NgModule({
  declarations: [
    MainComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    T5ExternalUploadRoutingModule,
    PaginationModule.forRoot()
  ],
  providers: []
})
export class T5ExternalUploadModule { }