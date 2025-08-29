import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { TabComponent } from './tab-component/tab.component';


@NgModule({
  declarations: [TabComponent],
  exports: [TabComponent],
  imports: [
    CommonModule,
    TabsModule.forRoot()
  ]
})
export class SharedModule { }
