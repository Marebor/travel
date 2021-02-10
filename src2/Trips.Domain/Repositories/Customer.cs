using System;
using Trips.Domain.Events;

namespace Trips.Domain.Repositories
{
    internal sealed class Customer : 
        IEntity, 
        IOwnedBy,
        IEntityBuilder<Customer, CustomerCreated>,
        IEntityBuilder<Customer, CustomerUpdated>
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Owner { get; private set; }
        public int SequenceNumber { get; private set; }

        Customer IEntityBuilder<Customer, CustomerCreated>.Apply(Customer entity, CustomerCreated @event)
        {
            return new Customer
            {
                Id = @event.CustomerId,
                Name = @event.Name,
                Owner = @event.Owner,
                SequenceNumber = @event.SequenceNumber,
            };
        }

        Customer IEntityBuilder<Customer, CustomerUpdated>.Apply(Customer entity, CustomerUpdated @event)
        {
            if (@event.Name != null)
            {
                entity.Name = @event.Name;
            }

            if (@event.Owner != null)
            {
                entity.Owner = @event.Owner;
            }

            entity.SequenceNumber = @event.SequenceNumber;

            return entity;
        }
    }
}
