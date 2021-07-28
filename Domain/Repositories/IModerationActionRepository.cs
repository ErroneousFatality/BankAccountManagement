using Domain.Entities.ModerationActions;
using System;

namespace Domain.Repositories
{
    public interface IModerationActionRepository : IRepository<ModerationAction, Guid>
    {
        
    }
}
