import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { HolidayMainComponent } from "./holiday-main/holiday-main.component";


const routes: Routes = [
  {
    path: '',
    data: {
      breadcrumb: 'Holiday'
    },
    component: HolidayMainComponent,
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HolidayRoutingModule { }
