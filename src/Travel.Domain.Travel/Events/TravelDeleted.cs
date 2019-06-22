using System;
using Travel.Common.Cqrs;

namespace Travel.Domain.Travel.Events
{
    public class TravelDeleted : IEvent
    {
        public Guid Id { get; set; }
    }
}
