import { Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { INavData } from '@coreui/angular';
import { Idle } from '@ng-idle/core';
import { TranslateService } from '@ngx-translate/core';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { User } from '../../_core/_models/user';
import { AuthService } from '../../_core/_services/auth.service';
import { SignalrService } from '../../_core/_services/signalr.service';
import { WebService } from '../../_core/_services/web.service';
import { NavItem } from '../../_nav';
import { navKey } from './navKey';

@Component({
  selector: 'app-dashboard',
  templateUrl: './default-layout.component.html',
  styleUrls: ['./default-layout.component.scss'],
})
export class DefaultLayoutComponent implements OnInit {
  @ViewChild('lgModal') modelLogin: ModalDirective;
  showLogin: boolean = true;
  dataUser: User;
  public sidebarMinimized = false;
  public navItems: INavData[] = [];
  time = new Date();
  usersCount: number;
  tab: any = 'tab1';
  local: string = localStorage.getItem('local');
  factoryName: string = localStorage.getItem('factory');

  constructor(
    private _router: Router,
    private _authService: AuthService,
    public translate: TranslateService,
    private webService: WebService,
    private idle: Idle,
    private signalr: SignalrService,
    private nav: NavItem
  ) { }

  ngOnInit(): void {
    const lang = localStorage.getItem('lang');
    if (lang) {
      this.translate.use(lang);
      if (lang === 'zh-TW') {
        this.tab = 'zh-TW';
      } else if (lang === 'en-US') {
        this.tab = 'en-US';
      } else {
        if (this.local !== lang) {
          localStorage.setItem('lang', this.local);
          this.translate.use(lang);
          location.reload();
        }
        this.tab = this.local;
      }
    } else {
      localStorage.setItem('lang', this.local);
      this.tab = this.local;
      this.translate.use(lang);
      location.reload();
      this.webService.setLanguage(this.local);
    }

    this.translate.get(navKey.main_a).subscribe(res => {
      let listValue = [];
      let a = 1;
      listValue = Object.values(res);
      this.navItems = this.nav.getNav().map((item, index) => {
        item.name = a + '. ' + listValue[index];
        a++;
        return item;
      });
    });
    this.translate.get(navKey.main_b).subscribe(res => {
      let listValue = [];
      listValue = Object.values(res);
      let idx = 0;
      this.navItems.map((item, index) => {
        item.children.map((i, n) => {
          i.name = (index + 1).toString() + '.' + (n + 1).toString() + ' ' + listValue[idx];
          idx++;
        });
        return item;
      });
    });

    setInterval(() => {
      this.time = new Date();
    }, 1000);
    if (this._authService.loggedIn()) {
      this.showLogin = false;
      this.dataUser = JSON.parse(localStorage.getItem('user'));
      this._authService.setTime();
    }

    this._authService.onLoggedOut(this.autoLogOut.bind(this));

    this.signalr.usersCount.subscribe((res: number) => {
      this.usersCount = res;
    });
  }

  showLoginChil(value: boolean) {
    this.showLogin = value;
  }

  showUser(value: User) {
    this.dataUser = value;
  }

  switchLang(lang: string) {
    localStorage.setItem('lang', lang);
    this.webService.setLanguage(lang).subscribe(res => {
      location.reload();
    });

    if (lang === 'zh-TW') {
      this.tab = 'zh-TW';
    } else if (lang === 'en-US') {
      this.tab = 'en-US';
    } else {
      this.tab = this.local;
    }

  }

  toggleMinimize(e) {
    this.sidebarMinimized = e;
  }

  autoLogOut() {
    this.idle.stop();
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this._authService.decodedToken = null;
    this.showLogin = true;
  }

  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('user');
    this._authService.decodedToken = null;
    this.showLogin = true;
    this._router.navigate(['/home']);
  }
}
