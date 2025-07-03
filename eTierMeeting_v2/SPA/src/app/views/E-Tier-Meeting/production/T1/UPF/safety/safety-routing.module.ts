import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { SafetyMainComponent } from "./safetymain/safety-main.component";

const routes: Routes = [
  {
    path: 'safetymain/UPF/:deptId',
    component: SafetyMainComponent,
    data: {
      title: 'Safety-MainPage',
      page_Name: 'Safety'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SafetyRoutingModule { }
