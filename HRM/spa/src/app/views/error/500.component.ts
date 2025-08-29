import { Component } from '@angular/core';
import { IconButton } from '@constants/common.constants';
import { InjectBase } from '@utilities/inject-base-app';

@Component({
  selector: 'app-p500',
  templateUrl: '500.component.html'
})
export class P500Component extends InjectBase {
  iconButton = IconButton;
  constructor() { super() }
  reload = () => this.functionUtility.hardReload();
}
