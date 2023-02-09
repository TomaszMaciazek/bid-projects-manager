import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { EditReportComponent } from './components/edit-report/edit-report.component';
import { GenerateReportComponent } from './components/generate-report/generate-report.component';
import { ReportsComponent } from './components/reports/reports.component';

const routes: Routes = [
  { 
    path: '', component: ReportsComponent, canActivate: [AuthGuard]
  },
  { 
    path: 'list', component: ReportsComponent, canActivate: [AuthGuard]
  },
  { 
    path: 'edit', component: EditReportComponent, canActivate: [AuthGuard]
  },
  { 
    path: 'generate', component: GenerateReportComponent, canActivate: [AuthGuard]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportRoutingModule { }
