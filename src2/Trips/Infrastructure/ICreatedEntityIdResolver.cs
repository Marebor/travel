using System;
using System.Threading;
using System.Threading.Tasks;

namespace Trips.Infrastructure
{
    public interface ICreatedEntityIdResolver
    {
        Task<Guid> ResolveIdByCorrelationIdAsync(
            Guid correlationId, 
            CancellationToken cancellationToken);
    }
}
