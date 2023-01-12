namespace BidProjectsManager.Model.Commands
{
    public abstract class BaseUpdateProjectCommand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CountryId { get; set; }
        public int CurrencyId { get; set; }
        public string NoBidReason { get; set; }
        public IEnumerable<CreateCapexCommand> NewCapexes { get; set; }
        public IEnumerable<CreateEbitCommand> NewEbits { get; set; }
        public IEnumerable<CreateOpexCommand> NewOpexes { get; set; }
        public IEnumerable<UpdateCapexCommand> Capexes { get; set; }
        public IEnumerable<UpdateEbitCommand> Ebits { get; set; }
        public IEnumerable<UpdateOpexCommand> Opexes { get; set; }
        public IEnumerable<int> YearsToRemove { get; set; }
    }
}
