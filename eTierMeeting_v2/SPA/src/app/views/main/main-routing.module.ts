import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { UserGuard } from '../../_core/_gurad/user.guard';
import { ErrorComponent } from './error/error.component';
import { HistoryMoveComponent } from './history-move/history-move.component';
import { MoveMachineBlockedComponent } from './move-machine-blocked/move-machine-blocked.component';
import { MoveComponent } from './move/move.component';
import { ReportMachineListComponent } from './report-machine/report-machine-list/report-machine-list.component';
import { UploadDataHpA15Component } from './upload-data-hp-a15/upload-data-hp-a15.component';
import { SpecialRoleGuard } from '../../_core/_gurad/special-role.guard';
import { Error403Component } from './error403/error403.component';

const routes: Routes = [
  {
    path: 'home',
    loadChildren: () => import('./dashboard/dashboard.module').then(m => m.DashboardModule),
    data: {
      title: 'Home'
    }
  },
  {
    path: '',
    data: {
      title: 'Main'
    },
    children: [
      {
        path: 'move',
        component: MoveComponent,
        data: {
          title: 'Move'
        },
        canActivate: [UserGuard]
      },
      {
        path: 'inventoryv2',
        data: {
          title: 'InventoryV2'
        },
        canActivate: [UserGuard],
        children: [
          {
            path: '',
            loadChildren: () => import('./inventory/inventory.module').then(m => m.InventoryModule)
          }
        ]
      },
      {
        path: 'historyinventory',
        data: {
          title: 'HistoryInventory'
        },
        children: [
          {
            path: '',
            loadChildren: () => import('./history-inventory/history-inventory.module').then(m => m.HistoryInventoryModule)
          }
        ]
      },
      {
        path: 'checkmachine',
        data: {
          title: 'CheckMachine'
        },
        canActivate: [UserGuard],
        children: [
          {
            path: '',
            loadChildren: () => import('./check-machine/check-machine.module').then(m => m.CheckMachineModule)
          }
        ]
      },
      {
        path: 'checkmachinesafety',
        data: {
          title: 'Check Machine Safety'
        },
        canActivate: [SpecialRoleGuard],
        children: [
          {
            path: '',
            loadChildren: () => import('./check-machine-safety/check-machine-safety.module').then(m => m.CheckMachineSafetyModule)
          }
        ]
      },
      {
        path: 'historycheckmachine',
        data: {
          title: 'HistoryCheckMachine'
        },
        children: [
          {
            path: '',
            loadChildren: () => import('./history-check-machine/history-check-machine.module').then(m => m.HistoryCheckMachineModule)
          }
        ]
      },
      {
        path: 'assetslendmaintain',
        data: {
          title: 'AssetsLendMaintain'
        },
        // canActivate: [SpecialRoleGuard],
        children: [
          {
            path: '',
            loadChildren: () => import('./assets-lend-maintain/assets-lend-maintain.module').then(m => m.AssetsLendMaintainModule)
          }
        ]
      },
      {
        path: 'reportinventory',
        data: {
          title: 'ReportInventory'
        },
        children: [
          {
            path: '',
            loadChildren: () => import('./report-inventory/report-inventory.module').then(m => m.ReportInventoryModule)
          }
        ]
      },
      {
        path: 'historymove',
        component: HistoryMoveComponent,
        data: {
          title: 'HistoryMove'
        }
      },
      {
        path: 'reportmachine',
        component: ReportMachineListComponent,
        data: {
          title: 'ReportMachine'
        }
      },
      {
        path: 'reportcheckmachinesafety',
        data: {
          title: 'Check Machine Safety Report'
        },
        // canActivate: [SpecialRoleGuard],
        children: [
          {
            path: '',
            loadChildren: () => import('./report-check-machine-safety/report-check-machine-safety.module').then(m => m.ReportCheckMachineSafetyModule)
          }
        ]
      },
      {
        path: 'upload-data-hp-a15',
        component: UploadDataHpA15Component,
        data: {
          title: 'Upload Data Excel'
        }
      },
      {
        path: 'error/:url',
        component: ErrorComponent,
      },
      {
        path: 'error-403',
        component: Error403Component,
      },
      {
        path: 'Error/MoveMachineBlocked',
        component: MoveMachineBlockedComponent,
      }
    ]
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MainRoutingModule { }
