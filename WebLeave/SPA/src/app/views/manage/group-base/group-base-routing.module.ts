import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { GroupBaseMainComponent } from "./group-base-main/group-base-main.component";

const routes: Routes = [
  {
    path: '',
    data: {
      breadcrumb: 'GroupBase'
    },
    component: GroupBaseMainComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class GroupBaseRoutingModule { }
