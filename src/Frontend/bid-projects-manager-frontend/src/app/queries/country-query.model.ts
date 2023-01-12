import { CountrySortOption } from "../enums/country-sort-option";

export class CountryQuery {
    public Name? : string | null;
    public PageSize! : number;
    public PageNumber! : number;
    public SortOption? : CountrySortOption | null;

    constructor(data: {
        Name : string | null,
        PageSize : number | null,
        PageNumber : number | null,
        SortOption : CountrySortOption | null,
    }){
        Object.assign(this, data)
    }
}
