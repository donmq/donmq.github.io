import { Component, HostBinding, ViewChild, Injectable, ElementRef, OnInit } from '@angular/core';
import { animate, style, transition, trigger, animateChild, query } from '@angular/animations';
import { ViewerContext } from '../viewer-context';
import { ViewerContainer } from '../viewer-model';

@Injectable()
@Component({
  selector: 'viewer-container',
  standalone: false,
  templateUrl: './viewer-container.component.html',
  styleUrls: ['./viewer-container.component.scss'],
  animations: [
    trigger('host', [
      transition(':leave', [
        query('@backdrop,@box', [
          animateChild()
        ])
      ]),
      transition(':enter', [
        query('@backdrop,@box', [
          animateChild()
        ])
      ]),
    ]),
    trigger('box', [
      transition(':leave', [
        style({
          transform: 'scale(1)'
        }),
        animate('100ms ease-out', style({
          transform: 'scale(1.2)'
        })),
        animate('300ms ease-in', style({
          transform: 'scale(0)'
        }))
      ])
    ]),
    trigger('backdrop', [
      transition(':leave', [
        style({
          opacity: 1,
        }),
        animate('230ms ease-in', style({
          opacity: 0,
        }))
      ]),
      transition(':enter', [
        style({
          opacity: 0,
        }),
        animate('230ms ease-in', style({
          opacity: 1,
        }))
      ]),
    ])
  ]
})
export class ViewerContainerComponent implements ViewerContainer, OnInit {
  @HostBinding('@host') host: any;
  @ViewChild('divRef') divRef: ElementRef;
  image: HTMLImageElement
  context: ViewerContext;
  screenWidth: number;
  screenHeight: number;
  imageWidth: number;
  imageHeight: number;
  initialised = false
  constructor() {  }
  ngOnInit(): void {
    this.imageHeight = this.image.height
    this.imageWidth = this.image.width
    const ratio = this.imageHeight / this.imageWidth
    if (this.imageWidth>= this.screenWidth) {
      this.imageWidth = this.screenWidth * 0.8
    }
    this.imageHeight = this.imageWidth * ratio
    if (this.imageHeight >= this.screenHeight) {
      this.imageHeight = this.screenHeight * 0.8
    }
    this.imageWidth = this.imageHeight / ratio
  }
  close() {
    this.context.resolve()
  }
}
