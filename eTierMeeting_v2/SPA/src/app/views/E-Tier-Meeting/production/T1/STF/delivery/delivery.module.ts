import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { SnotifyModule } from "ng-alt-snotify";
import { NgxSpinnerModule } from "ngx-spinner";
import { CommonsModule } from "../../../../../commons/commons.module";
import { DeliveryRoutingModule } from "./delivery-routing.module";
import { DeliverymainComponent } from "./deliverymain/deliverymain.component";

@NgModule({
  declarations: [
    DeliverymainComponent],
  imports: [
    CommonModule,
    CommonsModule,
    FormsModule,
    DeliveryRoutingModule,
    NgxSpinnerModule,
    SnotifyModule
  ],
})
export class DeliveryModule { }
