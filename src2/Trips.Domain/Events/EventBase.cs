using System;
using Trips.Common;

namespace Trips.Domain.Events
{
    public abstract class EventBase : IEvent
    {
        protected EventBase(Guid correlationId, int sequenceNumber)
        {
            CorrelationId = correlationId;
            SequenceNumber = sequenceNumber;
        }

        public Guid CorrelationId { get; }
        public int SequenceNumber { get; }
    }
}
