﻿using BidProjectsManager.Model.Enums;

namespace BidProjectsManager.Model.Dto
{
    public class ProjectListItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }
        public BidStatus? Status { get; set; }
        public ProjectStage Stage { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Modified { get; set; }
    }
}
