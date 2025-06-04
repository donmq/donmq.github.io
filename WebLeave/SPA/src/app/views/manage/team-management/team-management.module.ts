import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TeamManagementRoutingModule } from './team-management-routing.module';
import { MainComponent } from './main/main.component';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TranslateModule } from '@ngx-translate/core';
import { CreateOrUpdateComponent } from './create-or-update/create-or-update.component';


@NgModule({
  declarations: [
    MainComponent,
    CreateOrUpdateComponent
  ],
  imports: [
    CommonModule,
    TeamManagementRoutingModule,
    FormsModule,
    NgSelectModule,
    PaginationModule,
    ModalModule.forRoot(),
    TranslateModule
  ]
})
export class TeamManagementModule { }
