import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReportRoutingModule } from './report-routing.module';
import { ReportsComponent } from './components/reports/reports.component';
import { EditReportComponent } from './components/edit-report/edit-report.component';
import { GenerateReportComponent } from './components/generate-report/generate-report.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    ReportsComponent,
    EditReportComponent,
    GenerateReportComponent
  ],
  imports: [
    CommonModule,
    ReportRoutingModule,
    SharedModule
  ]
})
export class ReportModule { }
