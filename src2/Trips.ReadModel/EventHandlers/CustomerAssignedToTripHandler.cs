using System.Threading;
using System.Threading.Tasks;
using Trips.Common;
using Trips.Domain.Events;
using Trips.ReadModel.Repositories;

namespace Trips.ReadModel.EventHandlers
{
    internal sealed class CustomerAssignedToTripHandler : IEventHandler<CustomerAssignedToTrip>
    {
        private readonly ITripRepository _tripsRepository;

        public CustomerAssignedToTripHandler(ITripRepository tripsRepository)
        {
            _tripsRepository = tripsRepository;
        }

        Task IEventHandler<CustomerAssignedToTrip>.HandleAsync(
            CustomerAssignedToTrip @event, 
            CancellationToken cancellationToken)
        {
            return _tripsRepository.AssignCustomerToTripAsync(
                @event.TripId, 
                @event.CustomerId, 
                cancellationToken);
        }
    }
}
