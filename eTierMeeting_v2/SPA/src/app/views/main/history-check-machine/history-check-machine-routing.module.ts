import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HistoryCheckMachineHomeComponent } from './history-check-machine-home/history-check-machine-home.component';
import { HistoryCheckMachineViewComponent } from './history-check-machine-view/history-check-machine-view.component';

const routes: Routes = [
    {
        path: '',
        component:  HistoryCheckMachineHomeComponent,
        data: {
            title: 'history-check-machine-home'
        }
    },
    {
        path: 'historycheckmachineview',
        component:  HistoryCheckMachineViewComponent,
        data: {
            title: 'history-check-machine-view'
        }
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class HistoryCheckMachineRoutingModule { }
