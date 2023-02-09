export class UpdateReportDefinitionCommand {
    public Id : number;
    public Title : string;
    public Group : string;
    public Description : string;
    public Version : string;
    public MaxRow : number;
    public IsActive : boolean;
    public XmlDefinition : string;

    constructor(data: {
        Id : number;
        Title : string,
        Group : string,
        Description : string,
        Version : string,
        MaxRow : number,
        IsActive : boolean,
        XmlDefinition : string
    }){
        Object.assign(this,data);
    }
}
