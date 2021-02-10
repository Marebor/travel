using System;

namespace Trips.Domain.Events
{
    public sealed class CustomerAssignedToTrip : EventBase
    {
        public CustomerAssignedToTrip(
            Guid customerId,
            Guid tripId, 
            Guid correlationId,
            int sequenceNumber) : base(correlationId, sequenceNumber)
        {
            CustomerId = customerId;
            TripId = tripId;
        }

        public Guid CustomerId { get; }
        public Guid TripId { get; }
    }
}
