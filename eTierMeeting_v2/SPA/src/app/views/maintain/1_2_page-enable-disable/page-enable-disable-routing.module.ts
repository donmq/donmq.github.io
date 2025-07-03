import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { PageEnableDisableComponent } from "./page-enable-disable/page-enable-disable.component";

const routes: Routes = [
  {
    path: '',
    component: PageEnableDisableComponent
  }
]

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class PageEnableDisableRoutingModule { }