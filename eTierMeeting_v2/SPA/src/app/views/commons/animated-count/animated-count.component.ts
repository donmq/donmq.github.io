import { AfterViewInit, Component, ElementRef, Input, ViewChild } from '@angular/core';

@Component({
  selector: 'app-animated-count',
  templateUrl: './animated-count.component.html',
  styleUrls: ['./animated-count.component.scss']
})
export class AnimatedCountComponent implements AfterViewInit {
  @Input() duration: number;
  @Input() digit: number;
  @Input() steps: number;
  @ViewChild("animatedDigit") animatedDigit: ElementRef;

  animateCount() {
    if (!this.duration) {
      this.duration = 1000;
    }

    this.counterFunc(this.digit, this.duration, this.animatedDigit);
  }

  counterFunc(endValue, durationMs, element) {
    if (!this.steps) {
      this.steps = 12;
    }

    const stepCount = Math.abs(durationMs / this.steps);
    const valueIncrement = (endValue - 0) / stepCount;
    const sinValueIncrement = Math.PI / stepCount;

    let currentValue = 0;
    let currentSinValue = 0;
    let number = this.digit;

    function step() {
      currentSinValue += sinValueIncrement;
      currentValue += valueIncrement * Math.sin(currentSinValue) ** 2 * 2;

      if (Math.abs(Math.floor(currentValue)) !== Math.floor(number)) {
        element.nativeElement.textContent = Math.abs(Math.floor(currentValue));
      } else {
        element.nativeElement.textContent = number;
      }

      if (currentSinValue < Math.PI) {
        window.requestAnimationFrame(step);
      }
    }

    step();
  }

  ngAfterViewInit() {
    if (this.digit) {
      this.animateCount();
    }
  }
}
