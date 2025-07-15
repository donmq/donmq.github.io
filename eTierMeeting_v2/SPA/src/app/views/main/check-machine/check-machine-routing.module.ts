import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CheckMachineHomeComponent } from './check-machine-home/check-machine-home.component';
import { CheckMachineListComponent } from './check-machine-list/check-machine-list.component';

const routes: Routes = [
    {
        path: '',
        component: CheckMachineHomeComponent,
        data: {
            title: 'check-machine-home'
        }
    },
    {
        path: 'checkmachinelist',
        component: CheckMachineListComponent,
        // resolve: {
        //     lineInventoy: InventoryGetLineResolve
        // },
        data: {
            title: 'check-machine-list'
        }
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class CheckMachineRoutingModule { }
