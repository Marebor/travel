using System;

namespace Trips.Domain.Commands
{
    public sealed class AssignCustomerToTrip : CommandBase
    {
        public Guid CustomerId { get; }
        public Guid TripId { get; }

        public AssignCustomerToTrip(Guid customerId, Guid tripId, Guid correlationId) : base(correlationId)
        {
            CustomerId = customerId;
            TripId = tripId;
        }
    }
}
