import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { CountryRoutingModule } from './country-routing.module';
import { CountriesComponent } from './components/countries/countries.component';
import { SharedModule } from '../shared/shared.module';
import { AddCountryComponent } from './components/add-country/add-country.component';
import { EditCountryComponent } from './components/edit-country/edit-country.component';


@NgModule({
  declarations: [
    CountriesComponent,
    AddCountryComponent,
    EditCountryComponent
  ],
  imports: [
    CommonModule,
    CountryRoutingModule,
    SharedModule
  ]
})
export class CountryModule { }
