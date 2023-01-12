export class UpdateCurrencyCommand {
    public Id : number;
    public Name : string;
    public Code : string;

    constructor(data: {
        Id : number,
        Name : string,
        Code : string
    }){
        Object.assign(this, data)
    }
}
