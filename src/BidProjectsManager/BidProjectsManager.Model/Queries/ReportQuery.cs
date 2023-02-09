using BidProjectsManager.Model.Enums;

namespace BidProjectsManager.Model.Queries
{
    public class ReportQuery
    {
        public string Title { get; set; }
        public string Group { get; set; }
        public ReportSortOption? SortOption { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
