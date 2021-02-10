using System;

namespace Trips.Domain.Commands
{
    public sealed class EditTrip : CommandBase
    {
        public Guid TripId { get; }
        public string? Destination { get; }
        public bool? IsCancelled { get; }
        public string? Owner { get; }

        public EditTrip(
            Guid tripId, 
            string? destination, 
            bool? isCancelled, 
            string? owner,
            Guid correlationId) : base(correlationId)
        {
            TripId = tripId;
            Destination = destination;
            IsCancelled = isCancelled;
            Owner= owner;
        }
    }
}
