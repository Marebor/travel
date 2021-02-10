using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Trips.Common;
using Trips.Domain.Repositories;

namespace Trips.Domain.Events
{
    internal sealed class InMemoryEventPublisher : IEventPublisher
    {
        private readonly IEventRepository _eventRepository;
        private readonly Func<Type, IEnumerable<IEventHandler>> _handlersFactory;

        public InMemoryEventPublisher(
            IEventRepository eventRepository,
            Func<Type, IEnumerable<IEventHandler>> handlersFactory)
        {
            _eventRepository = eventRepository;
            _handlersFactory = handlersFactory;
        }

        async Task IEventPublisher.PublishAsync<T>(
            T @event,
            Guid relatedEntityId,
            CancellationToken cancellationToken)
        {
            await _eventRepository.AddEventAsync(@event, relatedEntityId, cancellationToken);

            var handlers = _handlersFactory(typeof(T)).Cast<IEventHandler<T>>();

            foreach (var handler in handlers)
            {
                await handler.HandleAsync(@event, cancellationToken);
            }
        }
    }
}
