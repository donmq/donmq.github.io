import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { Category } from '@models/manage/category-management/category';
import { CategoryService } from '@services/manage/category.service';
import { Pagination, PaginationParam, PaginationResult } from '@utilities/pagination-utility';
import { CategoryManagementAddEditComponent } from '../category-management-add-edit/category-management-add-edit.component';
import { CategoryManagementDetailComponent } from '../category-management-detail/category-management-detail.component';
import { InjectBase } from '@utilities/inject-base-app';
@Component({
  selector: 'app-category-management-view',
  templateUrl: './category-management-view.component.html',
  styleUrls: ['./category-management-view.component.scss']
})
export class CategoryManagementViewComponent extends InjectBase implements OnInit {
  category: Category[] = [];
  pagination: Pagination = <Pagination>{
    pageNumber: 1,
    pageSize: 20,
  }
  modalRef?: BsModalRef;

  constructor(
    private service: CategoryService,
    private modalService: BsModalService,
  ) {
    super();
  }

  ngOnInit(): void {
    this.getAllCategory(this.pagination);
  }

  getAllCategory(pagination: PaginationParam) {
    this.spinnerService.show();
    this.service.getAllCategory(pagination).subscribe({
      next: (res: PaginationResult<Category>) => {
        this.category = res.result;
        this.pagination = res.pagination;
      },
      error: (err) => {
        this.snotifyService.error(this.translateService.instant('System.Message.SystemError'), this.translateService.instant('System.Caption.Error'));
        this.spinnerService.hide();
      },
      complete: () => this.spinnerService.hide()
    });
  }

  pageChanged(event: any) {
    this.pagination.pageNumber = event.page;
    this.getAllCategory(this.pagination);
  }

  openEditModal(item: Category) {
    const initState: ModalOptions = {
      initialState: {
        data: {
          cate: item,
          type: 'edit'
        }
      }
    };

    this.modalRef = this.modalService.show(CategoryManagementAddEditComponent, initState);

  }

  openAddModal() {
    const initState: ModalOptions = {
      initialState: {
        data: {
          type: 'add'
        }
      }
    };
    this.modalRef = this.modalService.show(CategoryManagementAddEditComponent, initState);
  }

  openDetailModal(model: Category) {
    const initialState: ModalOptions = {
      initialState: {
        categoryDetail: model
      }
    }
    this.modalRef = this.modalService.show(CategoryManagementDetailComponent, initialState);

  }

  back() {
    this.router.navigate(['/manage']);
  }
}
