using Domain.Repositories;

namespace Infrastructure.DataAccess.EntityFramework.Repositories
{
    public class EfUnitOfWork : EntityFrameworkUnitOfWork, IUnitOfWork
    {
        // Constructors
        public EfUnitOfWork(EfContext context) : base(context)
        {
        }
    }
}
