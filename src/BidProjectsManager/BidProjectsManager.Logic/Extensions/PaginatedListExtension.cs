using BidProjectsManager.Logic.Result;

namespace BidProjectsManager.Logic.Extensions
{
    public static class PaginatedListExtension
    {
        public static Task<PaginatedList<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber, int pageSize)
            => PaginatedList<TDestination>.CreateAsync(queryable, pageNumber, pageSize);
    }
}
