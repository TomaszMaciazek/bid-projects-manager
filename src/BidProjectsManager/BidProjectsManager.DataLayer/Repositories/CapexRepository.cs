using BidProjectsManager.DataLayer.Repositories.Common;
using BidProjectsManager.Model.Entities;

namespace BidProjectsManager.DataLayer.Repositories
{
    public interface ICapexRepository: IRepository<BidCapex> { }
    public class CapexRepository : BaseRepository<BidCapex>, ICapexRepository
    {
        public CapexRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
