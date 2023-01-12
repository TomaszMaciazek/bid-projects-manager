import { BidPriority } from "../enums/bid-priority";
import { BidProbability } from "../enums/bid-probability";
import { BidStatus } from "../enums/bid-status";
import { ProjectType } from "../enums/project-type";
import { CreateProjectCommand } from "./create-project-command.model";

export class CreateSubmittedProjectCommand extends CreateProjectCommand{
    public Status : BidStatus;
    public Type : ProjectType;
    public NumberOfVechicles : number;
    public BidOperationStart : Date;
    public BidEstimatedOperationEnd : Date;
    public OptionalExtensionYears : number;
    public LifetimeInThousandsKilometers : number;
    public TotalCapex : number;
    public TotalOpex : number;
    public TotalEbit : number;
    public BidProbability : BidProbability;
    public Priority : BidPriority;
    public ApprovalDate : Date;

}
