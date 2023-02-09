export class CreateReportDefinitionCommand {
    public Title : string;
    public Group : string;
    public Description : string;
    public MaxRow : number;
    public XmlDefinition : string;

    constructor(data: {
        Title : string,
        Group : string,
        Description : string,
        MaxRow : number,
        XmlDefinition : string
    }){
        Object.assign(this,data);
    }
}
