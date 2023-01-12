using BidProjectsManager.Model.Entities;
using BidProjectsManager.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidProjectsManager.Model.Dto
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string ModifiedBy { get; set; }
        public int CountryId { get; set; }
        public BidStatus? Status { get; set; }
        public ProjectStage Stage { get; set; }
        public int NumberOfVechicles { get; set; }
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
        public int CurrencyId { get; set; }
        public ProjectType? Type { get; set; }
        public ICollection<CapexDto> Capexes { get; set; }
        public ICollection<EbitDto> Ebits { get; set; }
        public ICollection<OpexDto> Opexes { get; set; }
        public ICollection<CommentDto> Comments { get; set; }
    }
}
