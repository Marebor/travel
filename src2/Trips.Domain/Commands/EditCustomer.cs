using System;

namespace Trips.Domain.Commands
{
    public sealed class EditCustomer : CommandBase
    {
        public Guid CustomerId { get; }
        public string? Name { get; }
        public string? Owner { get; }

        public EditCustomer(
            Guid customerId, 
            string? name, 
            string? owner, 
            Guid correlationId) : base(correlationId)
        {
            CustomerId = customerId;
            Name = name;
            Owner = owner;
        }
    }
}
