import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { User } from '../../../../_core/_models/user';
import { UserRoles } from '../../../../_core/_models/userRoles';
import { InventoryService } from '../../../../_core/_services/inventory.service';

@Component({
  selector: 'app-inventory-options',
  templateUrl: './inventory-options.component.html',
  styleUrls: ['./inventory-options.component.scss']
})
export class InventoryOptionsComponent implements OnInit, OnDestroy {

  numActive: number;
  dataUser: User;
  checkRoles: number[] = [];
  listUserRoles: UserRoles[] = [];

  @Input() optonsLine: number;
  @Input() optonsBuilding: number;
  @Output() checkOption = new EventEmitter<number>();

  constructor(
    private _inventoryService: InventoryService,
  ) { }

  ngOnDestroy(): void {
    this._inventoryService.optionsInventory.next(0);
  }

  ngOnInit() {
    this.getUserRoles();
    const check = +localStorage.getItem('CheckInventory');
    this.numActive = this.optonsLine === undefined ? check : this.optonsLine;
    localStorage.removeItem('CheckInventory');
  }

  getUserRoles() {
    const user = JSON.parse(localStorage.getItem('user'));
    const listRoles = user.listRoles.filter(x => x.roles);
    listRoles.map(i => this.checkRoles.push(i.roles));
  }

  changeOptionInvetory(options: number): void {
    this.numActive = options;
    this.checkOption.emit(options);
  }
}
