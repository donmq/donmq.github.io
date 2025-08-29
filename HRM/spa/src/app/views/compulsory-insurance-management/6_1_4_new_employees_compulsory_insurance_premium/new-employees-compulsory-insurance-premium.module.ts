import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NewEmployeesCompulsoryInsurancePremiumRoutingModule } from './new-employees-compulsory-insurance-premium-routing.module';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { MainComponent } from './main/main.component';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';
import { SharedModule } from '@views/_shared/shared.module';


@NgModule({
  declarations: [MainComponent],
  imports: [
    CommonModule,
    NewEmployeesCompulsoryInsurancePremiumRoutingModule,
    FormsModule,
    PaginationModule.forRoot(),
    TranslateModule,
    NgSelectModule,
    BsDatepickerModule.forRoot(),
    TabsModule.forRoot(),
    NgxMaskDirective,
    NgxMaskPipe,
    SharedModule
  ]
})
export class NewEmployeesCompulsoryInsurancePremiumModule { }
