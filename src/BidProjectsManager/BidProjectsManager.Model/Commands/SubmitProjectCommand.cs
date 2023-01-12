using BidProjectsManager.Model.Enums;

namespace BidProjectsManager.Model.Commands
{
    public class SubmitProjectCommand : BaseUpdateProjectCommand
    {
        public BidStatus BidStatus { get; set; }
        public ProjectStage Stage { get; set; }
        public ProjectType Type { get; set; }
        public int NumberOfVechicles { get; set; }
        public DateTime BidOperationStart { get; set; }
        public DateTime BidEstimatedOperationEnd { get; set; }
        public int? OptionalExtensionYears { get; set; }
        public int? LifetimeInKilometers { get; set; }
        public decimal? TotalCapex { get; set; }
        public decimal? TotalOpex { get; set; }
        public decimal? TotalEbit { get; set; }
        public BidProbability? Probability { get; set; }
        public BidPriority? Priority { get; set; }
    }
}
