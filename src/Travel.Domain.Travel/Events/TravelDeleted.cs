using System;
using Travel.Common.Cqrs;

namespace Travel.Domain.Travel.Events
{
    public class TravelDeleted : IEvent
    {
        public Guid RelatedCommandId { get; set; }
        public int AggregateVersion { get; set; }
        public Guid Id { get; set; }
    }
}
