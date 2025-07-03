import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { DeliverymainComponent } from "./deliverymain/deliverymain.component";

const routes: Routes = [
  {
    path: "deliverymain/CTB/:deptId",
    component: DeliverymainComponent,
    data: {
      title: "Delivery-MainPage",
      page_Name: 'Delivery'
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DeliveryRoutingModule { }
