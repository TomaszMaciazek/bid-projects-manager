export class AddCountryCommand {
    public Name : string;
    public Code : string
    public CurrencyId : number;

    constructor(data: {
        Name : string,
        Code : string,
        CurrencyId : number,
    }){
        Object.assign(this, data)
    }
}
