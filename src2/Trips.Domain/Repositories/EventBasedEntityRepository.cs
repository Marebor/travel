using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Trips.Common;

namespace Trips.Domain.Repositories
{
    internal sealed class EventBasedEntityRepository : IEntityRepository
    {
        private readonly IEntityBuildersFactory _entityBuildersFactory;
        private readonly IEntityCache _entityCache;
        private readonly IEventRepository _eventRepository;

        public EventBasedEntityRepository(
            IEntityBuildersFactory entityBuildersFactory, 
            IEntityCache entityCache, 
            IEventRepository eventRepository)
        {
            _entityBuildersFactory = entityBuildersFactory;
            _entityCache = entityCache;
            _eventRepository = eventRepository;
        }

        async Task<T> IEntityRepository.GetEntityByIdAsync<T>(Guid id, CancellationToken cancellationToken)
        {
            var events = await _eventRepository.GetEntityEventsAsync(id, cancellationToken);
            var orderedEvents = events.OrderBy(e => e.SequenceNumber);
            T entity = default(T);

            foreach (var @event in orderedEvents)
            {
                var eventType = @event.GetType();
                var builder = _entityBuildersFactory.GetBuilder(typeof(T), eventType);

                entity = (T)typeof(IEntityBuilder<,>)
                    .MakeGenericType(typeof(T), eventType)
                    .GetMethod(nameof(IEntityBuilder<IEntity, IEvent>.Apply))
                    .Invoke(builder, new object[] { entity, @event });

                if (entity.SequenceNumber != @event.SequenceNumber)
                {
                    var technicalMessage = $"Entity {typeof(T).Name} with ID = {id} could not be built. " +
                        $"{nameof(IEntity.SequenceNumber)} should be equal to {@event.SequenceNumber}, " +
                        $"but actual value is {entity.SequenceNumber}.";

                    throw new TripsException(
                        "Critical system error occured.",
                        technicalMessage);
                }
            }

            return entity!;
        }
    }
}
