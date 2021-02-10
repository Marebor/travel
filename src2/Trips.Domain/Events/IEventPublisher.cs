using System;
using System.Threading;
using System.Threading.Tasks;
using Trips.Common;

namespace Trips.Domain.Events
{
    internal interface IEventPublisher
    {
        Task PublishAsync<T>(
            T @event, 
            Guid relatedEntityId, 
            CancellationToken cancellationToken) where T : IEvent;
    }
}
