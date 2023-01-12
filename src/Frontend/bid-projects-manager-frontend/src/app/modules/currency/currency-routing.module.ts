import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CurrenciesComponent } from './components/currencies/currencies.component';

const routes: Routes = [
  { 
    path: '', component: CurrenciesComponent,// canActivate: [AuthGuard]
  },
  { 
    path: 'list', component: CurrenciesComponent,// canActivate: [AuthGuard]
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class CurrencyRoutingModule { }
