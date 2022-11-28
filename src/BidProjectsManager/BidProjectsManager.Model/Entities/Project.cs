using BidProjectsManager.Model.Enums;
using System.ComponentModel.DataAnnotations;

namespace BidProjectsManager.Model.Entities
{
    public class Project : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        [Required]
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

        [Required]
        public BidStatus BidStatus { get; set; }

        [Required]
        public ProjectStage Stage { get; set; }

        [Required]
        public int NumberOfVechicles { get; set; }

        [Required]
        public DateTime BidOperationStart { get; set; }

        [Required]
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

        [Required]
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }

        public ICollection<BidCapex> Capexes { get; set; }
        public ICollection<BidEbit> Ebits { get; set; }
        public ICollection<BidOpex> Opexes { get; set; }
        public ICollection<ProjectComment> Comments { get; set; }
    }
}
