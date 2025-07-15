import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InventoryGetLineResolve } from '../../../_core/_resolver/inventoryGetLine.resolver';
import { InventoryBuildingComponent } from './inventory-building/inventory-building.component';
import { InventoryHomeComponent } from './inventory-home/inventory-home.component';
import { InventoryLineComponent } from './inventory-line/inventory-line.component';

const routes: Routes = [{
    path: '',
    component: InventoryBuildingComponent,
    data: {
        title: 'building-inventory'
    }
},
{
    path: 'line/:building/:check/:optons',
    component: InventoryLineComponent,
    resolve: {
        lineInventoy: InventoryGetLineResolve
    },
    data: {
        title: 'line-inventory'
    }
},
{
    path: 'inventory-home/:optonsLine',
    component: InventoryHomeComponent,
    data: {
        title: 'home-inventory'
    }

},
];
@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class InventoryRoutingModule { }
