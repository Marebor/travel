using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Trips.Common;
using Trips.Domain.Repositories;

namespace Trips.Infrastructure
{
    internal sealed class EfCoreDomainRepository : IEventRepository
    {
        private readonly TripsDomainContext _context;

        public EfCoreDomainRepository(TripsDomainContext context)
        {
            _context = context;
        }

        async Task IEventRepository.AddEventAsync<T>(T @event, Guid relatedEntityId, CancellationToken cancellationToken)
        {
            await _context.Events.AddAsync(
                new TripsDomainContext.Event
                {
                    RelatedEntityId = relatedEntityId,
                    EventType = typeof(T).AssemblyQualifiedName!,
                    Content = JsonConvert.SerializeObject(@event)
                },
                cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
        }

        async Task<IReadOnlyCollection<IEvent>> IEventRepository.GetEntityEventsAsync(Guid entityId, CancellationToken cancellationToken)
        {
            var events = await _context.Events
                .Where(e => e.RelatedEntityId == entityId)
                .ToListAsync(cancellationToken);

            return events
                .Select(e =>
                {
                    var eventType = Type.GetType(e.EventType)!;
                    var deserializeMethod = typeof(JsonConvert)
                        .GetMethod(nameof(JsonConvert.DeserializeObject), 1, new[] { typeof(string) })!
                        .MakeGenericMethod(eventType);
                    var @event = deserializeMethod.Invoke(null, new[] { e.Content })!;

                    return (IEvent)@event;
                })
                .ToArray();
        }
    }
}
