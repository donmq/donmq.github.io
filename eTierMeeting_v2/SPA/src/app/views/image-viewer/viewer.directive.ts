import { Directive, ElementRef, HostListener, Renderer2 } from '@angular/core';
import { ViewerService } from './viewer.service';

@Directive({
  standalone: false,
  selector: '[imageViewer]'
})
export class ViewerDirective {
  @HostListener('click', ['$event']) async onClick($event: any) {
    const imageUrl = $event.currentTarget.src
    const screenWidth = $event.view.innerWidth
    const screenHeight = $event.view.innerHeight
    let img = await this.processImage(imageUrl) as HTMLImageElement
    await this.viewerService.open(
      img,
      screenWidth,
      screenHeight
    )
  }
  @HostListener('mouseenter') onMouseEnter() {
    this.renderer2.setStyle(this.elRef.nativeElement, 'cursor', 'pointer')
  }
  @HostListener('mouseout') onMouseOut() {
    this.renderer2.setStyle(this.elRef.nativeElement, 'cursor', 'default')
  }
  constructor(
    private elRef: ElementRef,
    private renderer2: Renderer2,
    private viewerService: ViewerService
  ) { }
  processImage(url: string) {
    return new Promise((resolve, reject) => {
      let img = new Image()
      img.onload = () => resolve(img)
      img.onerror = reject
      img.src = url
    })
  }
}
