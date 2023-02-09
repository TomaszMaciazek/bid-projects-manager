import { ParamType } from "../enums/param-type";
import { ReportParamValue } from "./report-param-value.model";

export class ReportParam {
    public name : string;
    public caption : string;
    public type : ParamType;
    public isRequired : boolean;
    public availableValues : Array<ReportParamValue>;
}
