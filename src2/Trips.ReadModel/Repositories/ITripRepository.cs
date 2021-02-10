using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Trips.ReadModel.Repositories
{
    public interface ITripRepository
    {
        Task<IReadOnlyCollection<ITrip>> GetTripsAsync(
            CancellationToken cancellationToken);
        Task<ITrip> GetTripByUserFriendlyIdAsync(
            int id, 
            CancellationToken cancellationToken);
        Task<ITrip> GetTripByUniqueIdAsync(
            Guid id,
            CancellationToken cancellationToken);
        Task<ITrip> CreateTripAsync(
            Guid id, 
            string destination, 
            bool isCancelled, 
            string owner, 
            CancellationToken cancellationToken);
        Task<ITrip> UpdateTripAsync(
            Guid id, 
            string? destination, 
            bool? isCancelled, 
            string? owner, 
            CancellationToken cancellationToken);
        Task AssignCustomerToTripAsync(
            Guid id,
            Guid customerId, 
            CancellationToken cancellationToken);
    }
}
