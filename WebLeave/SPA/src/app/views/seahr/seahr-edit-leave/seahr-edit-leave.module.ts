import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { SeahrEditLeaveRoutingModule } from './seahr-edit-leave-routing.module';
import { SeahrEditLeaveComponent } from './seahr-edit-leave-main/seahr-edit-leave.component';
import { SeahrEditLeaveEmpDetailComponent } from './seahr-edit-leave-emp-detail/seahr-edit-leave-emp-detail.component';
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TooltipDirective } from '@directives/tooltip.directive';

@NgModule({
  declarations: [SeahrEditLeaveComponent, SeahrEditLeaveEmpDetailComponent],
  imports: [
    CommonModule,
    SeahrEditLeaveRoutingModule,
    TranslateModule,
    FormsModule,
    TooltipDirective,
    PaginationModule.forRoot()
  ]
})
export class SeahrEditLeaveModule { }
