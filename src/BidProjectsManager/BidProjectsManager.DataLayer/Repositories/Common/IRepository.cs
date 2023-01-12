using BidProjectsManager.Model.Entities;

namespace BidProjectsManager.DataLayer.Repositories.Common
{
    public interface IRepository<T> : IReadOnlyRepository<T> where T : BaseEntity
    {
        void Add(T entity);
        void AddRange(IEnumerable<T> entities);
        void Update(T entity);
        void Delete(int id);
        void RemoveRange(IEnumerable<T> entities);
        Task SaveChangesAsync();
        void SaveChanges();
    }
}
