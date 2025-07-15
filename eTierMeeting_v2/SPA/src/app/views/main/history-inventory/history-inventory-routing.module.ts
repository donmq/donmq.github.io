import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HistoryInventoryHomeComponent } from './history-inventory-home/history-inventory-home.component';
import { HistoryInventoryViewComponent } from './history-inventory-view/history-inventory-view.component';

const routes: Routes = [
    {
        path: '',
        component:  HistoryInventoryHomeComponent,
        data: {
            title: 'history-inventory-home'
        }
    },
    {
        path: 'historyinventoryview',
        component:  HistoryInventoryViewComponent,
        data: {
            title: 'history-inventory-view'
        }
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class HistoryInventoryRoutingModule { }
