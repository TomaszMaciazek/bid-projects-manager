using BidProjectsManager.DataLayer.Repositories.Common;
using BidProjectsManager.Model.Entities;

namespace BidProjectsManager.DataLayer.Repositories
{
    public interface IProjectCommentRepository : IRepository<ProjectComment> { }
    public class ProjectCommentRepository : BaseRepository<ProjectComment>, IProjectCommentRepository
    {
        public ProjectCommentRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
