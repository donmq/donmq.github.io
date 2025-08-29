import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RewardAndPenaltyReasonCodeMaintenanceRoutingModule } from './reward-and-penalty-reason-code-maintenance-routing.module';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { FormAddComponent } from './form-add/form-add.component';
import { FormEditComponent } from './form-edit/form-edit.component';
import { MainComponent } from './main/main.component';


@NgModule({
  declarations:  [
    MainComponent,
    FormEditComponent,
    FormAddComponent
  ],
  imports: [
    CommonModule,
    RewardAndPenaltyReasonCodeMaintenanceRoutingModule,
    FormsModule,
    PaginationModule.forRoot(),
    TranslateModule,
    NgSelectModule,
  ]
})
export class RewardAndPenaltyReasonCodeMaintenanceModule { }
