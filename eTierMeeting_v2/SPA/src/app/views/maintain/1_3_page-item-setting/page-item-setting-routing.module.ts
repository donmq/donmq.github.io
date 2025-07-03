import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PageItemSettingResolver } from '../../../_core/_resolvers/production/T2/CTB/page-item-setting.resolver';
import { MainComponent } from './main/main.component';

const routes: Routes = [
  {
    path: '',
    resolve: { res: PageItemSettingResolver },
    component: MainComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PageItemSettingRoutingModule { }
