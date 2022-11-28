using BidProjectsManager.Model.Enums;

namespace BidProjectsManager.Model.Commands
{
    public class UpdateProjectCommand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int CountryId { get; set; }
        public BidStatus BidStatus { get; set; }
        public ProjectStage Stage { get; set; }
        public int NumberOfVechicles { get; set; }
        public DateTime BidOperationStart { get; set; }
        public DateTime BidEstiamtedOperationEnd { get; set; }
        public string NoBidReason { get; set; }
        public int? OptionalExtensionYears { get; set; }
        public int? LifetimeInKilometers { get; set; }
        public decimal? TotalCapex { get; set; }
        public decimal? TotalOpex { get; set; }
        public decimal? TotalEbit { get; set; }
        public BidProbability? Probability { get; set; }
        public BidPriority? Priority { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public int CurrencyId { get; set; }
        public IEnumerable<CreateCapexCommand> NewCapexes { get; set; }
        public IEnumerable<CreateEbitCommand> NewEbits { get; set; }
        public IEnumerable<CreateOpexCommand> NewOpexes { get; set; }
        public IEnumerable<CreateCommentCommand> NewComments { get; set; }
        public IEnumerable<UpdateCapexCommand> Capexes { get; set; }
        public IEnumerable<UpdateEbitCommand> Ebits { get; set; }
        public IEnumerable<UpdateOpexCommand> Opexes { get; set; }
    }
}
