import { SaveProjectCommand } from "../commands/save-project-command.model";
import { UpdateCapexCommand } from "../commands/update-capex-command.model";
import { UpdateEbitCommand } from "../commands/update-ebit-command.model";
import { UpdateOpexCommand } from "../commands/update-opex-command.model";
import { BidPriority } from "../enums/bid-priority";
import { BidProbability } from "../enums/bid-probability";
import { BidStatus } from "../enums/bid-status";
import { ProjectType } from "../enums/project-type";

export class SaveProjectCommandBuilder {
    private command: SaveProjectCommand;

    constructor() {
        this.reset();
    }

    public reset(): void {
        this.command = new SaveProjectCommand();
    }

    public id(id: number){
        this.command.Id = id;
        return this;
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

    public capexes(capexes: Array<UpdateCapexCommand>){
        this.command.Capexes = capexes;
        return this;
    }

    public opexes(opexes: Array<UpdateOpexCommand>){
        this.command.Opexes = opexes;
        return this;
    }

    public ebits(ebits: Array<UpdateEbitCommand>){
        this.command.Ebits = ebits;
        return this;
    }

    public status(status : BidStatus){
        this.command.Status = status;
        return this;
    }

    public type(type : ProjectType){
        this.command.Type = type;
        return this;
    }

    public numberOfVechicles (numberOfVechicles : number){
        this.command.NumberOfVechicles = numberOfVechicles;
        return this;
    }

    public bidOperationStart(bidOperationStart : string){
        this.command.BidOperationStart = bidOperationStart;
        return this;
    }

    public bidEstimatedOperationEnd(bidEstimatedOperationEnd : string){
        this.command.BidEstimatedOperationEnd = bidEstimatedOperationEnd;
        return this;
    }

    public optionalExtensionYears(optionalExtensionYears : number){
        this.command.OptionalExtensionYears = optionalExtensionYears;
        return this;
    }
    public lifetimeInThousandsKilometers(lifetimeInThousandsKilometers : number){
        this.command.LifetimeInThousandsKilometers = lifetimeInThousandsKilometers;
        return this;
    }

    public totalCapex(totalCapex : number){
        this.command.TotalCapex = totalCapex;
        return this;
    }

    public totalOpex(totalOpex : number){
        this.command.TotalOpex = totalOpex;
        return this;
    }

    public totalEbit(totalEbit : number){
        this.command.TotalEbit = totalEbit;
        return this;
    }

    public bidProbability(bidProbability : BidProbability){
        this.command.Probability = bidProbability;
        return this;
    }

    public priority(priority : BidPriority){
        this.command.Priority = priority;
        return this;
    }

    public build(){
        return this.command;
    }
}
