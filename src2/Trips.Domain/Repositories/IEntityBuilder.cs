using Trips.Common;

namespace Trips.Domain.Repositories
{
    internal interface IEntityBuilder { }

    internal interface IEntityBuilder<TEntity, TEvent> : IEntityBuilder 
        where TEntity : IEntity 
        where TEvent : IEvent
    {
        TEntity Apply(TEntity entity, TEvent @event);
    }
}
