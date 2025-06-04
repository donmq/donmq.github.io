import { Component, OnInit } from '@angular/core';
import { BsModalRef, BsModalService, ModalOptions } from 'ngx-bootstrap/modal';
import { Users } from '@models/auth/users.model';
import { ManageRoleAdminComponent } from '../manage-role-admin/manage-role-admin.component';
import { ManageRoleReportComponent } from '../manage-role-report/manage-role-report.component';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-manage-dashboard',
  templateUrl: './manage-role-main.component.html',
  styleUrls: ['./manage-role-main.component.css']
})
export class ManageRoleMainComponent extends InjectBase implements OnInit {
  bsModalRef?: BsModalRef;
  user: Users;
  id: string;
  constructor(
    private modalService: BsModalService,
  ) {
    super()
  }

  ngOnInit(): void {
    this.route.data.subscribe(data => {
      this.user = data['user'];
    })
  }

  ShowRoleAdmin(user: Users) {
    const initialState: ModalOptions = {
      class: 'modal-dialog-centered',
      initialState: {
        user
      }
    };
    this.bsModalRef = this.modalService.show(ManageRoleAdminComponent, initialState);
  }
  ShowRoleReport(user: Users) {
    const initialState: ModalOptions = {
      class: 'modal-dialog-centered',
      initialState: {
        user
      }
    };
    this.bsModalRef = this.modalService.show(ManageRoleReportComponent, initialState);
  }
}
