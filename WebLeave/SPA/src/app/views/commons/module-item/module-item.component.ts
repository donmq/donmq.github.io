import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'module-item',
  templateUrl: './module-item.component.html',
  styleUrls: ['./module-item.component.scss']
})
export class ModuleItemComponent implements OnInit {
  @Input() routerLink: string = '/';
  @Input() imgSrc: string = 'assets/images/leave.jpg';
  @Input() title: string = 'TITLE';
  @Input() badge: number = null;

  constructor() { }

  ngOnInit(): void {
  }

}
