﻿using BidProjectsManager.Model.Enums;

namespace BidProjectsManager.Model.Commands
{
    public class CreateSubmittedProjectCommand : CreateProjectCommand
    {
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
        public BidProbability BidProbability { get; set; }
        public BidPriority Priority { get; set; }
    }
}
