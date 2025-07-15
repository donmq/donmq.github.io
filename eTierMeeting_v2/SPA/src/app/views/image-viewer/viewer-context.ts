import { Injectable, ComponentRef} from '@angular/core';
import { ViewerContainerComponent } from './viewer-container/viewer-container.component';

@Injectable()
export class ViewerContext {

  private componentRef: ComponentRef<ViewerContainerComponent>;

  private _resolve: Function;
  private _reject: Function;
  private _promise: Promise<any>;

  constructor() { }

  private hide() {
    this.componentRef.destroy()
  }
  resolve(...args: any[]) {
    this.hide();
    this._resolve(...args);
  }
  reject(reason: any) {
    this.hide();
    this._reject(reason);
  }
  async promise(componentRef: ComponentRef<ViewerContainerComponent>): Promise<any> {
    return this._promise || (this._promise = new Promise((resolve, reject) => {
      this.componentRef = componentRef;
      this._resolve = resolve;
      this._reject = reject;
    }));
  }

}
