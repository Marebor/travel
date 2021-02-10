using Microsoft.Extensions.Caching.Memory;
using System;

namespace Trips.Domain.Repositories
{
    internal sealed class EntityCache : IEntityCache
    {
        private readonly IMemoryCache _cache;

        public EntityCache(IMemoryCache cache)
        {
            _cache = cache;
        }

        T IEntityCache.GetById<T>(Guid id)
        {
            return _cache.Get<T>(CreateKey(id));
        }

        void IEntityCache.Set<T>(T entity)
        {
            _cache.Set(CreateKey(entity.Id), entity);
        }

        private static string CreateKey(Guid id)
        {
            return $"{nameof(EntityCache)}-{id}";
        }
    }
}
