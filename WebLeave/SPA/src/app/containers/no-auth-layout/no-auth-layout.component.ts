import { Component, OnDestroy, OnInit } from '@angular/core';
import * as moment from 'moment';
import { LangConstants } from '@constants/lang.constants';
import { UserCounterHubService } from '@services/user-counter-hub.service';
import { LocalStorageConstants } from '@constants/local-storage.enum';
import { Users } from '@models/auth/users.model';
import { AuthService } from '@services/auth/auth.service';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ChangePasswordComponent } from '../change-password/change-password.component';
import { UserForLoginParam } from '@params/auth/user-for-login.param';
import { firstValueFrom } from 'rxjs';
import { InjectBase } from '@utilities/inject-base-app';
declare var particlesJS: any;

@Component({
  selector: 'app-no-auth-layout',
  templateUrl: './no-auth-layout.component.html',
  styleUrls: ['./no-auth-layout.component.scss']
})
export class NoAuthLayout extends InjectBase implements OnInit, OnDestroy {
  bsModalRef?: BsModalRef;
  connectedUsers: number = 0;
  now: string = '';
  requiredLogin: boolean = false
  interval: NodeJS.Timeout;
  user: Users = JSON.parse(localStorage.getItem(LocalStorageConstants.USER));
  lang: string = localStorage.getItem(LocalStorageConstants.LANG);
  factory: string = localStorage.getItem(LocalStorageConstants.FACTORY);
  langConstants: typeof LangConstants = LangConstants;
  constructor(
    private authService: AuthService,
    private userCounterHubService: UserCounterHubService,
    private modalService: BsModalService,) {
    super();
    this.connectedUsers = this.userCounterHubService.connectedUsers;
    this.translateService.addLangs(['vi', 'en', 'zh']);

    if (this.lang) {
      this.translateService.setDefaultLang(this.lang === LangConstants.ZH_TW ? 'zh' : this.lang);
    } else {
      this.translateService.setDefaultLang(LangConstants.VN);
      localStorage.setItem(LocalStorageConstants.LANG, LangConstants.VN);
    }
    this.requiredLogin = !window.location.href.includes('personal-detail');
  }

  ngOnInit(): void {
    this.initParticles();
    this.interval = setInterval(async () => {
      this.now = moment().format('HH:mm:ss');
      if (!this.authService.loggedIn() && this.requiredLogin) {
        clearInterval(this.interval);
        let model: UserForLoginParam = <UserForLoginParam>{
          username: this.user?.username,
          ipLocal: localStorage.getItem(LocalStorageConstants.IPLOCAL)
        }
        await firstValueFrom(this.authService.loginExpired(model));
      }
    }, 1000);
    this.userCounterHubService.connectedUsersEmitter.subscribe(count => this.connectedUsers = count);
  }

  async logout() {
    let model: UserForLoginParam = <UserForLoginParam>{
      username: this.user?.username,
      ipLocal: localStorage.getItem(LocalStorageConstants.IPLOCAL)
    }
    let message = this.authService.loggedIn() ? 'Logout' : '';
    await firstValueFrom(this.authService.logout(model, true, message));
  }

  switchLang(lang: string) {
    this.translateService.use(lang == LangConstants.ZH_TW ? 'zh' : lang);
    localStorage.setItem(LocalStorageConstants.LANG, lang);
  }

  changLang() {
    this.lang = localStorage.getItem(LocalStorageConstants.LANG);
  }

  openChangePasswordModal() {
    this.bsModalRef = this.modalService.show(ChangePasswordComponent);
  }

  ngOnDestroy(): void {
    clearInterval(this.interval);
  }

  initParticles() {
    particlesJS("particles-js", {
      "particles": {
        "number": {
          "value": 40,
          "density": {
            "enable": true,
            "value_area": 700
          }
        },
        "color": {
          "value": "#ffffff"
        },
        "shape": {
          "type": "circle",
          "stroke": {
            "width": 0,
            "color": "#000000"
          },
          "polygon": {
            "nb_sides": 5
          },
        },
        "opacity": {
          "value": 0.5,
          "random": false,
          "anim": {
            "enable": false,
            "speed": 5,
            "opacity_min": 0.1,
            "sync": false
          }
        },
        "size": {
          "value": 1,
          "random": true,
          "anim": {
            "enable": false,
            "speed": 40,
            "size_min": 0.1,
            "sync": false
          }
        },
        "line_linked": {
          "enable": true,
          "distance": 150,
          "color": "#ffffff",
          "opacity": 0.4,
          "width": 1
        },
        "move": {
          "enable": true,
          "speed": 1,
          "direction": "none",
          "random": false,
          "straight": false,
          "out_mode": "out",
          "bounce": false,
          "attract": {
            "enable": false,
            "rotateX": 600,
            "rotateY": 1200
          }
        }
      },
      "interactivity": {
        "detect_on": "canvas",
        "events": {
          "onhover": {
            "enable": true,
            "mode": "grab"
          },
          "onclick": {
            "enable": true,
            "mode": "push"
          },
          "resize": true
        },
        "modes": {
          "grab": {
            "distance": 140,
            "line_linked": {
              "opacity": 1
            }
          },
          "bubble": {
            "distance": 400,
            "size": 40,
            "duration": 2,
            "opacity": 8,
            "speed": 3
          },
          "repulse": {
            "distance": 200,
            "duration": 0.4
          },
          "push": {
            "particles_nb": 4
          },
          "remove": {
            "particles_nb": 2
          }
        }
      },
      "retina_detect": true
    });
  }
}
