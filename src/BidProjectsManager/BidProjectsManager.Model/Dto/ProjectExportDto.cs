using BidProjectsManager.Model.Enums;
using System.ComponentModel;

namespace BidProjectsManager.Model.Dto
{
    public class ProjectExportDto
    {
        [DisplayName("No")]
        public string No { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public ProjectStage Stage { get; set; }
        public BidStatus? Status { get; set; }
        public ProjectType? Type { get; set; }

        [DisplayName("Number Of Vechicles")]
        public int NumberOfVechicles { get; set; }

        [DisplayName("Start Of Bid Operation")]
        public DateTime? BidOperationStart { get; set; }

        [DisplayName("Estimated End Of Bid Operation")]
        public DateTime? BidEstimatedOperationEnd { get; set; }

        [DisplayName("Optional Extension In Years")]
        public int? OptionalExtensionYears { get; set; }
        [DisplayName("Lifetime In Thousands Kilometers")]
        public int? LifetimeInThousandsKilometers { get; set; }
        public BidProbability? Probability { get; set; }
        public BidPriority? Priority { get; set; }

        [DisplayName("Total Capex")]
        public decimal? TotalCapex { get; set; }

        [DisplayName("Total Opex")]
        public decimal? TotalOpex { get; set; }

        [DisplayName("Total Ebit")]
        public decimal? TotalEbit { get; set; }

        [DisplayName("Approval Date")]
        public DateTime? ApprovalDate { get; set; }
        public DateTime Created { get; set; }

        [DisplayName("Created By")]
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }

        [DisplayName("Modified By")]
        public string ModifiedBy { get; set; }

        [DisplayName("Reason For No Bid")]
        public string NoBidReason { get; set; }
    }
}
