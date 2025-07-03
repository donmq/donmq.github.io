import { Component, OnInit, ViewChild } from '@angular/core';
import { BsModalRef, BsModalService, ModalDirective } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { Pagination } from '@models/pagination';
import { RoleByUser } from '@models/role-by-user';
import { AddUser } from '@models/user';
import { NgSnotifyService } from '@services/ng-snotify.service';
import { UserService } from '@services/user.service';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.scss']
})
export class UserComponent extends InjectBase implements OnInit {
  users: AddUser[] = [];
  account: string = '';
  isActive: string = 'all';
  pagination: Pagination = {
    currentPage: 1,
    itemsPerPage: 10,
    totalItems: 1,
    totalPages: 1,
  };
  addUser: AddUser = new AddUser();

  modalRef: BsModalRef;
  @ViewChild('authorizationModal', { static: false }) authorizationModal: ModalDirective;
  userAuthorizationAccount: string = '';
  userAuthorizationName: string = '';
  listRoleByUser: RoleByUser[] = [];

  editUser: AddUser = new AddUser();
  @ViewChild('modalEditUser', { static: false }) modalEditUser: ModalDirective;

  constructor(private userService: UserService,
    private modalService: BsModalService) {
    super();
  }

  ngOnInit() {
    this.getUser();
  }

  getUser() {
    this.spinnerService.show();
    this.userService.getUsers(this.account, this.isActive, this.pagination.currentPage, this.pagination.itemsPerPage)
      .subscribe(res => {
        this.users = res.result;
        this.pagination = res.pagination;
        this.spinnerService.hide();
      });
  }

  search() {
    this.pagination.currentPage = 1;
    this.getUser();
  }

  saveAddUser() {
    this.spinnerService.show();
    this.userService.addUser(this.addUser)
      .subscribe(() => {
        this.snotifyService.success('Add user success!');
        this.spinnerService.hide();
        this.getUser();
      }, error => {
        this.snotifyService.error('Fail add user!');
        this.spinnerService.hide();
      });
  }

  openModalAuthorization(account: string, name: string) {
    this.userAuthorizationAccount = account;
    this.userAuthorizationName = name;
    this.userService.getRoleByUser(this.userAuthorizationAccount).subscribe(res => {
      this.listRoleByUser = res;
      this.authorizationModal.show();
    });
  }

  saveAuthorizationUser() {
    const updateRoleByUser = this.listRoleByUser.filter(item => {
      return item.status === true;
    });
    this.spinnerService.show();
    this.userService.updateRoleByUser(this.userAuthorizationAccount, updateRoleByUser)
      .subscribe(() => {
        this.snotifyService.success('Update role user success!');
        this.spinnerService.hide();
        this.authorizationModal.hide();
      }, error => {
        this.snotifyService.error('Fail update role user!');
        this.spinnerService.hide();
      });
  }

  saveEditUser() {
    this.spinnerService.show();
    this.userService.updateUser(this.editUser)
      .subscribe(() => {
        this.snotifyService.success('Update user success!');
        this.spinnerService.hide();
        this.modalEditUser.hide();
        this.getUser();
      }, error => {
        this.snotifyService.error('Fail update user!');
        this.spinnerService.hide();
      });
  }

  pageChanged(event: any): void {
    this.pagination.currentPage = event.page;
    this.getUser();
  }

  openModalEditUser(user: AddUser) {
    this.editUser = user;
    this.modalEditUser.show();
  }
}
