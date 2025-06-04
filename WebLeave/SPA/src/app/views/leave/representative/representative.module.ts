import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RepresentativeRoutingModule } from './representative-routing.module';
import { RepresentativeMainComponent } from './representative-main/representative-main.component';
import { FormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { A2Edatetimepicker } from 'ng2-eonasdan-datetimepicker';
import { TooltipDirective } from '@directives/tooltip.directive';
@NgModule({
  declarations: [
    RepresentativeMainComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    ModalModule.forRoot(),
    NgSelectModule,
    A2Edatetimepicker,
    TranslateModule,
    TooltipDirective,
    RepresentativeRoutingModule,
  ]
})
export class RepresentativeModule { }
