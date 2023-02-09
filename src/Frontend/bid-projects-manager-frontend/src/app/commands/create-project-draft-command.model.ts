import { BidPriority } from "../enums/bid-priority";
import { BidProbability } from "../enums/bid-probability";
import { BidStatus } from "../enums/bid-status";
import { ProjectType } from "../enums/project-type";
import { CreateProjectCommand } from "./create-project-command.model";

export class CreateProjectDraftCommand extends CreateProjectCommand {
    public Status : BidStatus | null;
    public Type : ProjectType | null;
    public NumberOfVechicles : number | null;
    public BidOperationStart : string | null;
    public BidEstimatedOperationEnd : string | null;
    public OptionalExtensionYears : number | null;
    public LifetimeInThousandsKilometers : number | null;
    public TotalCapex : number | null;
    public TotalOpex : number | null;
    public TotalEbit : number | null;
    public BidProbability : BidProbability | null;
    public Priority : BidPriority | null;
}
