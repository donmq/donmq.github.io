import { Component, OnInit, ViewChild } from '@angular/core';
import { UntypedFormControl } from '@angular/forms';
import { TranslateService } from '@ngx-translate/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { debounceTime, startWith, switchMap, tap } from 'rxjs/operators';
import { OperationResult } from '../../../../_core/_models/operation-result';
import { Pagination } from '../../../../_core/_models/pagination';
import { Roles, RolesDTO } from '../../../../_core/_models/roles';
import { User } from '../../../../_core/_models/user';
import { UserRoles } from '../../../../_core/_models/userRoles';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { PreliminaryListService } from '../../../../_core/_services/preliminary-list.service';
import { RolesService } from '../../../../_core/_services/roles.service';
import { SweetAlertService } from '../../../../_core/_services/sweet-alert.service';
import { UserRolesService } from '../../../../_core/_services/user-roles.service';
import { UserService } from '../../../../_core/_services/user.service';
import { RolesConst } from '../../../../_core/_constants/roles.constants';

@Component({
  selector: 'app-user-home',
  templateUrl: './user-home.component.html',
  styleUrls: ['./user-home.component.scss']
})
export class UserHomeComponent implements OnInit {
  @ViewChild('childModal', { static: false }) childModal: ModalDirective;
  listUser: User[] = [];
  userUpdate: any = [];
  keyword: UntypedFormControl = new UntypedFormControl('');
  lang: string;
  passWord: string = '';
  pagination: Pagination = {
    currentPage: 1,
    pageSize: 7,
    totalCount: 0,
    totalPage: 0,
  };
  documents: RolesDTO[] = [];
  checkedRole: string[] = [];
  listRoles: UserRoles[] = [];
  checkSokiem: boolean = true;
  checkRoles: number;

  checkChangePage: boolean = true;
  currentPage: number;
  constructor(
    private _userService: UserService,
    private _sweetAlert: SweetAlertService,
    private translate: TranslateService,
    private _alertifyService: AlertifyService,
    private _spinner: NgxSpinnerService,
    private rolesService: RolesService,
    private priliminaryListService: PreliminaryListService,
    private userRolesService: UserRolesService
  ) { }

  ngOnInit() {
    this.searchDelay();
    this.getAllRoles();
  }

  keywordAdd(value: string) {
    this.keyword.setValue(value);
  }

  getAllRoles() {
    this.rolesService.getAllRoles().subscribe(res => {
      this.documents = res;
    });
  }

  searchDelay() {
    this.keyword.valueChanges.pipe(
      tap(() => {
        this._spinner.show('from-list');
        this.pagination.currentPage = 1;
      }),
      debounceTime(500),
      startWith(''),
      switchMap(res => {
        return this._userService.searchUser(res, this.pagination);
      }),
    ).subscribe(val => {
      this.listUser = val.result;
      this.pagination = val.pagination;
      this.currentPage = this.pagination.currentPage;
      this._spinner.hide('from-list');
    });
  }

  pageChanged(event: any): void {
    this.keyword.patchValue(this.keyword.value);
  }

  clearSearch() {
    this.pagination.currentPage = 1;
    this.keyword.setValue('');
  }

  removeUser(userName: string) {
    this.lang = localStorage.getItem('lang');
    this.priliminaryListService.getPreliminaryPlnos(userName).subscribe(res => {
      if (res.listBuilding?.length !== 0) {
        this.checkSokiem = false;
      }
    });
    this._sweetAlert.confirm('Delete', this.translate.instant('alert.admin.are_you_sure_you_want_to_delete_this_user'), () => {
      this._userService.removeUser(userName, this.lang).subscribe((res: OperationResult) => {
        if (res.success) {
          this._sweetAlert.success('Success', res.message);
          this.keyword.patchValue('');
        } else {
          this._sweetAlert.error('Failed', res.message);
        }
      });
    });
  }
  restoreUser(userName: string) {
    this.lang = localStorage.getItem('lang');
    this._sweetAlert.confirm1('Restore', this.translate.instant('alert.admin.are_you_sure_you_want_to_restore_this_user'), () => {
      this._userService.restoreUser(userName, this.lang).subscribe((res: OperationResult) => {
        if (res.success) {
          this._sweetAlert.success('Success', res.message);
          this.keyword.setValue(userName);
        } else {
          this._sweetAlert.error('Failed', res.message);
        }
      });
    });
  }

  updateUser() {
    this.lang = localStorage.getItem('lang');
    if (this.userUpdate.empName === '') {
      return this._sweetAlert.error('Error', this.translate.instant('alert.admin.please_enter_user_name'));
    }
    if (this.userUpdate.roles.length === 0) {
      return this._sweetAlert.error(this.translate.instant('alert.admin.choes_roles_inventory'));
    }
    if (!this.checkSokiem && this.checkRoles !== this.userUpdate.roles.find(x => x === 3)) {
      return this._alertifyService.error(this.translate.instant('admin.adminuser.check_update_roles'));
    }
    this.userUpdate.hashPass = this.passWord;
    this._userService.updateUser(this.userUpdate, this.lang).subscribe(res => {
      if (res.success) {
        this._sweetAlert.success('Success', res.message);
        this.keyword.setValue(this.userUpdate.userName);
        this.documents.map(item => {
          item.checked = false;
        });

      } else {
        this._sweetAlert.error('Error', res.message);
      }
    }, error => {
      this._alertifyService.error(this.translate.instant('error.system_error'));
    });
    this.hideChildModal();
  }

  showChildModal(item: User): void {
    this.checkSokiem = true;
    this.documents.map((x) => {
      if (x.id !== 1) {
        x.checked = false;
        x.visible = true;
      } else {
        x.checked = false;
        x.visible = false;
      }
    });
    this.priliminaryListService.getPreliminaryPlnos(item.userName).subscribe(res => {
      if (res.listBuilding?.length > 0) {
        this.checkSokiem = false;
      }
    });
    this.userRolesService.getRoleByUser(item.userName).subscribe(res => {
      this.listRoles = res.filter(i => i.active === true);
      const roles = [];
      this.listRoles.map(x => {
        roles.push(x.roles);
      });
      item.roles = roles;
      this.checkRoles = item.roles.find(i => i === RolesConst.soKiem);
      const userObject = {
        userID: item.userID,
        userName: item.userName,
        hashPass: item.hashPass,
        hashImage: item.hashImage,
        emailAddress: item.emailAddress,
        visible: item.visible,
        updateDate: item.updateDate,
        updateBy: item.updateBy,
        empName: item.empName,
        roles: item.roles,
      };
      this.userUpdate = userObject;
      this.userUpdate.roles.map(val => {
        if (val !== 0) {
          this.documents.filter(f => f.id === val).map(x => {
            x.checked = true;
            x.visible = false;
          });
          if (this.documents.find(i => i.id === 4 && i.checked === true)) {
            this.documents.find(i => i.id === 5).visible = false;
          }
          if (this.documents.find(i => i.id === 5 && i.checked === true)) {
            this.documents.find(i => i.id === 4).visible = false;
          }
        }
      });
      if (this.userUpdate.roles.length === 1 && this.userUpdate.roles.find(i => i === RolesConst.admin) !== undefined) {
        this.documents.map(x => {
          if (x.id !== 1) {
            x.visible = false;
          }
        });
      }
      this.childModal.show();
    });
  }

  hideChildModal() {
    this.childModal.hide();
  }

  exportExcel() {
    this._userService.exportExcel();
  }

  checkboxClicked(event, id) {
    const items = [];
    this.documents.filter(x => x.checked).map(x => {
      const item = x.id;
      items.push(item);
    });
    this.userUpdate.roles = items;
    this.checkDisabledRoles(event, id);
  }

  checkDisabledRoles(event, id) {
    if (id === 2) {
      if (event.target.checked === true) {
        this.documents.map((i) => {
          if (i.id === 3 || i.id === 4 || i.id === 5) {
            i.visible = true;
            i.checked = false;
          }
        });
      } else if (event.target.checked === false) {
        this.documents.map((i) => {
          if (i.id === 3 || i.id === 4 || i.id === 5) {
            i.visible = false;
            i.checked = false;
          }
        });
      }
    } else if (id === 3) {
      if (event.target.checked === true) {
        this.documents.map((i) => {
          if (i.id === 2 || i.id === 4 || i.id === 5) {
            i.visible = true;
            i.checked = false;
          }
        });
      } else if (event.target.checked === false) {
        this.documents.map((i) => {
          if (i.id === 2 || i.id === 4 || i.id === 5) {
            i.visible = false;
            i.checked = false;
          }
        });
      }
    } else if (id === 4) {
      if (event.target.checked === true) {
        this.documents.map((i) => {
          if (i.id === 2 || i.id === 3) {
            i.visible = true;
            i.checked = false;
          }
        });
      } else {
        if (this.documents.find(f => f.id === 5).checked === true) {
          this.documents.map((i) => {
            if (i.id === 2 || i.id === 3) {
              i.visible = true;
              i.checked = false;
            }
          });
        } else {
          this.documents.map((i) => {
            {
              if (i.id !== 1) {
                i.visible = false;
                i.checked = false;
              }
            }
          });
        }
      }
    } else if (id === 5) {
      if (event.target.checked === true) {
        this.documents.map((i) => {
          if (i.id === 2 || i.id === 3) {
            i.visible = true;
            i.checked = false;
          }
        });
      } else {
        if (this.documents.find(f => f.id === 4).checked === true) {
          this.documents.map((i) => {
            if (i.id === 2 || i.id === 3) {
              i.visible = true;
              i.checked = false;
            }
          });
        } else {
          this.documents.map((i) => {
            {
              if (i.id !== 1) {
                i.visible = false;
                i.checked = false;
              }
            }
          });
        }
      }
    }
  }
}
