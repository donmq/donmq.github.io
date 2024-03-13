import { AboutComponent } from './../about/about.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HomeRoutingModule } from './home-routing.module';
import { MainHomeComponent } from './main-home/main-home.component';
import { ContactComponent } from '../contact/contact.component';
import { ProfileComponent } from '../profile/profile.component';


@NgModule({
  declarations: [
    MainHomeComponent
  ],
  imports: [
    CommonModule,
    HomeRoutingModule,
    AboutComponent,
    ContactComponent,
    ProfileComponent
  ]
})
export class HomeModule { }
