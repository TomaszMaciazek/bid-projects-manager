using BidProjectsManager.DataLayer.Repositories.Common;
using BidProjectsManager.Model.Entities;

namespace BidProjectsManager.DataLayer.Repositories
{
    public interface IOpexRepository : IRepository<BidOpex> { }
    public class OpexRepository : BaseRepository<BidOpex>, IOpexRepository
    {
        public OpexRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
