using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Trips.Common;

namespace Trips.Domain.Repositories
{
    public interface IEventRepository
    {
        Task AddEventAsync<T>(
            T @event, 
            Guid relatedEntityId,
            CancellationToken cancellationToken) where T : IEvent;
        Task<IReadOnlyCollection<IEvent>> GetEntityEventsAsync(
            Guid entityId,
            CancellationToken cancellationToken);
    }
}
