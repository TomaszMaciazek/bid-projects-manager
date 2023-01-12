using BidProjectsManager.Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace BidProjectsManager.Model.Entities
{
    public class Project : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public string CreatedBy { get; set; }

        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set;}

        [Required]
        public int CountryId { get; set; }
        public Country Country { get; set; }
        public BidStatus? Status { get; set; }
        public ProjectStage Stage { get; set; }
        public int? NumberOfVechicles { get; set; }
        public DateTime? BidOperationStart { get; set; }
        public DateTime? BidEstimatedOperationEnd { get; set; }

        public string NoBidReason { get; set; }

        public int? OptionalExtensionYears { get; set; }

        public int? LifetimeInThousandsKilometers { get; set; }

        public decimal? TotalCapex { get; set; }

        public decimal? TotalOpex { get; set; }

        public decimal? TotalEbit { get; set; }
        public BidProbability? Probability { get; set; }
        public BidPriority? Priority { get; set; }

        public DateTime? ApprovalDate { get; set; }
        public ProjectType? Type { get; set; }

        [Required]
        public int CurrencyId { get; set; }
        public Currency ProjectCurrency { get; set; }

        public ICollection<BidCapex> Capexes { get; set; }
        public ICollection<BidEbit> Ebits { get; set; }
        public ICollection<BidOpex> Opexes { get; set; }
        public ICollection<ProjectComment> Comments { get; set; }
    }
}
