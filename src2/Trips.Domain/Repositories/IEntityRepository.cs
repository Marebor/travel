using System;
using System.Threading;
using System.Threading.Tasks;

namespace Trips.Domain.Repositories
{
    internal interface IEntityRepository
    {
        Task<T> GetEntityByIdAsync<T>(
            Guid id, 
            CancellationToken cancellationToken) where T : IEntity;
    }
}
