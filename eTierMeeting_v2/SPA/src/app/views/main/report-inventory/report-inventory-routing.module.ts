import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReportInventoryBuildingComponent } from './report-inventory-building/report-inventory-building.component';
import { ReportInventoryLineComponent } from './report-inventory-line/report-inventory-line.component';

const routes: Routes = [{
    path: '',
    component: ReportInventoryBuildingComponent,
    data: {
        title: 'ReportInventory'
    }
},
{
    path: 'line',
    component: ReportInventoryLineComponent,
    data: {
        title: 'line-inventory'
    }
}
];
@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})

export class ReportInventoryRoutingModule { }
