import { ViewerDirective } from './viewer.directive';
import { NgModule } from '@angular/core';
import { CommonModule, NgOptimizedImage } from '@angular/common';
import { ViewerService } from './viewer.service';
import { ViewerContainerComponent } from './viewer-container/viewer-container.component';

@NgModule({
  imports: [
    CommonModule,
    NgOptimizedImage,
  ],
  declarations: [ViewerContainerComponent, ViewerDirective],
  exports: [ViewerContainerComponent, ViewerDirective],
  providers: [ViewerService]
})
export class ViewerModule { }
