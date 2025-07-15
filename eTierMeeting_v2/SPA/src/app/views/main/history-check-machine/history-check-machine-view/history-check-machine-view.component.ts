import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { HistoryCheckMachine } from '../../../../_core/_models/list-history-check-machine';

@Component({
  selector: 'app-history-check-machine-view',
  templateUrl: './history-check-machine-view.component.html',
  styleUrls: ['./history-check-machine-view.component.scss']
})
export class HistoryCheckMachineViewComponent implements OnInit {
  @Input() listViewCheckMachineHisstory: HistoryCheckMachine;
  @Output() listView = new EventEmitter<boolean>();
  constructor() { }

  ngOnInit() {
  }
  backList() {
    this.listView.emit(false);
  }
}
