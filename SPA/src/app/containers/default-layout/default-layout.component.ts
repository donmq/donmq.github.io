import { routes } from './../../app.routing';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { INavData } from '@coreui/angular';
import moment from 'moment';

@Component({
  selector: 'app-default-layout',
  templateUrl: './default-layout.component.html',
  styleUrl: './default-layout.component.scss',
  // providers: [DestroyService]
})
export class DefaultLayoutComponent implements OnInit, OnDestroy {
  public sidebarMinimized = false;
  public navItems: INavData[] = [
    { name: 'Trang chủ', url: '/home' },
    { name: 'Giới thiệu', url: '/about' },
    { name: 'Lý lịch', url: '/profile' },
    { name: 'Liên hệ', url: '/contact' },
  ];
  now: string = '';
  interval: NodeJS.Timeout;
  user: any = {
    fullName: 'demo'
  }
  ngOnInit() {
    this.now = moment().format('HH:mm:ss');
    this.interval = setInterval(() => {
      this.now = moment().format('HH:mm:ss');
    }, 1000);

  }
  ngOnDestroy(): void {
    clearInterval(this.interval);
  }

  toggleMinimize(e) {
    this.sidebarMinimized = e;
  }
}
