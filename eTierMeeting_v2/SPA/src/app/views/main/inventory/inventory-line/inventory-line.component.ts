import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Hpa15 } from '../../../../_core/_models/hp-a15';
import { InventoryLine } from '../../../../_core/_models/inventoryLine';
import { ListHpa15 } from '../../../../_core/_models/preliminary-list';
import { InventoryService } from '../../../../_core/_services/inventory.service';

@Component({
  selector: 'app-inventory-line',
  templateUrl: './inventory-line.component.html',
  styleUrls: ['./inventory-line.component.scss']
})
export class InventoryLineComponent implements OnInit {
  nameCode: string;
  listLine: InventoryLine[];
  listPlno: ListHpa15[] = [];
  optonsLine: number;

  constructor(
    private _inventoryService: InventoryService,
    private _router: Router,
    private _activatedRoute: ActivatedRoute
  ) { }

  ngOnInit() {
    this._inventoryService.codeName.asObservable().subscribe(res => this.nameCode = res);
    this._inventoryService.listPlno.asObservable().subscribe(res => {
      this.listLine = this._activatedRoute.snapshot.data['lineInventoy'];
      if (res) {
        this.listPlno = res;
        const listLineAfter = [];
        this.listPlno.map(m => {
          if (this.listLine.find(i => i.plnoId === m.plno) !== undefined) {
            listLineAfter.push(this.listLine.find(i => i.plnoId === m.plno));
          }
        });
        this.listLine = listLineAfter;
      }
    });

    // get data in url
    this._activatedRoute.paramMap.subscribe(res => {
      const optonsUrl = res.get('optons');
      this.optonsLine = +optonsUrl;
    });

    if (this.nameCode === '') {
      this._router.navigateByUrl('/inventoryv2');
    }
  }

  checkOption(value: number) {
    this.optonsLine = value;
  }

  redirectToInventory(item: InventoryLine): void {
    this._inventoryService.getPositionInventory(item);
    this._router.navigate(['inventoryv2/inventory-home', this.optonsLine]);
  }

  back(options: number): void {
    this._router.navigate(['inventoryv2/']);
    localStorage.setItem('CheckInventory', JSON.stringify(options));
  }
}
