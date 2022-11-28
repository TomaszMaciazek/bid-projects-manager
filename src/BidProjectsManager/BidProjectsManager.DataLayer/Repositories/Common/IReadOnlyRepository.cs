using BidProjectsManager.Model.Entities;

namespace BidProjectsManager.DataLayer.Repositories.Common
{
    public interface IReadOnlyRepository<T> where T : BaseEntity
    {
        IQueryable<T> GetAll();
        IQueryable<T> GetById(int id);
    }
}
