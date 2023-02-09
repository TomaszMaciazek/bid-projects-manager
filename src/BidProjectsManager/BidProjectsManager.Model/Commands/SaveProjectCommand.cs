using BidProjectsManager.Model.Enums;

namespace BidProjectsManager.Model.Commands
{
    public class SaveProjectCommand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CountryId { get; set; }
        public int CurrencyId { get; set; }
        public string NoBidReason { get; set; }
        public IEnumerable<UpdateCapexCommand> Capexes { get; set; }
        public IEnumerable<UpdateEbitCommand> Ebits { get; set; }
        public IEnumerable<UpdateOpexCommand> Opexes { get; set; }
        public BidStatus Status { get; set; }
        public ProjectType Type { get; set; }
        public int NumberOfVechicles { get; set; }
        public DateTime BidOperationStart { get; set; }
        public DateTime BidEstimatedOperationEnd { get; set; }
        public int OptionalExtensionYears { get; set; }
        public int LifetimeInThousandsKilometers { get; set; }
        public decimal TotalCapex { get; set; }
        public decimal TotalOpex { get; set; }
        public decimal TotalEbit { get; set; }
        public BidProbability Probability { get; set; }
        public BidPriority Priority { get; set; }
    }
}
