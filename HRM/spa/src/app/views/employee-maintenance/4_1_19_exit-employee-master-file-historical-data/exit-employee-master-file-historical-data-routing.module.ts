import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { MainComponent } from "./main/main.component";
import { QueryComponent } from './query/query.component';
import { formGuard } from '@guards/app.guard';

const routes: Routes = [
  {
    path: '',
    component: MainComponent,
    resolve: {

    },
    data: {
      title: 'Main'
    }
  },
  {
    path: 'query',
    canMatch: [formGuard],
    component: QueryComponent,
    data: {
      title: 'Query'
    }
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ExitEmployeeMasterFileHistoricalDataRoutingModule { }
