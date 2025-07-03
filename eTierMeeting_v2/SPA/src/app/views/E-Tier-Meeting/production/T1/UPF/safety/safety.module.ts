import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { ModalModule } from "ngx-bootstrap/modal";
import { CommonsModule } from "../../../../../commons/commons.module";
import { SafetyMainComponent } from "./safetymain/safety-main.component";
import { SafetyRoutingModule } from "../../UPF/safety/safety-routing.module";

@NgModule({
  imports: [
    CommonModule,
    CommonsModule,
    SafetyRoutingModule,
    ModalModule.forRoot(),
  ],
  declarations: [SafetyMainComponent],
})
export class SafetyModule {}
