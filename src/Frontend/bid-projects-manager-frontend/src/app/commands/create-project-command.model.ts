import { CreateCapexCommand } from "./create-capex-command.model";
import { CreateEbitCommand } from "./create-ebit-command.model";
import { CreateOpexCommand } from "./create-opex-command.model";

export class CreateProjectCommand {
    public Name : string;
    public Description : string;
    public CountryId : number;
    public CurrencyId : number;
    public Capexes : Array<CreateCapexCommand>;
    public Ebits : Array<CreateEbitCommand>;
    public Opexes : Array<CreateOpexCommand>;
    public NoBidReason: string;
}
