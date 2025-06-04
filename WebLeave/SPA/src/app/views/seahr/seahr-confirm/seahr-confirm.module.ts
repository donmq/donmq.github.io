import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SeahrConfirmRoutingModule } from './seahr-confirm-routing.module';
import { TranslateModule } from '@ngx-translate/core';
import { SeahrConfirmComponent } from './seahr-confirm/seahr-confirm.component';
import { SeahrConfirmEmpDetailComponent } from './seahr-confirm-emp-detail/seahr-confirm-emp-detail.component';
import { FormsModule } from '@angular/forms';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { NgSelectModule } from '@ng-select/ng-select';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TooltipDirective } from '@directives/tooltip.directive';

@NgModule({
  declarations: [
    SeahrConfirmComponent,
    SeahrConfirmEmpDetailComponent
  ],
  imports: [
    CommonModule,
    SeahrConfirmRoutingModule,
    TranslateModule,
    FormsModule,
    NgSelectModule,
    BsDatepickerModule.forRoot(),
    PaginationModule.forRoot(),
    TooltipDirective
  ]
})
export class SeahrConfirmModule { }
