using System;
using System.Collections.Generic;
using Trips.Domain.Events;

namespace Trips.Domain.Repositories
{
    internal sealed class Trip : 
        IEntity, 
        IOwnedBy,
        IEntityBuilder<Trip, TripCreated>,
        IEntityBuilder<Trip, TripUpdated>,
        IEntityBuilder<Trip, CustomerAssignedToTrip>
    {
        private readonly List<Guid> _assignedCustomersIds = new List<Guid>();

        public Guid Id { get; private set; }
        public string Destination { get; private set; }
        public IReadOnlyCollection<Guid> AssignedCustomersIds => _assignedCustomersIds;
        public bool IsCancelled { get; private set; }
        public string Owner { get; private set; }
        public int SequenceNumber { get; private set; }

        Trip IEntityBuilder<Trip, TripCreated>.Apply(Trip entity, TripCreated @event)
        {
            return new Trip
            {
                Id = @event.TripId,
                Destination = @event.Destination,
                IsCancelled = @event.IsCancelled,
                Owner = @event.Owner,
                SequenceNumber = @event.SequenceNumber,
            };
        }

        Trip IEntityBuilder<Trip, TripUpdated>.Apply(Trip entity, TripUpdated @event)
        {
            if (@event.Destination != null)
            {
                entity.Destination = @event.Destination;
            }

            if (@event.IsCancelled.HasValue)
            {
                entity.IsCancelled = @event.IsCancelled.Value;
            }

            if (@event.Owner != null)
            {
                entity.Owner = @event.Owner;
            }

            entity.SequenceNumber = @event.SequenceNumber;

            return entity;
        }

        Trip IEntityBuilder<Trip, CustomerAssignedToTrip>.Apply(Trip entity, CustomerAssignedToTrip @event)
        {
            entity._assignedCustomersIds.Add(@event.CustomerId);

            entity.SequenceNumber = @event.SequenceNumber;

            return entity;
        }
    }
}
