import { Component, OnInit } from '@angular/core';
import { Category } from '@models/manage/category-management/category';
import { InjectBase } from '@utilities/inject-base-app';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-category-management-detail',
  templateUrl: './category-management-detail.component.html',
  styleUrls: ['./category-management-detail.component.scss']
})
export class CategoryManagementDetailComponent extends InjectBase implements OnInit {
  categoryDetail: Category;
  constructor(public bsModalRef: BsModalRef) { super() }

  ngOnInit(): void {
  }

}
