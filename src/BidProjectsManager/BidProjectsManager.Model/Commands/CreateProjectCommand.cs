using BidProjectsManager.Model.Commands;
using BidProjectsManager.Model.Entities;
using BidProjectsManager.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BidProjectsManager.Model.ViewModels
{
    public class CreateProjectCommand
    {
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
        public IEnumerable<CreateCapexCommand> Capexes { get; set; }
        public IEnumerable<CreateEbitCommand> Ebits { get; set; }
        public IEnumerable<CreateOpexCommand> Opexes { get; set; }
        public IEnumerable<CreateCommentCommand> Comments { get; set; }

    }
}
