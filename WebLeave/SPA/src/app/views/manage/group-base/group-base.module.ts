import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { GroupBaseMainComponent } from './group-base-main/group-base-main.component';
import { GroupBaseRoutingModule } from './group-base-routing.module';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';

@NgModule({
  declarations: [
    GroupBaseMainComponent,
  ],
  imports: [
    CommonModule,
    FormsModule,
    TranslateModule,
    GroupBaseRoutingModule
  ]
})
export class GroupBaseModule { }
