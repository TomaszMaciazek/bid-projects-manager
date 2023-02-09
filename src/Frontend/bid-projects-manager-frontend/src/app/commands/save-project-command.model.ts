import { BidPriority } from "../enums/bid-priority";
import { BidProbability } from "../enums/bid-probability";
import { BidStatus } from "../enums/bid-status";
import { ProjectType } from "../enums/project-type";
import { UpdateCapexCommand } from "./update-capex-command.model";
import { UpdateEbitCommand } from "./update-ebit-command.model";
import { UpdateOpexCommand } from "./update-opex-command.model";

export class SaveProjectCommand {
    public Id : number;
    public Name : string;
    public Description : string;
    public CountryId : number;
    public CurrencyId : number;
    public NoBidReason : string;
    public Capexes : Array<UpdateCapexCommand>;
    public Ebits : Array<UpdateEbitCommand>;
    public Opexes : Array<UpdateOpexCommand>;
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
