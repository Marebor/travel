using System;

namespace Trips.Domain.Repositories
{
    internal interface IEntityBuildersFactory
    {
        IEntityBuilder GetBuilder(Type entityType, Type eventType);
    }
}
