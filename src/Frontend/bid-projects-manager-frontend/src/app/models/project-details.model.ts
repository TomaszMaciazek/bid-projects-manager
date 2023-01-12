import { BidPriority } from "../enums/bid-priority";
import { BidProbability } from "../enums/bid-probability";
import { BidStatus } from "../enums/bid-status";
import { ProjectStage } from "../enums/project-stage";
import { ProjectType } from "../enums/project-type";
import { Capex } from "./capex.model";
import { Ebit } from "./ebit.model";
import { Opex } from "./opex.model";
import { ProjectComment } from "./project-comment.model";

export class ProjectDetails {
    public id : number;
    public name : string;
    public description : string | null;
    public type: ProjectType | null;
    public created : Date;
    public createdBy : string
    public modified : Date | null;
    public modifiedBy : string | null;
    public countryId : number;
    public status : BidStatus | null;
    public stage : ProjectStage;
    public numberOfVechicles : number | null;
    public bidOperationStart : Date | null;
    public bidEstimatedOperationEnd : Date | null;
    public noBidReason : string | null;
    public optionalExtensionYears : number | null;
    public lifetimeInThousandsKilometers : number | null;
    public totalCapex : number | null;
    public totalOpex : number | null;
    public totalEbit : number | null;
    public probability : BidProbability | null;
    public priority : BidPriority | null;
    public approvalDate : Date | null;
    public currencyId : number;
    public capexes : Array<Capex>;
    public ebits : Array<Ebit>;
    public opexes : Array<Opex>;
    public comments : Array<ProjectComment>;
}
