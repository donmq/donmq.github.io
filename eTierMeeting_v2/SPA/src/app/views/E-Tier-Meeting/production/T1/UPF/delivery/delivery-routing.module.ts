import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DeliveryMainComponent } from './delivery-main/delivery-main.component';

const routes: Routes = [
  {
    path: "deliverymain/UPF/:deptId",
    component: DeliveryMainComponent,
    data: {
      title: "Delivery-MainPage",
      page_Name: 'Delivery'
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class DeliveryRoutingModule { }
