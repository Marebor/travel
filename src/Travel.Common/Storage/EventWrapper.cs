using Travel.Common.Cqrs;

namespace Travel.Common.Storage
{
    public class EventWrapper
    {
        public string AggregateId { get; set; }
        public int AggregateVersion { get; set; }
        public IEvent Event { get; set; }
    }
}
