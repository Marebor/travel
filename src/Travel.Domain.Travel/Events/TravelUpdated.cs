using System;
using Travel.Common.Cqrs;

namespace Travel.Domain.Travel.Events
{
    public class TravelUpdated : IEvent
    {
        public Guid Id { get; set; }
        public string Destination { get; set; }
        public DateTime Date { get; set; }
    }
}
