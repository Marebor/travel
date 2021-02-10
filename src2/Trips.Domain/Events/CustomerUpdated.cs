using System;

namespace Trips.Domain.Events
{
    public sealed class CustomerUpdated : EventBase
    {
        public CustomerUpdated(
            Guid customerId, 
            string? name, 
            string? owner, 
            Guid correlationId,
            int sequenceNumber) : base(correlationId, sequenceNumber)
        {
            CustomerId = customerId;
            Name = name;
            Owner = owner;
        }

        public Guid CustomerId { get; }
        public string? Name { get; }
        public string? Owner { get; }
    }
}
