import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { TranslateModule } from '@ngx-translate/core';
import { NgSelect2Module } from 'ng-select2';
import { NgxSpinnerModule } from 'ngx-spinner';
import { MainModule } from '../main.module';
import { CheckMachineHomeComponent } from './check-machine-home/check-machine-home.component';
import { CheckMachineListComponent } from './check-machine-list/check-machine-list.component';
import { CheckMachineRoutingModule } from './check-machine-routing.module';

@NgModule({
    imports: [
        CommonModule,
        CheckMachineRoutingModule,
        NgSelect2Module,
        NgxSpinnerModule,
        FormsModule,
        ReactiveFormsModule,
        NgSelectModule,
        MainModule,
        TranslateModule
    ],
    declarations: [
        CheckMachineHomeComponent,
        CheckMachineListComponent,
    ]
})
export class CheckMachineModule { }