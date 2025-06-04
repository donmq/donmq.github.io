import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CategoryManageRoutingModule } from './category-management-routing.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonsModule } from '../../commons/commons.module';
import { NgxSpinnerModule } from 'ngx-spinner';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { ModalModule } from "ngx-bootstrap/modal";
import { TranslateModule } from '@ngx-translate/core';
import { CategoryManagementViewComponent } from './category-management-view/category-management-view.component';
import { CategoryManagementAddEditComponent } from './category-management-add-edit/category-management-add-edit.component';
import { CategoryManagementDetailComponent } from './category-management-detail/category-management-detail.component';

@NgModule({
  declarations: [
    CategoryManagementViewComponent,
    CategoryManagementAddEditComponent,
    CategoryManagementDetailComponent,
  ],
  imports: [
    CommonModule,
    CategoryManageRoutingModule,
    FormsModule,
    ReactiveFormsModule,
    CommonsModule,
    NgxSpinnerModule,
    PaginationModule,
    TranslateModule,
    ModalModule.forRoot(),
  ]
})
export class CategoryManagementModule { }
