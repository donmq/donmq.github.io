import { Component, OnDestroy, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { interval, Subscription } from 'rxjs';
import { map, take } from 'rxjs/operators';

@Component({
  selector: 'app-error',
  templateUrl: './error.component.html',
  styleUrls: ['./error.component.scss']
})
export class ErrorComponent implements OnInit, OnDestroy {
  time: number;
  time$: Subscription;
  constructor(
    private _rotuer: Router) { }

  ngOnDestroy(): void {
    this.time$.unsubscribe();
  }

  ngOnInit() {
    this.time$ = interval(1000).pipe(
      map(data => data = 60 - data),
      take(61)
    ).subscribe((res: number) => {
      this.time = res;
      if (this.time === 0) {
        this._rotuer.navigateByUrl("/home");
      }
    });
  }
}
