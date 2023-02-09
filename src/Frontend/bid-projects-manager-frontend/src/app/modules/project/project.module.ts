import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProjectRoutingModule } from './project-routing.module';
import { SharedModule } from '../shared/shared.module';
import { ProjectsComponent } from './components/projects/projects.component';
import { EditProjectComponent } from './components/edit-project/edit-project.component';
import { TimelineModule } from 'primeng/timeline';
import { MAT_DATE_LOCALE } from '@angular/material/core';


@NgModule({
  providers: [
    {provide: MAT_DATE_LOCALE, useValue: 'en-GB'}
  ],
  declarations: [
    ProjectsComponent,
    EditProjectComponent
  ],
  imports: [
    CommonModule,
    ProjectRoutingModule,
    SharedModule,
    TimelineModule
  ]
})
export class ProjectModule { }
