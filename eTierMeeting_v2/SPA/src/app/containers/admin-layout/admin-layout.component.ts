import { Component, OnInit } from '@angular/core';
import { INavData } from '@coreui/angular';
import { TranslateService } from '@ngx-translate/core';
import { User } from '../../_core/_models/user';
import { WebService } from '../../_core/_services/web.service';
import { navAdmin } from '../../_navAdmin';
import { navKey } from '../default-layout/navKey';

@Component({
  selector: 'app-admin-layout',
  templateUrl: './admin-layout.component.html',
  styleUrls: ['./admin-layout.component.scss']
})
export class AdminLayoutComponent implements OnInit {
  public navItems: INavData[] = [];
  tab: any = 'tab1';
  local: string = localStorage.getItem('local');
  factoryName: string = localStorage.getItem('factory');

  dataUser: User;
  public sidebarMinimized = false;

  constructor(
    public translate: TranslateService,
    private webService: WebService
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


    this.translate.get(navKey.admin).subscribe(res => {
      let listValue = [];
      listValue = Object.values(res);
      this.navItems = navAdmin.map((item, index) => {
        item.name = listValue[index];
        return item;
      });
    });

    this.dataUser = JSON.parse(localStorage.getItem('user'));
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
}
