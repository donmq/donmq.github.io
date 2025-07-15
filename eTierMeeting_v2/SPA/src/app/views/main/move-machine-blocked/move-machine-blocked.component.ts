import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { DateInventoryService } from '../../../_core/_services/date-inventory.service';

@Component({
  selector: 'app-move-machine-blocked',
  templateUrl: './move-machine-blocked.component.html',
  styleUrls: ['./move-machine-blocked.component.scss']
})
export class MoveMachineBlockedComponent implements OnInit {
  time: number;
  dateTime: Date;
  hours: Number;
  minutes: Number;
  seconds: Number;
  countDownDate: number;
  constructor(
    private _rotuer: Router,
    private dateInventoryService: DateInventoryService,
  ) { }

  ngOnInit() {
    this.checkTime();
    this.time = setInterval(() => {
      const now = new Date().getTime();
      const distance = this.countDownDate - now;
      // If the count down is over, write some text
      if (distance <= 0) {
        clearInterval(this.time);
        //this._rotuer.navigateByUrl('/move');
      }
      else {
        const days = Math.floor(distance / (1000 * 60 * 60 * 24));
        this.hours = Math.floor((days * 24) + (distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        this.minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        this.seconds = Math.floor((distance % (1000 * 60)) / 1000);
      }
    });
  }

  checkTime() {
    this.dateInventoryService.checkScheduleInventory().subscribe(res => {
      if (!res.item1) { }
      this.dateTime = res.item2;
      this.countDownDate = new Date(this.dateTime).getTime();
    });
  }
}
