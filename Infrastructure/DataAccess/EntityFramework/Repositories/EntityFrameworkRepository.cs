using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Infrastructure.DataAccess.EntityFramework.Repositories
{
    public abstract class EntityFrameworkRepository<TEntity, TId> : IRepository<TEntity, TId>
        where TEntity : class
    {
        // Fields
        protected readonly DbContext Context;
        protected readonly DbSet<TEntity> DbSet;

        // Constructors
        public EntityFrameworkRepository(
            DbContext dbContext
        )
        {
            Context = dbContext;
            DbSet = Context.Set<TEntity>();
        }

        // Methods
        public Task InsertAsync(TEntity entity)
        {
            DbSet.Add(entity);
            return Task.CompletedTask;
        }

        public async Task<TEntity> GetByIdAsync(TId id)
        {
            return await DbSet.FindAsync(id);
        }

        public Task RemoveAsync(TEntity entityToDelete)
        {
            DbSet.Remove(entityToDelete);
            return Task.CompletedTask;
        }
    }
}
