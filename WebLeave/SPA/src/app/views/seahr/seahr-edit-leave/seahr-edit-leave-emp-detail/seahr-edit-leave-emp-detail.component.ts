import { Component, OnInit } from '@angular/core';
import { LangConstants } from '@constants/lang.constants';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { LeaveData } from '@models/common/leave-data';
import { DetailEmployee } from '@models/seahr/edit-leave/detail-employee';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-seahr-edit-leave-emp-detail',
  templateUrl: './seahr-edit-leave-emp-detail.component.html',
  styleUrls: ['./seahr-edit-leave-emp-detail.component.scss']
})
export class SeahrEditLeaveEmpDetailComponent implements OnInit {
  detailEmployee: DetailEmployee;
  leaveData: LeaveData;
  lang: string = localStorage.getItem(LocalStorageConstants.LANG) ?? LangConstants.VN;
  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
  }


}
