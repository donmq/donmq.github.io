import { Component, OnInit, ViewChild } from "@angular/core";
import { Route, Router } from "@angular/router";
import { ModalDirective } from "ngx-bootstrap/modal";
import { NgxSpinnerService } from "ngx-spinner";
import { AuthService } from "../../_core/_services/auth.service";
import { CommonService } from "../../_core/_services/common.service";
import { NgSnotifyService } from "../../_core/_services/ng-snotify.service";
import { UserService } from "../../_core/_services/user.service";
import { RecordMeetingDurationService } from "../../_core/_services/record-meeting-duration.service";
import { NavItem, navETierMeeting } from "../../_nav";
import { eTM_Meeting_Log } from "../../_core/_models/eTM_Meeting_Log";
import { environment } from "../../../environments/environment";
import { LocalStorageConstants } from "@constants/storage.constants";
import { OperationResult } from "@utilities/operation-result";

@Component({
  selector: "app-dashboard",
  templateUrl: "./default-layout.component.html",
  styleUrls: ['./default-layout.component.css']
})
export class DefaultLayoutComponent implements OnInit {
  // public sidebarMinimized = false;
  defaultNav = navETierMeeting;
  public navItems = [];
  routeT5: string = ''
  currentUser: any = JSON.parse(localStorage.getItem(LocalStorageConstants.USER));
  oldPassword: string;
  newPassword: string;
  confirmPassword: string;
  @ViewChild('modalChangePassword', { static: false }) modalEditUser: ModalDirective;
  isFromMES: boolean = false;
  isFromT2Selection: boolean = false;
  // toggleMinimize(e) {
  //   this.sidebarMinimized = e;
  // }
  mettingLog: eTM_Meeting_Log = <eTM_Meeting_Log>{};
  mesKanbanUrl: string = '';

  constructor(
    private authService: AuthService,
    private router: Router,
    private userService: UserService,
    private spinnerService: NgxSpinnerService,
    private snotifyService: NgSnotifyService,
    private commonService: CommonService,
    private recordMeetingDurationService: RecordMeetingDurationService,
    private nav: NavItem) { }

  ngOnInit(): void {
    this.initializeAsyncData();
    this.recordMeetingDurationService.t2StartRecordEvent
      .subscribe((deptId) => {
        this.startT2Meeting(deptId);
      });
  }

  async initializeAsyncData() {
    //dang nhap roi moi getRouteT5 ext
    if (this.currentUser && this.currentUser.role.includes("em.T5Efficiency"))
      await this.getRouteT5()
    this.navItems = this.nav.getNavETierMeeting(this.currentUser, this.routeT5)
  }
  getRouteT5 = () => {
    return new Promise<void>((resolve, reject) => {
      this.commonService.getRouteT5().subscribe({
        next: (res) => res.success && res.data.value != null
          ? resolve(this.routeT5 = res.data.value)
          : resolve(this.snotifyService.error(res.message, 'Error!')),
        error: () => reject(this.snotifyService.error('An error occurred while connecting to the server', 'Error!'))
      });
    })
  };

  logout() {
    localStorage.removeItem(LocalStorageConstants.TOKEN);
    localStorage.removeItem(LocalStorageConstants.USER);
    this.authService.decodedToken = null;
    this.authService.currentUser = null;
    this.currentUser = null;
    this.navItems = this.defaultNav;
    this.snotifyService.info('Logged out');
    if (this.router.url.includes('T5')) this.router.navigate(['/dashboard']);
  }

  changePassword() {
    if (this.newPassword !== this.confirmPassword) {
      this.snotifyService.error('Confirm password not match!');
      return;
    }
    this.spinnerService.show();
    this.userService.changePassword(this.currentUser.username, this.oldPassword, this.newPassword)
      .subscribe(res => {
        this.spinnerService.hide();
        if (res.success) {
          this.snotifyService.success(res.message);
          this.modalEditUser.hide();
        }
        else {
          this.snotifyService.error(res.message);
        }
      }, () => {
        this.snotifyService.error('Fail change pasword user!');
        this.spinnerService.hide();
      });
  }

  goToLayout2() {
    if (this.currentUser != null)
      this.router.navigate(['/dashboard2']);
    else
      this.router.navigate(['/login']);
  }

  async checkFromMES() {
    if (window.location.href.includes('linkFrom=MES')) {
      this.isFromMES = true;
      let classType = this.getClassType();
      let tierLevel = this.getTierLevel();
      let unitCode = this.getUnitCode();

      await this.createRecordMeetingDuration(unitCode, classType, tierLevel)

      if (this.mettingLog && this.mettingLog.record_ID) {
        if (window.location.href.includes('level=building')) {
          let building = unitCode
          if (classType == "CTB")
            this.mesKanbanUrl = (`${environment.ipKanban}?orgId=${building}&oleave=2`);
          else if (classType == "STF")
            this.mesKanbanUrl = (`${environment.ipKanbanSTF}?orgId=${building}&level=2`);
          else if (classType == "UPF")
            this.mesKanbanUrl = (`${environment.ipKanbanGQT}?orgId=${building}&oleave=2`);
          else
            this.mesKanbanUrl = (`${environment.ipKanbanSTF}?orgId=${building}&level=2`);
        }
        else {
          let deptId = unitCode          
          await this.getLineID(deptId).then((res) => {
            if (res.success) {
              this.mesKanbanUrl = `${environment.ipKanban}&LINE=${res.data.value}`;
            }
            else {
              this.isFromMES = false;
              this.snotifyService.error("Get LineID Failed", 'Error!')
            }
          })
        }
        //Get last record meeting duration data
        this.mettingLog = await this.recordMeetingDurationService.get(this.mettingLog.record_ID).toPromise();
      }
    }
  }

  backToMES() {
    if (Object.keys(this.mettingLog).length == 0)
      return this.snotifyService.error("Cannot return to MES because the record data could not be retrieved.", 'Error!');
    this.recordMeetingDurationService.update(this.mettingLog).subscribe(
      () => window.location.href = this.mesKanbanUrl,
      () => this.snotifyService.error("Update record failed, can not back to MES", 'Error!'));
  }

  /** Example:
   * urlString = #/safety/safetymain/EA9?linkFrom=MES
   * urlSplited[urlSplited.length - 1] = EA9?linkFrom=MES
   * deptId = EA9
  */
  getUnitCode() {
    let urlString = window.location.hash;
    let urlSplited = urlString.split('/');
    return urlSplited[urlSplited.length - 1].split('?')[0];
  }

  getClassType() {
    let urlString = window.location.hash;
    let urlSplited = urlString.split('/');
    return urlSplited[urlSplited.length - 2].split('?')[0];
  }

  getTierLevel() {
    let urlString = window.location.hash;
    let urlSplited = urlString.split('/');
    return urlSplited[urlSplited.length - 5];
  }

  linkClicked(attr: any, url: string) {
    if (attr.tier_Level == "T5") {
      const parts = url.split('/#/');
      const resultUrl = parts[1];
      this.router.navigateByUrl(resultUrl).then(() => {
        location.reload();
      });
    }

    localStorage.setItem(LocalStorageConstants.CENTER_LEVEL, attr.center_Level);
    localStorage.setItem(LocalStorageConstants.TIER_LEVEL, attr.tier_Level);
  }

  startT2Meeting(deptId: string) {
    this.isFromT2Selection = true;
    this.createRecordMeetingDuration(deptId, 'CTB', 'T2')
  }
  endT2Meeting() {
    this.isFromT2Selection = false;
    this.spinnerService.show();
    this.recordMeetingDurationService.update(this.mettingLog).subscribe(() => {
      this.router.navigateByUrl('/Production/T2/selectline/selectlinemain');
      this.spinnerService.hide();
    });
  }
  createRecordMeetingDuration(unitCode: string, classType: string, tierLevel: string) {
    return new Promise<void>((resolve, reject) => {
      this.spinnerService.show()
      this.recordMeetingDurationService.create(unitCode, classType, tierLevel).subscribe({
        next: (res) => {
          this.spinnerService.hide()
          if (res.success) {
            this.mettingLog = res.data;
            resolve()
          } else {
            this.mettingLog = <eTM_Meeting_Log>{};
            resolve(this.snotifyService.error(res.message, 'Error!'))
          }
        },
        error: () => {
          this.spinnerService.hide()
          this.mettingLog = <eTM_Meeting_Log>{};
          reject(this.snotifyService.error('An error occurred while connecting to the server', 'Error!'))
        }
      });
    })
  }
  getLineID(deptId: string) {
    return new Promise<OperationResult>((resolve, reject) => {
      this.commonService.getLineID(deptId).subscribe({
        next: (res) => resolve(res),
        error: () => reject(this.snotifyService.error('An error occurred while connecting to the server', 'Error!'))
      });
    })
  }
}
