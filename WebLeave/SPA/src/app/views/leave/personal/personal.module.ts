
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PersonalRoutingModule } from './personal-routing.module';
import { PersonalMainComponent } from './personal-main/personal-main.component';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { A2Edatetimepicker} from 'ng2-eonasdan-datetimepicker'
import { TooltipDirective } from '@directives/tooltip.directive';
@NgModule({
  declarations: [
    PersonalMainComponent
  ],
  imports: [
    CommonModule,
    PersonalRoutingModule,
    A2Edatetimepicker,
    ModalModule.forRoot(),
    NgSelectModule,
    FormsModule,
    TranslateModule,
    TooltipDirective
  ]
})
export class PersonalModule { }
