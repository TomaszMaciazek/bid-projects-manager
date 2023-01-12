using BidProjectsManager.DataLayer.Repositories.Common;
using BidProjectsManager.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace BidProjectsManager.DataLayer.Repositories
{

    public interface IProjectRepository : IRepository<Project> {
        IQueryable<Project> GetByIdEager(int id);
    }

    public class ProjectRepository : BaseRepository<Project>, IProjectRepository
    {
        public ProjectRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IQueryable<Project> GetByIdEager(int id)
            => _context.Projects.AsNoTracking()
            .Include(x => x.Capexes)
            .Include(x => x.Ebits)
            .Include(x => x.Opexes)
            .Include(x => x.Comments)
            .Where(x => x.Id == id);


    }
}
