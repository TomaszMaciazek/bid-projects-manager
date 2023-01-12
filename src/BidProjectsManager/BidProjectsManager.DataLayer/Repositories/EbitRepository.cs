using BidProjectsManager.DataLayer.Repositories.Common;
using BidProjectsManager.Model.Entities;

namespace BidProjectsManager.DataLayer.Repositories
{
    public interface IEbitRepository : IRepository<BidEbit> { }

    public class EbitRepository : BaseRepository<BidEbit>, IEbitRepository
    {
        public EbitRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
