import { Component } from '@angular/core';

@Component({
  selector: 'body',
  template: `<router-outlet></router-outlet>
            <ng-snotify></ng-snotify>
            <ng-progress></ng-progress>
            <ngx-spinner bdColor="rgba(51,51,51,0.8)" size="medium" color="#fff" type="ball-scale-multiple"></ngx-spinner>`,
  styleUrl: './app.component.scss',
  // providers: [IconSetService],
})
export class AppComponent {
  title = 'SPA';
}
