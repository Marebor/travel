using System.Threading;
using System.Threading.Tasks;
using Trips.Common;
using Trips.Domain.Events;
using Trips.ReadModel.Repositories;

namespace Trips.ReadModel.EventHandlers
{
    internal sealed class TripCreatedHandler : IEventHandler<TripCreated>
    {
        private readonly ITripRepository _tripRepository;

        public TripCreatedHandler(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        Task IEventHandler<TripCreated>.HandleAsync(TripCreated @event, CancellationToken cancellationToken)
        {
            return _tripRepository.CreateTripAsync(
                @event.TripId,
                @event.Destination,
                @event.IsCancelled,
                @event.Owner, 
                cancellationToken);
        }
    }
}
