import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './main/main.component';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgSelectModule } from '@ng-select/ng-select';
import { LeaveSurrogateRoutingModule } from './leave-surrogate-routing.module';

@NgModule({
  imports: [
    CommonModule,
    LeaveSurrogateRoutingModule,
    TranslateModule,
    FormsModule,
    PaginationModule.forRoot(),
    ModalModule.forRoot(),
    NgSelectModule
  ],
  declarations: [
    MainComponent
  ]
})
export class LeaveSurrogateModule { }
