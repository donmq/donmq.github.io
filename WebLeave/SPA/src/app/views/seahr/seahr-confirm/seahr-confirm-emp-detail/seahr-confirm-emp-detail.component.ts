import { Component, OnInit } from '@angular/core';
import { LangConstants } from '@constants/lang.constants';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { LeaveData } from '@models/common/leave-data';
import { SeaConfirmEmpDetail } from '@models/seahr/sea-confirm-emp-detail';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-seahr-confirm-emp-detail',
  templateUrl: './seahr-confirm-emp-detail.component.html',
  styleUrls: ['./seahr-confirm-emp-detail.component.scss']
})
export class SeahrConfirmEmpDetailComponent implements OnInit {
  lang: string = localStorage.getItem(LocalStorageConstants.LANG) ?? LangConstants.VN;
  leaveItem: LeaveData = <LeaveData>{};
  empDetail: SeaConfirmEmpDetail = <SeaConfirmEmpDetail>{};
  constructor(public bsModalRef: BsModalRef) { }

  ngOnInit(): void {
  }

}
