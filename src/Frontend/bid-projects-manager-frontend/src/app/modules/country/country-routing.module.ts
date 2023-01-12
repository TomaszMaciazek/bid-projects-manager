import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CountriesComponent } from './components/countries/countries.component';

const routes: Routes = [
  { 
    path: '', component: CountriesComponent,// canActivate: [AuthGuard]
  },
  { 
    path: 'list', component: CountriesComponent,// canActivate: [AuthGuard]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CountryRoutingModule { }
