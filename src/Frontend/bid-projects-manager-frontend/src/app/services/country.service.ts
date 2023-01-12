import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { environment } from 'src/environments/environment';
import { toParams } from '../app-helpers';
import { AddCountryCommand } from '../commands/add-country-command.model';
import { UpdateCountryCommand } from '../commands/update-country-command.model';
import { CountryListItem } from '../models/country-list-item.model';
import { Country } from '../models/country.model';
import { PaginatedList } from '../models/paginated-list.model';
import { CountryQuery } from '../queries/country-query.model';

@Injectable({
  providedIn: 'root'
})
export class CountryService {

  private url = `${environment.apiUrl}/country`

  constructor(private http: HttpClient) { }

  getCountries(query: CountryQuery) : Promise<PaginatedList<CountryListItem>>{
    return lastValueFrom(this.http.get<PaginatedList<CountryListItem>>(`${this.url}`, {params: toParams(query)})).catch(this.handleError.bind(this));
  }

  getAllCountries() : Promise<Country[]>{
    return lastValueFrom(this.http.get<Country[]>(`${this.url}/all`)).catch(this.handleError.bind(this));
  }

  addCountry(command : AddCountryCommand){
    return lastValueFrom(this.http.post(this.url, command))
    .catch(this.handleError.bind(this));
  }

  updateCountry(command : UpdateCountryCommand){
    return lastValueFrom(this.http.put(this.url, command))
    .catch(this.handleError.bind(this));
  }

  deleteCountry(id: number){
    return lastValueFrom(this.http.delete(`${this.url}/${id}`))
    .catch(this.handleError.bind(this));
  }

  private async handleError(error: any) {
    if (error.status === 404) {
      return Promise.reject('Not found');
    }
    else if(error.status === 409){
      return Promise.reject('Country with given code already exists');
    }
    else {
      return Promise.reject('Something went wrong');
    }
  }
}
