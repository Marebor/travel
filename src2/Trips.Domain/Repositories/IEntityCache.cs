using System;

namespace Trips.Domain.Repositories
{
    internal interface IEntityCache
    {
        void Set<T>(T entity) where T : IEntity;
        T GetById<T>(Guid id) where T : IEntity;
    }
}
