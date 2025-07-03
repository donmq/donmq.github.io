import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SelectlinemainComponent } from './selectlinemain/selectlinemain.component';


const routes: Routes = [
  {
    path: 'selectlinemain',
    component: SelectlinemainComponent,
    data: {
      title: 'SelectLine-MainPage'
    }
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SelectlineRoutingModule { }
