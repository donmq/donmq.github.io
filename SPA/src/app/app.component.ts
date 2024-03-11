import { Component } from '@angular/core';

@Component({
  selector: 'body',
  template: `<router-outlet></router-outlet>`,
  styleUrl: './app.component.scss',
  // providers: [IconSetService],
})
export class AppComponent {
  title = 'SPA';
}
