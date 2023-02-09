using BidProjectsManager.DataLayer.Repositories.Common;
using BidProjectsManager.Model.Entities;

namespace BidProjectsManager.DataLayer.Repositories
{
    public interface IReportDefinitionRepository : IRepository<ReportDefinition> { }
    public class ReportDefinitionRepository : BaseRepository<ReportDefinition>, IReportDefinitionRepository
    {
        public ReportDefinitionRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
