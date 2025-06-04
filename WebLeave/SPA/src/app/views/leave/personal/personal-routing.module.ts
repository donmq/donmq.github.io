import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { PersonalMainComponent } from './personal-main/personal-main.component';

const routes: Routes = [
  {
    path: '',
    component: PersonalMainComponent
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class PersonalRoutingModule { }
