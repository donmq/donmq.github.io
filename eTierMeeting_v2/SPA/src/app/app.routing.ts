import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

// Import Containers
import {
  DefaultLayoutComponent,
  Layout2ForMaintainpageComponent,
} from "./containers";
import { P404Component } from "./views/error/404.component";
import { P500Component } from "./views/error/500.component";
import { LoginComponent } from "./views/login/login.component";
// import { RegisterComponent } from "./views/register/register.component";
import { AuthGuard } from "./auth/auth.guard";
import { PageSettingsGuard } from "./_core/_guards/page-settings.guard";
import { PageInitGuard } from "./_core/_guards/page-init.guard";
import { PageItemSettingGuard } from "./_core/_guards/production/T2/CTB/page-item-setting.guard";
import { EfficiencyGuard } from "./_core/_guards/production/T5/efficiency.guard";

export const routes: Routes = [
  {
    path: "",
    redirectTo: '/dashboard',
    pathMatch: "full",
  },
  {
    path: "404",
    component: P404Component,
    data: {
      title: "Page 404",
    },
  },
  {
    path: "500",
    component: P500Component,
    data: {
      title: "Page 500",
    },
  },
  {
    path: "login",
    component: LoginComponent,
    data: {
      title: "Login Page",
    },
  },
  {
    path: "",
    component: DefaultLayoutComponent,
    data: {
      title: "Home",
    },
    children: [
      {
        path: "dashboard",
        loadChildren: () =>
          import("./views/dashboard/dashboard.module").then((m) => m.DashboardModule),
      },
      /* ------------------------------- Production ------------------------------- */
      {
        path: 'Production',
        data: { center_Level: 'Production' },
        children: [
          /* ----------------------------------- T1 ----------------------------------- */
          {
            path: 'T1',
            data: { tier_Level: 'T1' },
            canActivate: [PageInitGuard],
            children: [
              /* ------------------------------- Select Line ------------------------------ */
              {
                path: "selectline",
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T1/selectline/selectline.module").then((m) => m.SelectlineModule),
              },
              /* ----------------------------------- CTB ---------------------------------- */
              {
                path: "safety",
                data: { class_Level: 'CTB' },
                canActivateChild: [PageSettingsGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T1/C2B/safety/safety.module").then((m) => m.SafetyModule),
              },
              {
                path: "quality",
                data: { class_Level: 'CTB' },
                canActivateChild: [PageSettingsGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T1/C2B/quality/quality.module").then((m) => m.QualityModule),
              },
              {
                path: "delivery",
                data: { class_Level: 'CTB' },
                canActivateChild: [PageSettingsGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T1/C2B/delivery/delivery.module").then((m) => m.DeliveryModule),
              },
              {
                path: "efficiency",
                data: { class_Level: 'CTB' },
                canActivateChild: [PageSettingsGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T1/C2B/efficiency/efficiency.module").then((m) => m.EfficiencyModule),
              },
              {
                path: "kaizen",
                data: { class_Level: 'CTB' },
                canActivateChild: [PageSettingsGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T1/C2B/kaizen/kaizen.module").then((m) => m.KaizenModule),
              },
              {
                path: "modelpreparation",
                data: { class_Level: 'CTB' },
                canActivateChild: [PageSettingsGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T1/C2B/model-preparation/model-preparation.module").then((m) => m.ModelPreparationModule),
              },
              /* ----------------------------------- STF ---------------------------------- */
              {
                path: "safety",
                data: { class_Level: 'STF' },
                canActivateChild: [PageSettingsGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T1/STF/safety/safety.module").then((m) => m.SafetyModule),
              },
              {
                path: "quality",
                data: { class_Level: 'STF' },
                canActivateChild: [PageSettingsGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T1/STF/quality/quality.module").then((m) => m.QualityModule),
              },
              {
                path: "delivery",
                data: { class_Level: 'STF' },
                canActivateChild: [PageSettingsGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T1/STF/delivery/delivery.module").then((m) => m.DeliveryModule),
              },
              {
                path: "efficiency",
                data: { class_Level: 'STF' },
                canActivateChild: [PageSettingsGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T1/STF/efficiency/efficiency.module").then((m) => m.EfficiencyModule),
              },
              {
                path: "kaizen",
                data: { class_Level: 'STF' },
                canActivateChild: [PageSettingsGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T1/STF/kaizen/kaizen.module").then((m) => m.KaizenModule),
              },
              {
                path: "modelpreparation",
                data: { class_Level: 'STF' },
                canActivateChild: [PageSettingsGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T1/STF/model-preparation/model-preparation.module").then((m) => m.ModelPreparationModule),
              },
              /* ----------------------------------- UPF ---------------------------------- */
              {
                path: "kaizen",
                data: { class_Level: 'UPF' },
                canActivateChild: [PageSettingsGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T1/UPF/kaizen/kaizen.module").then((m) => m.KaizenModule)
              },
              {
                path: "modelpreparation",
                data: { class_Level: 'UPF' },
                canActivateChild: [PageSettingsGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T1/UPF/model-preparation/model-preparation.module").then((m) => m.ModelPreparationModule),
              },
              {
                path: "delivery",
                data: { class_Level: 'UPF' },
                canActivateChild: [PageSettingsGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T1/UPF/delivery/delivery.module").then((m) => m.DeliveryModule),
              },
              {
                path: "safety",
                data: { class_Level: 'UPF' },
                canActivateChild: [PageSettingsGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T1/UPF/safety/safety.module").then((m) => m.SafetyModule),
              },
              {
                path: "quality",
                data: { class_Level: 'UPF' },
                canActivateChild: [PageSettingsGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T1/UPF/quality/quality.module").then((m) => m.QualityModule),
              },
              {
                path: "efficiency",
                data: { class_Level: 'UPF' },
                canActivateChild: [PageSettingsGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T1/UPF/efficiency/efficiency.module").then((m) => m.EfficiencyModule),
              }
            ]
          },
          /* ----------------------------------- T2 ----------------------------------- */
          {
            path: 'T2',
            data: { tier_Level: 'T2' },
            canActivate: [PageInitGuard],
            children: [
              /* ------------------------------- Select Line ------------------------------ */
              /* ----------------------------------- CTB ---------------------------------- */
              {
                path: "selectline",
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T2/selectline/selectline.module").then((m) => m.SelectlineModule),
              },
              {
                path: "safety",
                data: { class_Level: 'CTB' },
                canActivateChild: [PageSettingsGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T2/CTB/safety/safety.module").then(m => m.SafetyModule)
              },
              {
                path: "quality",
                data: { class_Level: 'CTB' },
                canActivateChild: [PageSettingsGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T2/CTB/quality/quality.module").then((m) => m.QualityModule),
              },
              {
                path: "efficiency",
                data: { class_Level: 'CTB' },
                canActivateChild: [PageSettingsGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T2/CTB/efficiency/efficiency.module").then((m) => m.EfficiencyModule),
              },
              {
                path: "kaizen",
                data: { class_Level: 'CTB' },
                canActivateChild: [PageSettingsGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T2/CTB/kaizen/kaizen.module").then((m) => m.KaizenModule),
              }
            ]
          },
          {
            path: 'T5',
            data: { tier_Level: 'T5' },
            children: [
              {
                path: "efficiency",
                canLoad: [EfficiencyGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T5/efficiency/efficiency.module").then((m) => m.EfficiencyModule),
              },
              {
                path: "efficiency",
                canLoad: [EfficiencyGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T5/efficiency-ext/efficiency-ext.module").then(m => m.EfficiencyExtModule)
              }
            ]
          },
          {
            path: 'T6',
            data: { tier_Level: 'T6' },
            children: [
              {
                path: "efficiency",
                canLoad: [EfficiencyGuard],
                loadChildren: () =>
                  import("./views/E-Tier-Meeting/production/T6/efficiency/efficiency.module").then((m) => m.EfficiencyModule),
              }
            ]
          }
        ]
      },
    ],
  },
  {
    path: "",
    canActivate: [AuthGuard],
    component: Layout2ForMaintainpageComponent,
    data: {
      title: "Home",
    },
    children: [
      {
        path: "dashboard2",
        loadChildren: () =>
          import("./views/dashboard2/dashboard2.module").then(
            (m) => m.Dashboard2Module
          ),
      },
      {
        // 0.1
        path: "user",
        loadChildren: () =>
          import("./views/maintain/0_1_user_list/user.module").then((m) => m.UserModule),
      },
      {
        // 1.1
        path: "classification",
        loadChildren: () =>
          import(
            "./views/maintain/1_1_deptclassification/deptclassification.module"
          ).then((m) => m.DeptclassificationModule),
      },
      {
        // 1.2
        path: 'page-enable-disable',
        loadChildren: () => import('./views/maintain/1_2_page-enable-disable/page-enable-disable.module')
          .then(m => m.PageEnableDisableModule),
      },
      {
        // 1.3
        path: 'page-item-setting',
        canLoad: [PageItemSettingGuard],
        loadChildren: () => import('./views/maintain/1_3_page-item-setting/page-item-setting.module')
          .then(m => m.PageItemSettingModule),
      },
      {
        // 1.4
        path: 't2-meeting-time-setting',
        loadChildren: () => import('./views/maintain/1_4_t2-meeting-time-setting/t2-metting-time-setting.module')
          .then(m => m.T2MeetingTimeSettingModule),
      },
      {
        // 2.2
        path: "uT1safety",
        loadChildren: () =>
          import(
            "./views/maintain/2_2_upload-t1safety/upload-t1safety.module"
          ).then((m) => m.UploadT1safetyModule),
      },
      {
        // 2.3
        path: 'hse-upload',
        data: { class_Level: 'STF' },
        loadChildren: () =>
          import("./views/maintain/2_3_hse-result-upload/hse-result-upload.module").then(m => m.HseResultUploadModule)
      },
      {
        // 2.4 T5 External Upload
        path: 't5-external-upload',
        loadChildren: () => import('./views/maintain/2_4_t5-external-upload/t5-external-upload.module')
          .then(m => m.T5ExternalUploadModule),
      },
      {
        // 3.1
        path: 'report',
        loadChildren: () =>
          import("./views/maintain/3_1_report/report.module").then(m => m.ReportModule)
      }
    ],
  },
  // { path: '**', component: DefaultLayoutComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
