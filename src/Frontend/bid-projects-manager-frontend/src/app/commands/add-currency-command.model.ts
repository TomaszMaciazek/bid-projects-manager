export class AddCurrencyCommand {
    public Name : string;
    public Code : string

    constructor(data: {
        Name : string,
        Code : string
    }){
        Object.assign(this, data)
    }
}
