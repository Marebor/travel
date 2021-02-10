using System.Threading;
using System.Threading.Tasks;
using Trips.Common;
using Trips.Domain.Events;
using Trips.ReadModel.Repositories;

namespace Trips.ReadModel.EventHandlers
{
    internal sealed class TripUpdatedHandler : IEventHandler<TripUpdated>
    {
        private readonly ITripRepository _tripRepository;

        public TripUpdatedHandler(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        Task IEventHandler<TripUpdated>.HandleAsync(TripUpdated @event, CancellationToken cancellationToken)
        {
            return _tripRepository.UpdateTripAsync(
                @event.TripId,
                @event.Destination,
                @event.IsCancelled,
                @event.Owner, 
                cancellationToken);
        }
    }
}
