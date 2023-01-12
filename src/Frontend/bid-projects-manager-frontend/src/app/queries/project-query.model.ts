import { BidStatus } from "../enums/bid-status";
import { ProjectSortOption } from "../enums/project-sort-option";
import { ProjectStage } from "../enums/project-stage";

export class ProjectQuery {
    public Name : string | null ;
    public Stages : Array<ProjectStage> | null;
    public Statuses : Array<BidStatus> | null;
    public CountriesIds : Array<number>| null;
    public PageSize : number;
    public PageNumber : number;
    public SortOption : ProjectSortOption | null;

    constructor(data: {
        Name : string | null,
        Stages : Array<ProjectStage> | null,
        Statuses : Array<BidStatus> | null,
        CountriesIds : Array<number> | null,
        PageSize : number | null,
        PageNumber : number | null,
        SortOption : ProjectSortOption | null,
    }){
        Object.assign(this, data)
    }
}
