import { ReportSortOption } from "../enums/report-sort-option";

export class ReportQuery {
    public Title : string | null;
    public Group : string | null;
    public SortOption : ReportSortOption | null;
    public PageSize : number;
    public PageNumber : number;

    constructor(data:{
        Title : string | null,
        Group : string | null,
        SortOption : ReportSortOption | null,
        PageSize : number,
        PageNumber : number,
    }){
        Object.assign(this,data);
    }
}
