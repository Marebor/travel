using System;
using Travel.Common.Model;
using Travel.Domain.Travel.Events;

namespace Travel.Domain.Travel.Models
{
    public class Travel : Aggregate
        , IApplyEvent<TravelCreated>
        , IApplyEvent<TravelUpdated>
        , IApplyEvent<TravelDeleted>
    {
        public bool Deleted { get; private set; }
        public string Owner { get; private set; }
        public string Destination { get; private set; }
        public DateTime Date { get; private set; }

        public Travel()
        {
        }

        public void Apply(TravelCreated @event)
        {
            Id = @event.Id;
            Owner = @event.Owner;
            Destination = @event.Destination;
            Date = @event.Date;
        }

        public void Apply(TravelUpdated @event)
        {
            Destination = @event.Destination;
            Date = @event.Date;
        }

        public void Apply(TravelDeleted @event)
        {
            Deleted = true;
        }
    }
}
