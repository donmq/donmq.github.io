import { Component, Input, OnInit } from '@angular/core';
import { ReportShowModel } from '@models/report/report-show/report-show-model.model';
import { BsModalService } from 'ngx-bootstrap/modal';
@Component({
  selector: 'app-popupReportGridDetail',
  templateUrl: './popupReportGridDetail.component.html',
  styleUrls: ['./popupReportGridDetail.component.css'],
})
export class PopupReportGridDetailComponent implements OnInit {
  @Input() public items: ReportShowModel[];
  title: string = '';
  leaveDay: string = '';
  constructor(private modelService: BsModalService) {}
  ngOnInit() {}
}
