using Domain.Entities.Users;
using System;

namespace Domain.Repositories
{
    public interface IUserRepository : IRepository<User, Guid>
    {
    }
}
