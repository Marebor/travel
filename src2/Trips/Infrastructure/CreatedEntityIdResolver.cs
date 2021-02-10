using Microsoft.Extensions.Caching.Memory;
using System;
using System.Threading;
using System.Threading.Tasks;
using Trips.Common;
using Trips.Domain.Events;

namespace Trips.Infrastructure
{
    public class CreatedEntityIdResolver : 
        ICreatedEntityIdResolver, 
        IEventHandler<CustomerCreated>,
        IEventHandler<TripCreated>
    {
        private readonly static TimeSpan CacheExpiration = TimeSpan.FromMinutes(5);

        private readonly IMemoryCache _cache;

        public CreatedEntityIdResolver(IMemoryCache cache)
        {
            _cache = cache;
        }

        Task IEventHandler<CustomerCreated>.HandleAsync(CustomerCreated @event, CancellationToken cancellationToken)
        {
            _cache.Set(
                GetCacheKey(@event.CorrelationId), 
                @event.CustomerId, 
                CacheExpiration);

            return Task.CompletedTask;
        }

        Task IEventHandler<TripCreated>.HandleAsync(TripCreated @event, CancellationToken cancellationToken)
        {
            _cache.Set(
                GetCacheKey(@event.CorrelationId), 
                @event.TripId, 
                CacheExpiration);

            return Task.CompletedTask;
        }

        Task<Guid> ICreatedEntityIdResolver.ResolveIdByCorrelationIdAsync(
            Guid correlationId, 
            CancellationToken cancellationToken)
        {
            if (!_cache.TryGetValue(GetCacheKey(correlationId), out Guid id))
            {
                throw new Exception($"Could not resolve created entity ID by correlationId = {correlationId}");
            }

            return Task.FromResult(id);
        }

        private static string GetCacheKey(Guid correlationId)
        {
            return $"{nameof(CreatedEntityIdResolver)}-{correlationId}";
        }
    }
}
