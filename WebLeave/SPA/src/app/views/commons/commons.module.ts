import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { ModuleItemComponent } from './module-item/module-item.component';
import { NgOptimizedImage } from '@angular/common'

@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    NgOptimizedImage
  ],
  exports: [
    ModuleItemComponent
  ],
  declarations: [
    ModuleItemComponent
  ],
})
export class CommonsModule { }
