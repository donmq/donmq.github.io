import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { EditComponent } from './edit/edit.component';

const routes: Routes = [
  {
          path: '',
          component:  MainComponent,
          data: {
            title: 'assets-lend-maintain-home'
          }
      },
      {
          path: 'edit',
          component:  EditComponent,
          data: {
            title: 'assets-lend-maintain-edit'
          }
      }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AssetsLendMaintainRoutingModule { }
