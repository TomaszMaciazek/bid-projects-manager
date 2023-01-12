import { BidStatus } from "../enums/bid-status";
import { ProjectStage } from "../enums/project-stage";

export class ProjectListItem {
    public id! : number;
    public name! : string;
    public countryId! : number;
    public status? : BidStatus;
    public stage! : ProjectStage;
    public created! : Date;
    public modified? : Date;
}
