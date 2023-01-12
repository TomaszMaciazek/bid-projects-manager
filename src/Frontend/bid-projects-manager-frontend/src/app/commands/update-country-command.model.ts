export class UpdateCountryCommand {
    public Id : number;
    public Name : string;
    public Code : string;
    public CurrencyId : number;

    constructor(data: {
        Id : number,
        Name : string,
        Code : string,
        CurrencyId : number,
    }){
        Object.assign(this, data)
    }
}
