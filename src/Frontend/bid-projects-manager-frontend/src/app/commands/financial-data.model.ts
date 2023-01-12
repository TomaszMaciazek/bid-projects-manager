import { CreateCapexCommand } from "./create-capex-command.model";
import { CreateEbitCommand } from "./create-ebit-command.model";
import { CreateOpexCommand } from "./create-opex-command.model";
import { UpdateCapexCommand } from "./update-capex-command.model";
import { UpdateEbitCommand } from "./update-ebit-command.model";
import { UpdateOpexCommand } from "./update-opex-command.model";

export class FinancialData {
    public NewCapexes :  Array<CreateCapexCommand> = [];
    public NewEbits : Array<CreateEbitCommand> = [];
    public NewOpexes : Array<CreateOpexCommand> = [];
    public Capexes : Array<UpdateCapexCommand> = [];
    public Ebits : Array<UpdateEbitCommand> = [];
    public Opexes : Array<UpdateOpexCommand> = [];
    public YearsToRemove : Array<number> = [];
}
