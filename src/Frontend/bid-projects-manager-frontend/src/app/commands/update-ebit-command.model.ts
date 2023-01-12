export class UpdateEbitCommand {
    public Id : number;
    public Value : number | null;

    constructor(data: {
        Id : number,
        Value : number | null
    }){
        Object.assign(this, data);
    }
}
