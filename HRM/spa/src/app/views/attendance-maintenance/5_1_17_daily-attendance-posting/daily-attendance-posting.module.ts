import { TranslateModule } from '@ngx-translate/core';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './main/main.component';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import {DailyAttendancePostingRoutingModule} from './daily-attendance-posting-routing.module'

@NgModule({
  imports: [
    CommonModule,
    DailyAttendancePostingRoutingModule,
    FormsModule,
    TranslateModule,
    NgSelectModule,
  ],
  declarations: [MainComponent]
})
export class DailyAttendancePostingModule { }
