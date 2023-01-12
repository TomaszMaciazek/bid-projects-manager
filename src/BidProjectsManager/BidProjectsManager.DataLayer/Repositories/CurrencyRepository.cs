using BidProjectsManager.DataLayer.Repositories.Common;
using BidProjectsManager.Model.Entities;

namespace BidProjectsManager.DataLayer.Repositories
{
    public interface ICurrencyRepository : IRepository<Currency> { }
    public class CurrencyRepository : BaseRepository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
