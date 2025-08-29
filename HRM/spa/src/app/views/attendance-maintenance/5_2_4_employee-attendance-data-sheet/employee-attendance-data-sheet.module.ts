import { TranslateModule } from '@ngx-translate/core';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';
import { EmployeeAttendanceDataSheetRoutingModule } from './employee-attendance-data-sheet-routing.module';
import { MainComponent } from './main/main.component';
@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule,
    BsDatepickerModule.forRoot(),
    NgSelectModule,
    EmployeeAttendanceDataSheetRoutingModule,
    NgxMaskDirective,
    NgxMaskPipe,
  ],
  declarations: [MainComponent],
})
export class EmployeeAttendanceDataSheetModule { }
