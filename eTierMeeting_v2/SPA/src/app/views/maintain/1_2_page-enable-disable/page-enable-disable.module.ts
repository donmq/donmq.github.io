import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';
import { PageEnableDisableRoutingModule } from './page-enable-disable-routing.module';
import { PageEnableDisableComponent } from './page-enable-disable/page-enable-disable.component';

@NgModule({
  imports: [
    CommonModule,
    PageEnableDisableRoutingModule,
    NgSelectModule,
    FormsModule
  ],
  exports: [],
  declarations: [
    PageEnableDisableComponent
  ],
  providers: [],
})
export class PageEnableDisableModule { }
