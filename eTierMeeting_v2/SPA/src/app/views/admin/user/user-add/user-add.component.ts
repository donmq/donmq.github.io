import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { RolesDTO } from '../../../../_core/_models/roles';
import { User } from '../../../../_core/_models/user';
import { AlertifyService } from '../../../../_core/_services/alertify.service';
import { RolesService } from '../../../../_core/_services/roles.service';
import { SweetAlertService } from '../../../../_core/_services/sweet-alert.service';
import { UserService } from '../../../../_core/_services/user.service';

@Component({
  selector: 'app-user-add',
  templateUrl: './user-add.component.html',
  styleUrls: ['./user-add.component.scss']
})
export class UserAddComponent implements OnInit {
  @Output() keyword = new EventEmitter<string>();
  documents: RolesDTO[] = [];
  user: User = {
    userID: 0,
    userName: '',
    hashPass: '',
    hashImage: '',
    emailAddress: '',
    visible: true,
    updateDate: null,
    updateBy: '',
    empName: '',
    roles: []
  };
  checkedRole: string[] = [];
  lang: string;

  constructor(
    private _userService: UserService,
    private _sweetAlert: SweetAlertService,
    private translate: TranslateService,
    private _alertifyService: AlertifyService,
    private rolesService: RolesService
  ) { }

  ngOnInit() {
    this.getAllRoles();
  }

  getAllRoles() {
    this.rolesService.getAllRoles().subscribe(res => {
      this.documents = res;
    });
  }

  checkboxClicked(event, id) {
    const items = [];
    this.documents.filter(x => x.checked).map(x => {
      const item = x.id;
      items.push(item);
    });
    this.user.roles = items;
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

  addUser() {
    this.lang = localStorage.getItem('lang');
    if (this.user.empName === '') {
      return this._sweetAlert.error(this.translate.instant('alert.admin.please_enter_user_name'));
    }
    if (this.user.userName === '') {
      return this._sweetAlert.error(this.translate.instant('alert.admin.please_enter_user_code'));
    }
    if (this.user.roles.length === 0) {
      return this._sweetAlert.error(this.translate.instant('alert.admin.choes_roles_inventory'));
    }
    this._userService.addUser(this.user, this.lang).subscribe(res => {
      if (res.success) {
        this._sweetAlert.success('Success', res.message);
        this.keyword.emit(this.user.userName);
        this.clearFormAdd();
      } else {
        this._sweetAlert.error('Error', res.message);
      }
    }, error => {
      this._alertifyService.error(this.translate.instant('error.system_error'));
    });

  }

  clearFormAdd() {
    this.user.userName = '';
    this.user.empName = '';
    this.user.emailAddress = '';
    this.user.hashPass = '';
    this.user.roles = [];
    this.documents.map(item => {
      item.checked = false;
      item.visible = false;
    });
  }
}
