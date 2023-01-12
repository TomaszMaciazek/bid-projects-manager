import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CurrencyRoutingModule } from './currency-routing.module';
import { CurrenciesComponent } from './components/currencies/currencies.component';
import { AddCurrencyComponent } from './components/add-currency/add-currency.component';
import { EditCurrencyComponent } from './components/edit-currency/edit-currency.component';
import { SharedModule } from '../shared/shared.module';



@NgModule({
  declarations: [
    CurrenciesComponent,
    AddCurrencyComponent,
    EditCurrencyComponent
  ],
  imports: [
    CommonModule,
    CurrencyRoutingModule,
    SharedModule
  ]
})
export class CurrencyModule { }
