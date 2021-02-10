using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Trips.Domain.Repositories
{
    public interface IDestinationRepository
    {
        Task<IReadOnlyCollection<string>> GetDestinationsAsync(CancellationToken cancellationToken);
        Task<bool> DestinationExistsAsync(string destination, CancellationToken cancellationToken);
    }
}
