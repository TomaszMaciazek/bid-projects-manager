using BidProjectsManager.Model.Entities;

namespace BidProjectsManager.DataLayer.Repositories.Common
{
    public interface IRepository<T> : IReadOnlyRepository<T> where T : BaseEntity
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
        Task SaveChangesAsync();
        void SaveChanges();
    }
}
