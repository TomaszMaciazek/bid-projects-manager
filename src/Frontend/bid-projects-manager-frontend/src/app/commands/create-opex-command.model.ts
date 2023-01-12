export class CreateOpexCommand {
    public Year : number;
    public Value : number;

    constructor(data: {
        Year : number,
        Value : number
    }){
        Object.assign(this, data);
    }
}
