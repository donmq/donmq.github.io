import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NightShiftSubsidyMaintenanceRoutingModule } from './night-shift-subsidy-maintenance-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelectModule } from '@ng-select/ng-select';
import { MainComponent } from './main/main.component';
import { PipesModule } from 'src/app/_core/pipes/pipes.module';
import { NgxSpinnerModule } from 'ngx-spinner';
import { NgxMaskDirective, NgxMaskPipe } from 'ngx-mask';


@NgModule({
  declarations: [MainComponent],
  imports: [
    NightShiftSubsidyMaintenanceRoutingModule,
        CommonModule,
        FormsModule,
        PipesModule,
        ReactiveFormsModule,
        NgxSpinnerModule,
        BsDatepickerModule.forRoot(),
        TranslateModule,
        NgSelectModule,
        NgxMaskDirective,
        NgxMaskPipe
  ]
})
export class NightShiftSubsidyMaintenanceModule { }
