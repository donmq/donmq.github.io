import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ReportMainComponent } from './report-main/report-main.component';

const routes: Routes = [
  {
    path: 'report-show',
    loadChildren: () =>
      import('./report-show/reportshow.module').then((m) => m.ReportShowModule),
  },
  {
    path: '',
    component: ReportMainComponent,
    data: {
      title: 'Report Main',
    },
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class ReportRoutingModule {}
