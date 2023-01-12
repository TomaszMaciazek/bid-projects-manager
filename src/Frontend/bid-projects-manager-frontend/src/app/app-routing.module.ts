import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BasicLayoutComponent } from './common/basic-layout/basic-layout.component';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  {
    path: '', redirectTo: 'projects', pathMatch: 'full'
  },
  // {
  //   path: 'projects', component: ProjectsComponent//, canActivate: [AuthGuard]
  // },
  {
    path: 'projects', component: BasicLayoutComponent,
    loadChildren: () => import('./modules/project/project.module').then(m => m.ProjectModule)
  },
  {
    path: 'countries', component: BasicLayoutComponent,
    loadChildren: () => import('./modules/country/country.module').then(m => m.CountryModule)
  },
  {
    path: 'currencies', component: BasicLayoutComponent,
    loadChildren: () => import('./modules/currency/currency.module').then(m => m.CurrencyModule)
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
