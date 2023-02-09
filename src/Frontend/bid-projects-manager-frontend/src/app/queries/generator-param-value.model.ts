export class GeneratorParamValue {
    public Key : string;
    public Values : Array<string>

    constructor(data:{
        Key: string,
        Values: Array<string | null>
    })
    {
        Object.assign(this,data);
    }
}
