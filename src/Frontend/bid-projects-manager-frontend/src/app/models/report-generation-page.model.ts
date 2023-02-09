import { ReportParam } from "./report-param.model";

export class ReportGenerationPage {
    public reportId : number;
    public title : string;
    public description : string;
    public maxRow : number;
    public params : Array<ReportParam>;
}
