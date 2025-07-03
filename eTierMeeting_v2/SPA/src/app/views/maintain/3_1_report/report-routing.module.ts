import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: 'meeting-audit-report',
    loadChildren: () => import('./meeting-audit-report/meeting-audit-report.module').then(m => m.MeetingAuditReportModule)
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportRoutingModule { }
