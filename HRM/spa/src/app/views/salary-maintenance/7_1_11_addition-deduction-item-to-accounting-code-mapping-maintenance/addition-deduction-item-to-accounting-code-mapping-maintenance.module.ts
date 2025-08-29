import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdditionDeductionItemtoAccountingCodeMappingMaintenanceRoutingModule } from './addition-deduction-item-to-accounting-code-mapping-maintenance-routing.module';
import { MainComponent } from "./main/main.component";
import { FormComponent } from "./form/form.component";
import { TranslateModule } from '@ngx-translate/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { DragScrollComponent, DragScrollItemDirective } from 'ngx-drag-scroll';
import { PipesModule } from 'src/app/_core/pipes/pipes.module';

@NgModule({
  declarations: [
    MainComponent,
    FormComponent
  ],
  imports: [
    CommonModule,
    AdditionDeductionItemtoAccountingCodeMappingMaintenanceRoutingModule,
    TranslateModule,
    FormsModule,
    ReactiveFormsModule,
    NgSelectModule,
    PaginationModule.forRoot(),
    // BsDatepickerModule.forRoot(),
    DragScrollComponent,
    DragScrollItemDirective,
    PipesModule,
    // NgxSpinnerModule,
    // TypeaheadModule.forRoot(),
    // NgxMaskDirective,
    // NgxMaskPipe
  ]
})
export class AdditionDeductionItemtoAccountingCodeMappingMaintenanceModule { }
