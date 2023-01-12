using BidProjectsManager.Model.Enums;

namespace BidProjectsManager.Model.Queries
{
    public class CountryQuery
    {
        public string Name { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public CountrySortOption? SortOption { get; set; }
    }
}
