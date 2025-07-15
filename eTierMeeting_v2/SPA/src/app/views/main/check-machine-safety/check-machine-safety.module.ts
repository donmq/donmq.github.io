import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CheckMachineSafetyRoutingModule } from './check-machine-safety-routing.module';
import { HomeComponent } from './home/home.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelect2Module } from 'ng-select2';
import { NgxSpinnerModule } from 'ngx-spinner';
import { MainModule } from '../main.module';
import { WebcamModule } from 'ngx-webcam';
import { ViewerModule } from './../../image-viewer/viewer.module';

@NgModule({
  declarations: [
    HomeComponent
  ],
  imports: [
    CommonModule,
    CheckMachineSafetyRoutingModule,
    NgSelect2Module,
    NgxSpinnerModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
    MainModule,
    TranslateModule,
    WebcamModule,
    ViewerModule
  ]
})
export class CheckMachineSafetyModule { }
