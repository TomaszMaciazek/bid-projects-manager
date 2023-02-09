import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/guards/auth.guard';
import { EditProjectComponent } from './components/edit-project/edit-project.component';
import { ProjectsComponent } from './components/projects/projects.component';

const routes: Routes = [
  { 
    path: '', component: ProjectsComponent, canActivate: [AuthGuard]
  },
  { 
    path: 'list', component: ProjectsComponent, canActivate: [AuthGuard]
  },
  { 
    path: 'edit', component: EditProjectComponent, canActivate: [AuthGuard]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProjectRoutingModule { }
