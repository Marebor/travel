using System;

namespace Trips.Domain.Events
{
    public sealed class TripUpdated : EventBase
    {
        public TripUpdated(
            Guid tripId,
            string? destination, 
            bool? isCancelled, 
            string? owner,
            Guid correlationId,
            int sequenceNumber) : base(correlationId, sequenceNumber)
        {
            TripId = tripId;
            Destination = destination;
            IsCancelled = isCancelled;
            Owner = owner;
        }

        public Guid TripId { get; }
        public string? Destination { get; }
        public bool? IsCancelled { get; }
        public string? Owner { get; }
    }
}
