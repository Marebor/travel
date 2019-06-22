using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Travel.Common.Cqrs;

namespace Travel.Infrastructure.Messaging
{
    public class EventPublisher : IEventPublisher
    {
        private readonly Func<Type, IEnumerable<IEventHandler>> handlersFactory;

        public EventPublisher(Func<Type, IEnumerable<IEventHandler>> handlersFactory)
        {
            this.handlersFactory = handlersFactory;
        }

        public Task Publish<T>(T @event) where T : IEvent
        {
            IEnumerable<IEventHandler<T>> handlers = handlersFactory(@event.GetType()).Cast<IEventHandler<T>>();
            
            return Task.WhenAll(handlers.Select(x => x.Handle(@event)));
        }
    }
}
