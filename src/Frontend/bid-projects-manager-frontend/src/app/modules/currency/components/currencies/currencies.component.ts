import { Component, ViewChild } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { ConfirmationService } from 'primeng/api';
import { CurrencySortOption } from 'src/app/enums/currency-sort-option';
import { Country } from 'src/app/models/country.model';
import { CurrencyListItem } from 'src/app/models/currency-list-item.model';
import { CurrencyQuery } from 'src/app/queries/currency-query.model';
import { CountryService } from 'src/app/services/country.service';
import { CurrencyService } from 'src/app/services/currency.service';
import { EditCurrencyComponent } from '../edit-currency/edit-currency.component';

@Component({
  selector: 'app-currencies',
  templateUrl: './currencies.component.html',
  styleUrls: ['./currencies.component.scss']
})
export class CurrenciesComponent {
  @ViewChild(EditCurrencyComponent) editCurrency: EditCurrencyComponent;

  public pageSizeOptions = [10, 20, 50];
  public displayedColumns: string[] = ['name', 'code', 'actions'];
  public length = 0;
  public pageSize = 20;
  public pageIndex = 0;
  public nameFilter : string | null = null;
  public countries : Array<Country> = [];
  public currencies: Array<CurrencyListItem> = [];
  private sortOption : CurrencySortOption = CurrencySortOption.NameAscending;
  public showFilters : boolean = false;
  public displayCreateCurrencyDialog : boolean = false;
  public displayEditCurrencyDialog : boolean = false;

  constructor(
    private countryService: CountryService,
    private currencyService: CurrencyService,
    private spinner: NgxSpinnerService,
    private toastr : ToastrService,
    private confirmationService: ConfirmationService
    ){}
  
  ngOnInit(): void {
    this.spinner.show();
    this.countryService.getAllCountries().then(res => this.countries = res);
    this.getCurrencies(this.getCurrencyQuery());
  }

  getCurrencies(query: CurrencyQuery){
    this.currencyService.getCurrencies(query)
    .then(res => {
      this.currencies = res.items;
      this.length = res.totalCount;
      this.pageIndex = res.pageIndex - 1;
      this.spinner.hide();
    });
  }

  public resetFilters(){
    this.spinner.show();
    this.pageIndex = 0;
    this.nameFilter = null;
    this.getCurrencies(this.getCurrencyQuery())
  }

  public search(){
    this.spinner.show();
    this.pageIndex = 0;
    this.getCurrencies(this.getCurrencyQuery())
  }

  public reload(){
    this.displayCreateCurrencyDialog = false;
    this.displayEditCurrencyDialog = false;
    this.resetFilters();
  }

  currencySort(e: any){
    this.spinner.show();
    const isAsc = e.direction === 'asc';
    switch(e.active){
      case 'name':
        this.sortOption = isAsc ? CurrencySortOption.NameAscending : CurrencySortOption.NameDescending;
        break;
      case 'code':
        this.sortOption = isAsc ? CurrencySortOption.CodeAscending : CurrencySortOption.CodeDescending;
        break;
      default:
        this.sortOption = CurrencySortOption.NameAscending;
        break;
    }
    this.getCurrencies(this.getCurrencyQuery());
  }

  handlePage(e: any) {
    this.spinner.show();
    this.pageIndex = e.pageIndex;
    this.pageSize = e.pageSize;
    this.getCurrencies(this.getCurrencyQuery());
  }

  showEditDialog(id : number){
      let currency = this.currencies.find(x => x.id === id);
      if(currency){
        this.editCurrency.setValues(id, currency.name, currency.code);
        this.displayEditCurrencyDialog = true;
      }
  }

  deleteCurrency(id: number){
    this.confirmationService.confirm({
      message: 'Are you sure that you want to delete this currency?',
      accept: () => {
        this.spinner.show();
        this.currencyService.deleteCurrency(id)
        .then(res => {
          this.toastr.success("Currency deleted");
          this.pageIndex = 0;
          this.getCurrencies(this.getCurrencyQuery());
        })
        .catch(error => {
          this.toastr.error(error);
          this.spinner.hide();
        });
      }
    });
  }

  private getCurrencyQuery() : CurrencyQuery{
    return new CurrencyQuery({
      Name: this.nameFilter,
      PageSize: this.pageSize,
      PageNumber: this.pageIndex + 1,
      SortOption : this.sortOption
    });
  }
}
