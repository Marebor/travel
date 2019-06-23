using System;
using Travel.Common.Cqrs;

namespace Travel.Domain.Travel.Events
{
    public class TravelUpdated : IEvent
    {
        public Guid RelatedCommandId { get; set; }
        public int AggregateVersion { get; set; }
        public Guid Id { get; set; }
        public string Destination { get; set; }
        public DateTime Date { get; set; }
    }
}
