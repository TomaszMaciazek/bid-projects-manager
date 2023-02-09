import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BasicLayoutComponent } from './common/basic-layout/basic-layout.component';
import { AdminGuard } from './guards/admin.guard';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  {
    path: '', redirectTo: 'projects', pathMatch: 'full'
  },
  {
    path: 'projects', component: BasicLayoutComponent, canActivate: [AuthGuard],
    loadChildren: () => import('./modules/project/project.module').then(m => m.ProjectModule)
  },
  {
    path: 'countries', component: BasicLayoutComponent, canActivate: [AdminGuard],
    loadChildren: () => import('./modules/country/country.module').then(m => m.CountryModule)
  },
  {
    path: 'currencies', component: BasicLayoutComponent, canActivate: [AdminGuard],
    loadChildren: () => import('./modules/currency/currency.module').then(m => m.CurrencyModule)
  },
  {
    path: 'reports', component: BasicLayoutComponent, canActivate: [AuthGuard],
    loadChildren: () => import('./modules/report/report.module').then(m => m.ReportModule)
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
