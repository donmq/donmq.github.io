import { Injectable, OnInit } from "@angular/core";
import { INavData } from "@coreui/angular";
import { UserStorage } from "./_core/_models/user";
import { environment } from '../environments/environment';

// for layout1
export const navETierMeeting: INavData[] = [
  {
    name: 'Production',
    children: [
      {
        name: 'T1',
        url: '/#/Production/T1/selectline/selectlinemain',
        attributes: {
          center_Level: 'Production',
          tier_Level: 'T1',
        }
      },
      {
        name: 'T2',
        url: '/#/Production/T2/selectline/selectlinemain',
        attributes: {
          center_Level: 'Production',
          tier_Level: 'T2',
        }
      },
      {
        name: 'T3',
      },
      {
        name: 'T4',
      }
    ]
  },
  {
    name: 'Operation',
    children: [
      {
        name: 'T1',
      },
      {
        name: 'T2',
      },
      {
        name: 'T3',
      },
      {
        name: 'T4',
      },
    ]
  },
  {
    name: 'Administration',
    children: [
      {
        name: 'T1',
      },
      {
        name: 'T2',
      },
      {
        name: 'T3',
      },
      {
        name: 'T4',
      },
    ]
  },
  {
    name: 'Development',
    children: [
      {
        name: 'T1',
      },
      {
        name: 'T2',
      },
      {
        name: 'T3',
      },
      {
        name: 'T4',
      },
    ]
  },
  {
    name: 'Kaizen',
    href: environment.ipKaizen
  }
];

@Injectable({
  providedIn: "root", // <- ADD THIS
})

// for layout2
export class NavItem {
  navItems: INavData[] = [];
  hasSetting: boolean;
  hasMaintain: boolean;
  hasTransaction: boolean;
  hasKanban: boolean;
  hasReport: boolean;
  hasQuery: boolean;
  constructor() { }


  getNavETierMeeting(user: UserStorage, routeT5: string) {
    let navETierMeetingTemp = JSON.parse(JSON.stringify(navETierMeeting));

    if (user && user.role.includes("em.T5Efficiency")) {
      const navT5 = {
        name: 'T5',
        url: `/#/Production/T5${routeT5}`,
        attributes: {
          center_Level: 'Production',
          tier_Level: 'T5',
        }
      };
      const navProduction = navETierMeetingTemp.find(x => x.name == 'Production')

      navProduction.children.push(navT5);
    }

    return navETierMeetingTemp;
  }

  getNav(user: any) {
    if (user == null) return [];

    this.navItems = [];
    this.hasSetting = false;
    this.hasMaintain = false;
    this.hasTransaction = false;
    this.hasKanban = false;
    this.hasReport = false;
    this.hasQuery = false;

    // 左菜單父節點
    const navItemSettings = {
      name: "0. Settings",
      url: "settings",
      icon: "fa fa-cogs",
      children: [],
    };
    const navItemMaintain = {
      name: "1. Maintain",
      url: "maintain",
      icon: "icon-list",
      children: [],
    };
    const navItemTransaction = {
      name: "2. Transaction",
      url: "transaction",
      icon: "icon-pie-chart",
      children: [],
    };
    const navItemKanban = {
      name: "3. Kanban",
      url: "kanban",
      icon: "fa fa-line-chart",
      children: [],
    };
    const navItemReport = {
      name: "3. Report",
      url: "report",
      icon: "icon-chart",
      children: [],
    };
    const navItemQuery = {
      name: "5. Query",
      url: "query",
      icon: "fa fa-search",
      children: [],
    };
    // 左菜單子節點
    if (user != null) {
      user.role.forEach((element) => {
        // 0. Settings
        if (element === "em.UserList") {
          const children = {
            name: "0.1 User List",
            url: "/user",
            class: "menu-margin",
          };
          this.hasSetting = true;
          navItemSettings.children.push(children);
        }
        // 1. Maintain
        if (element === "em.Classification") {
          const children = {
            name: "1.1 Classification",
            url: "/classification",
            class: "menu-margin",
          };
          this.hasMaintain = true;
          navItemMaintain.children.push(children);
        }
        if (element === "em.PageEnableDisable") {
          const children = {
            name: "1.2 Page Enable Disable",
            url: "/page-enable-disable",
            class: "menu-margin",
          };
          this.hasMaintain = true;
          navItemMaintain.children.push(children);
        }
        if (element === "em.PageItemSetting") {
          const children = {
            name: "1.3 Page Item Setting",
            url: "/page-item-setting",
            class: "menu-margin",
          };
          this.hasMaintain = true;
          navItemMaintain.children.push(children);
        }
        if (element === "em.T2MeetingTimeSetting") {
          const children = {
            name: "1.4 T2 Meeting Time Setting",
            url: "/t2-meeting-time-setting",
            class: "menu-margin",
          };
          this.hasMaintain = true;
          navItemMaintain.children.push(children);
        }
        // 2. Transaction
        if (element === "em.UploadT1") {
          const children = {
            name: "2.2 Upload Video",
            url: "/uT1safety",
            class: "menu-margin",
          };
          this.hasTransaction = true;
          navItemTransaction.children.push(children);
        }
        if (element === "em.HSEResultUpload") {
          const children = {
            name: "2.3 HSE Result Upload",
            url: "/hse-upload",
            class: "menu-margin",
          }
          this.hasTransaction = true;
          navItemTransaction.children.push(children);
        }
        // T5 External Upload
        if (element === "em.HSEResultUpload") {
          const children = {
            name: "2.4 T5 External Upload",
            url: "/t5-external-upload",
            class: "menu-margin",
          }
          this.hasTransaction = true;
          navItemTransaction.children.push(children);
        }
        //// 3. Kanban
        // if (element === "") {
        //   const children = {
        //     name: "",
        //     url: "",
        //     class: "menu-margin",
        //   };
        //   this.hasKanban = true;
        //   navItemKanban.children.push(children);
        // }
        // 4. Report
        if (element === "em.T2MeetingAuditReport") {
          const children = {
            name: "3.1 Meeting Audit Report",
            url: "/report/meeting-audit-report",
            class: "menu-margin",
          };
          this.hasReport = true;
          navItemReport.children.push(children);
        }
        //// 5. Query
        // if (element === "") {
        //   const children = {
        //     name: "",
        //     url: "",
        //     class: "menu-margin",
        //   };
        //   this.hasQuery = true;
        //   navItemQuery.children.push(children);
        // }
      });
    }

    // 若有子節點,顯示父節點
    if (this.hasSetting) {
      this.navItems.push(navItemSettings);
    }
    if (this.hasMaintain) {
      this.navItems.push(navItemMaintain);
    }
    if (this.hasTransaction) {
      this.navItems.push(navItemTransaction);
    }
    if (this.hasKanban) {
      this.navItems.push(navItemKanban);
    }
    if (this.hasReport) {
      this.navItems.push(navItemReport);
    }
    if (this.hasQuery) {
      this.navItems.push(navItemQuery);
    }

    return this.navItems;
  }
}
