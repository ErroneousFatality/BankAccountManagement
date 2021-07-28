using Domain.Entities.Users;
using Domain.Repositories;
using System;

namespace Infrastructure.DataAccess.EntityFramework.Repositories
{
    public class EfUserRepository : EntityFrameworkRepository<User, Guid>, IUserRepository
    {
        // Constructors
        public EfUserRepository(EfContext context) : base(context)
        {

        }
    }
}
