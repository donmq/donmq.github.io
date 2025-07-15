import { Component, OnInit } from '@angular/core';
import { Router, NavigationEnd } from '@angular/router';
declare var jQuery: any;

@Component({
  // tslint:disable-next-line
  selector: 'body',
  template: `
    <router-outlet></router-outlet>
    <ngx-spinner bdColor="rgba(0, 0, 0, 0.8)" size="medium" color="#fff" type="ball-clip-rotate" [fullScreen]="true">
    </ngx-spinner>`
})
export class AppComponent implements OnInit {
  constructor(private router: Router) { }

  ngOnInit() {
    this.router.events.subscribe((evt) => {
      if (!(evt instanceof NavigationEnd)) {
        return;
      }
      window.scrollTo(0, 0);
    });

    // Select2 Bootstrap Theme
    (function ($) {
      $(document).ready(function () {
        $.fn.select2.defaults.set('theme', 'bootstrap');
      });
    })(jQuery);
  }
}
