import { Component, OnInit, ViewChild } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { ConfirmationService } from 'primeng/api';
import { CountrySortOption } from 'src/app/enums/country-sort-option';
import { CountryListItem } from 'src/app/models/country-list-item.model';
import { Currency } from 'src/app/models/currency.model';
import { CountryQuery } from 'src/app/queries/country-query.model';
import { CountryService } from 'src/app/services/country.service';
import { CurrencyService } from 'src/app/services/currency.service';
import { EditCountryComponent } from '../edit-country/edit-country.component';

@Component({
  selector: 'app-countries',
  templateUrl: './countries.component.html',
  styleUrls: ['./countries.component.scss']
})
export class CountriesComponent implements OnInit {

  @ViewChild(EditCountryComponent) editCountry: EditCountryComponent;

  public pageSizeOptions = [10, 20, 50];
  public displayedColumns: string[] = ['name', 'code', 'currency', 'actions'];
  public length = 0;
  public pageSize = 20;
  public pageIndex = 0;
  public nameFilter : string | null = null;
  public countries : Array<CountryListItem> = [];
  public currencies: Array<Currency> = [];
  private sortOption : CountrySortOption = CountrySortOption.NameAscending;
  public showFilters : boolean = false;
  public displayCreateCountryialog : boolean = false;
  public displayEditCountryialog : boolean = false;

  constructor(
    private countryService: CountryService,
    private currencyService: CurrencyService,
    private spinner: NgxSpinnerService,
    private toastr : ToastrService,
    private confirmationService: ConfirmationService
    ){}
  
  ngOnInit(): void {
    this.spinner.show();
    this.currencyService.getAllCurrencies().then(res => this.currencies = res);
    this.getCountries(this.getCountryQuery());
  }

  getCountries(query: CountryQuery){
    this.countryService.getCountries(query)
    .then(res => {
      this.countries = res.items;
      this.length = res.totalCount;
      this.pageIndex = res.pageIndex - 1;
      this.spinner.hide();
    });
  }

  public resetFilters(){
    this.spinner.show();
    this.pageIndex = 0;
    this.nameFilter = null;
    this.getCountries(this.getCountryQuery())
  }

  public search(){
    this.spinner.show();
    this.pageIndex = 0;
    this.getCountries(this.getCountryQuery())
  }

  public reload(){
    this.displayCreateCountryialog = false;
    this.displayEditCountryialog = false;
    this.resetFilters();
  }

  countrySort(e: any){
    this.spinner.show();
    const isAsc = e.direction === 'asc';
    switch(e.active){
      case 'name':
        this.sortOption = isAsc ? CountrySortOption.NameAscending : CountrySortOption.NameDescending;
        break;
      case 'code':
        this.sortOption = isAsc ? CountrySortOption.CodeAscending : CountrySortOption.CodeDescending;
        break;
      default:
        this.sortOption = CountrySortOption.NameAscending;
        break;
    }
    this.getCountries(this.getCountryQuery());
  }

  handlePage(e: any) {
    this.spinner.show();
    this.pageIndex = e.pageIndex;
    this.pageSize = e.pageSize;
    this.getCountries(this.getCountryQuery());
  }

  showEditDialog(id : number){
      let country = this.countries.find(x => x.id === id);
      let currency = this.currencies.find(x => x.id === country?.currencyId);
      if(country && currency){
        this.editCountry.setValues(id, country.name, country.code, currency);
        this.displayEditCountryialog = true;
      }
  }

  getCurrency(id : number){
    let currency = this.currencies.find(x => x.id == id);
    if(currency)
      return currency.code;
    return '';
  }

  deleteCountry(id: number){
    this.confirmationService.confirm({
      message: 'Are you sure that you want to delete this country?',
      accept: () => {
        this.spinner.show();
        this.countryService.deleteCountry(id)
        .then(res => {
          this.toastr.success("Country deleted");
          this.pageIndex = 0;
          this.getCountries(this.getCountryQuery());
        })
        .catch(error => {
          this.toastr.error(error);
          this.spinner.hide();
        });
      }
    });
  }

  private getCountryQuery() : CountryQuery{
    return new CountryQuery({
      Name: this.nameFilter,
      PageSize: this.pageSize,
      PageNumber: this.pageIndex + 1,
      SortOption : this.sortOption
    });
  }
}
