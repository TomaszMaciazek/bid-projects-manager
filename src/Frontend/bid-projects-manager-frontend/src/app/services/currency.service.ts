import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { lastValueFrom } from 'rxjs';
import { environment } from 'src/environments/environment';
import { toParams } from '../app-helpers';
import { AddCurrencyCommand } from '../commands/add-currency-command.model';
import { UpdateCurrencyCommand } from '../commands/update-currency-command.model';
import { CurrencyListItem } from '../models/currency-list-item.model';
import { Currency } from '../models/currency.model';
import { PaginatedList } from '../models/paginated-list.model';
import { CurrencyQuery } from '../queries/currency-query.model';

@Injectable({
  providedIn: 'root'
})
export class CurrencyService {

  private url = `${environment.apiUrl}/currency`

  constructor(private http: HttpClient) { }

  getCurrencies(query: CurrencyQuery) : Promise<PaginatedList<CurrencyListItem>>{
    return lastValueFrom(this.http.get<PaginatedList<CurrencyListItem>>(`${this.url}`, {params: toParams(query)}))
    .catch(this.handleError.bind(this));
  }

  getAllCurrencies() : Promise<Currency[]>{
    return lastValueFrom(this.http.get<Currency[]>(`${this.url}/all`));
  }

  addCurrency(command : AddCurrencyCommand){
    return lastValueFrom(this.http.post(this.url, command))
    .catch(this.handleError.bind(this));
  }

  updateCurrency(command : UpdateCurrencyCommand){
    return lastValueFrom(this.http.put(this.url, command))
    .catch(this.handleError.bind(this));
  }

  deleteCurrency(id: number){
    return lastValueFrom(this.http.delete(`${this.url}/${id}`))
    .catch(this.handleError.bind(this));
  }

  private async handleError(error: any) {
    if (error.status === 404) {
      return Promise.reject('Not found');
    }
    else if(error.status === 409){
      return Promise.reject('Currency with given code already exists');
    }
    else {
      return Promise.reject('Something went wrong');
    }
  }
}
