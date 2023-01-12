using BidProjectsManager.Model.Enums;

namespace BidProjectsManager.Model.Commands
{
    public abstract class CreateProjectCommand
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int CountryId { get; set; }
        public string NoBidReason { get; set; }
        public int CurrencyId { get; set; }
        public IEnumerable<CreateCapexCommand> Capexes { get; set; }
        public IEnumerable<CreateEbitCommand> Ebits { get; set; }
        public IEnumerable<CreateOpexCommand> Opexes { get; set; }
    }
}
