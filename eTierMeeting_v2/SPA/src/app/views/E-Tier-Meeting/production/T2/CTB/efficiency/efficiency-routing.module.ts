import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EfficiencyMainComponent } from './efficiencymain/efficiencymain.component';
import { Efficiencymain_page2Component } from './efficiencymain_page2/efficiencymain_page2/efficiencymain_page2.component';


const routes: Routes = [
    {
        path: 'efficiencymain/CTB/:deptId',
        component: EfficiencyMainComponent,
        data: {
            title: 'Efficiency-MainPage',
            page_Name: 'Efficiency'
        }
    },
    {
        path: 'efficiencymain_2/CTB/:deptId',
        component: Efficiencymain_page2Component,
        data: {
            title: 'Efficiency-MainPage-2',
            page_Name: 'Efficiency'
        }
    }


];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class EfficiencyRoutingModule { }
