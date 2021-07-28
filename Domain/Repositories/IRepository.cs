using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IRepository<TEntity, TId> where TEntity : class
    {
        Task InsertAsync(TEntity entity);
        Task<TEntity> GetByIdAsync(TId id);
        Task RemoveAsync(TEntity entity);
    }
}
