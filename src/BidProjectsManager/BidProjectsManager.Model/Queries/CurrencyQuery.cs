using BidProjectsManager.Model.Enums;

namespace BidProjectsManager.Model.Queries
{
    public class CurrencyQuery
    {
        public string Name { get; set; }
        public CurrencySortOption? SortOption { get; set; }
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
    }
}
