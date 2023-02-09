import { GeneratorParamValue } from "./generator-param-value.model";

export class GeneratorQuery {
    public Reportid : number
    public MaxRows : number | null;
    public ParamsValues : Array<GeneratorParamValue>;
}
