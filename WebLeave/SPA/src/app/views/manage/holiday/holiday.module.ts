import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule, ReactiveFormsModule } from "@angular/forms";
import { TranslateModule } from "@ngx-translate/core";
import { BsDatepickerModule } from "ngx-bootstrap/datepicker";
import { ModalModule } from "ngx-bootstrap/modal";
import { HolidayMainComponent } from "./holiday-main/holiday-main.component";
import { HolidayRoutingModule } from "./holiday-routing.module";


@NgModule({
  declarations: [
    HolidayMainComponent
  ],
  imports: [
    FormsModule,
    CommonModule,
    ReactiveFormsModule,
    ModalModule.forRoot(),
    BsDatepickerModule.forRoot(),
    TranslateModule,
    HolidayRoutingModule
  ]
})
export class HolidayModule { }
