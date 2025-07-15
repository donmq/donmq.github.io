import { Component, Input, OnInit } from '@angular/core';
import { ResultHistoryCheckMachine } from '../../../../_core/_models/export-pdf';
import { HistoryCheckMachine } from '../../../../_core/_models/list-history-check-machine';
import { User } from '../../../../_core/_models/user';
import { CheckMachineService } from '../../../../_core/_services/check-machine.service';

@Component({
  selector: 'app-check-machine-list',
  templateUrl: './check-machine-list.component.html',
  styleUrls: ['./check-machine-list.component.scss']
})
export class CheckMachineListComponent implements OnInit {
  @Input() listResultCheckMachine: ResultHistoryCheckMachine;
  @Input() historyCheckMachine: HistoryCheckMachine;
  @Input() dataReport: any;
  dataUser: User;
  constructor(private _checkMachineService: CheckMachineService) { }

  ngOnInit() {
    this.dataUser = JSON.parse(localStorage.getItem('user'));
  }

  //Export file PDF
  exportPDF() {
    this._checkMachineService.exportPDF(this.dataReport);
  }
}
