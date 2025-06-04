import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ExportHpMainComponent } from './export-hp-main/export-hp-main.component';

const routes: Routes = [
  {
    path: '',
    component: ExportHpMainComponent,
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ExportHpRoutingModule { }
