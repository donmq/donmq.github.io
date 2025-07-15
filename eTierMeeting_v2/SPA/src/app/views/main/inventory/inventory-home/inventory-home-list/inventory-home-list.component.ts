import { Component, Input, OnInit } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { User } from '../../../../../_core/_models/user';
import { InventoryService } from '../../../../../_core/_services/inventory.service';

@Component({
  selector: 'app-inventory-home-list',
  templateUrl: './inventory-home-list.component.html',
  styleUrls: ['./inventory-home-list.component.scss']
})
export class InventoryHomeListComponent implements OnInit {
  dataUser: User;
  @Input() listReport: any;

  constructor(
    private _inventoryService: InventoryService,
    private _spinnerService: NgxSpinnerService
  ) { }

  ngOnInit() {
    this.dataUser = JSON.parse(localStorage.getItem('user'));
  }

  exportPDF() {
    this.listReport.typeFile = 'pdf';
    this._inventoryService.exportPDF(this.listReport);
  }

  exportExcel() {
    this.listReport.typeFile = 'excel';
    this._inventoryService.exportExcel(this.listReport);
  }
}
