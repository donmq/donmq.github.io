import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { DataHistoryInventory } from '../../../../_core/_models/data-history-inventory';

@Component({
  selector: 'app-history-inventory-view',
  templateUrl: './history-inventory-view.component.html',
  styleUrls: ['./history-inventory-view.component.scss']
})
export class HistoryInventoryViewComponent implements OnInit {
  @Input() dataHistoryInventorys: DataHistoryInventory;
  @Output() listView = new EventEmitter<boolean>();
  constructor() { }

  ngOnInit() {
  }

  backList() {
    this.listView.emit(false);
  }
}
