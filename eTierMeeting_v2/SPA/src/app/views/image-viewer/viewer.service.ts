import {
  Injectable,
  Injector,
  ApplicationRef,
  EnvironmentInjector,
  createComponent
} from '@angular/core';

import { ViewerContext } from './viewer-context';
import { ViewerContainerComponent } from './viewer-container/viewer-container.component';

@Injectable()
export class ViewerService {

  constructor(
    private appRef: ApplicationRef,
    private injector: EnvironmentInjector
  ) {
  }


  async open(image: HTMLImageElement, screenWidth: number, screenHeight: number): Promise<any> {
    let _injector = Injector.create(
      { providers: [{ provide: ViewerContext }]
    });
    let container = createComponent(ViewerContainerComponent, {
      environmentInjector: this.injector,
      elementInjector: _injector
    });
    let context = _injector.get(ViewerContext);
    container.instance.context = context;
    container.instance.image = image;
    container.instance.screenWidth = screenWidth;
    container.instance.screenHeight = screenHeight;
    document.body.appendChild(container.location.nativeElement);
    this.appRef.attachView(container.hostView);
    let html = document.getElementsByTagName('html')[0] as HTMLElement;
    html.style.overflowY = 'hidden'
    const result = await context.promise(container);
    html.style.overflowY = 'auto'
    return result
  }
}
