import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalModule } from 'ngx-bootstrap/modal';
import { CommonsModule } from '../../../../../commons/commons.module';
import { ModelPreparationmainComponent } from './modelpreparationmain/modelpreparationmain.component';
import { ModelPreparationRoutingModule } from './model-preparation-routing.module';


@NgModule({
    declarations: [
        ModelPreparationmainComponent
    ],
    imports: [
        CommonModule,
        CommonsModule,
        ModelPreparationRoutingModule,
        ModalModule.forRoot()
    ]
})
export class ModelPreparationModule { }
