import { CommonModule, DatePipe } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { TranslateModule } from '@ngx-translate/core';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgxSpinnerModule } from 'ngx-spinner';
import { InventoryGetLineResolve } from '../../../_core/_resolver/inventoryGetLine.resolver';
import { MainModule } from '../main.module';
import { InventoryBuildingComponent } from './inventory-building/inventory-building.component';
import { InventoryHomeListComponent } from './inventory-home/inventory-home-list/inventory-home-list.component';
import { InventoryHomeLoginComponent } from './inventory-home/inventory-home-login/inventory-home-login.component';
import { InventoryHomeComponent } from './inventory-home/inventory-home.component';
import { InventoryLineComponent } from './inventory-line/inventory-line.component';
import { InventoryOptionsComponent } from './inventory-options/inventory-options.component';
import { InventoryRoutingModule } from './inventory-routing.module';

@NgModule({
    imports: [
        CommonModule,
        FormsModule,
        InventoryRoutingModule,
        NgxSpinnerModule,
        MainModule,
        TranslateModule,
        ModalModule.forRoot(),
    ],
    declarations: [
        InventoryBuildingComponent,
        InventoryLineComponent,
        InventoryHomeComponent,
        InventoryOptionsComponent,
        InventoryHomeListComponent,
        InventoryHomeLoginComponent
    ],
    providers: [
        InventoryGetLineResolve, DatePipe,
    ]
})
export class InventoryModule { }
