import { CreateCapexCommand } from "../commands/create-capex-command.model";
import { CreateEbitCommand } from "../commands/create-ebit-command.model";
import { CreateOpexCommand } from "../commands/create-opex-command.model";
import { CreateProjectDraftCommand } from "../commands/create-project-draft-command.model";
import { BidPriority } from "../enums/bid-priority";
import { BidProbability } from "../enums/bid-probability";
import { BidStatus } from "../enums/bid-status";
import { ProjectType } from "../enums/project-type";

export class CreateProjectDraftBuilder {
    private command : CreateProjectDraftCommand;

    constructor() {
        this.reset();
    }

    public reset(): void {
        this.command = new CreateProjectDraftCommand();
    }

    public name(name: string){
        this.command.Name = name;
        return this;
    }

    public description(description: string){
        this.command.Description = description;
        return this;
    }

    public countryId(countryId: number){
        this.command.CountryId = countryId;
        return this;
    }

    public currencyId(currencyId: number){
        this.command.CurrencyId = currencyId;
        return this;
    }

    public noBidReason(noBidReason : string){
        this.command.NoBidReason = noBidReason;
        return this;
    }

    public capexes(capexes: Array<CreateCapexCommand>){
        this.command.Capexes = capexes;
        return this;
    }

    public opexes(opexes: Array<CreateOpexCommand>){
        this.command.Opexes = opexes;
        return this;
    }

    public ebits(ebits: Array<CreateEbitCommand>){
        this.command.Ebits = ebits;
        return this;
    }

    public status(status : BidStatus | null){
        this.command.Status = status;
        return this;
    }

    public type(type : ProjectType | null){
        this.command.Type = type;
        return this;
    }

    public numberOfVechicles (numberOfVechicles : number | null){
        this.command.NumberOfVechicles = numberOfVechicles;
        return this;
    }

    public bidOperationStart(bidOperationStart : string | null){
        this.command.BidOperationStart = bidOperationStart;
        return this;
    }

    public bidEstimatedOperationEnd(bidEstimatedOperationEnd : string | null){
        this.command.BidEstimatedOperationEnd = bidEstimatedOperationEnd;
        return this;
    }

    public optionalExtensionYears(optionalExtensionYears : number | null){
        this.command.OptionalExtensionYears = optionalExtensionYears;
        return this;
    }
    public lifetimeInThousandsKilometers(lifetimeInThousandsKilometers : number | null){
        this.command.LifetimeInThousandsKilometers = lifetimeInThousandsKilometers;
        return this;
    }

    public totalCapex(totalCapex : number | null){
        this.command.TotalCapex = totalCapex;
        return this;
    }

    public totalOpex(totalOpex : number | null){
        this.command.TotalOpex = totalOpex;
        return this;
    }

    public totalEbit(totalEbit : number | null){
        this.command.TotalEbit = totalEbit;
        return this;
    }

    public bidProbability(bidProbability : BidProbability | null){
        this.command.BidProbability = bidProbability;
        return this;
    }

    public priority(priority : BidPriority | null){
        this.command.Priority = priority;
        return this;
    }

    public build(){
        return this.command;
    }
}
