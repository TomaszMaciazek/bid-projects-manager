import { CurrencySortOption } from "../enums/currency-sort-option";

export class CurrencyQuery {
    public Name : string | null;
    public PageSize : number;
    public PageNumber : number;
    public SortOption : CurrencySortOption | null;

    constructor(data: {
        Name : string | null,
        PageSize : number | null,
        PageNumber : number | null,
        SortOption : CurrencySortOption | null,
    }){
        Object.assign(this, data)
    }
}
