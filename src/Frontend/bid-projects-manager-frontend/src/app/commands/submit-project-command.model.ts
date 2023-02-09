import { BidPriority } from "../enums/bid-priority";
import { BidProbability } from "../enums/bid-probability";
import { BidStatus } from "../enums/bid-status";
import { ProjectType } from "../enums/project-type";
import { UpdateProjectCommand } from "./update-project-command.model";

export class SubmitProjectCommand extends UpdateProjectCommand {
    public Status : BidStatus;
    public Type : ProjectType;
    public NumberOfVechicles : number;
    public BidOperationStart : string;
    public BidEstimatedOperationEnd : string;
    public OptionalExtensionYears : number;
    public LifetimeInThousandsKilometers : number;
    public TotalCapex : number;
    public TotalOpex : number;
    public TotalEbit : number;
    public Probability : BidProbability;
    public Priority : BidPriority;
}
