using BidProjectsManager.Model.Enums;

namespace BidProjectsManager.Model.Queries
{
    public class ProjectQuery
    {
        public string Name { get; set; }
        public IEnumerable<ProjectStage> Stages { get; set; }
        public IEnumerable<BidStatus> Statuses { get; set; }
        public IEnumerable<int> CountriesIds { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public ProjectSortOption? SortOption { get; set; }
    }
}
