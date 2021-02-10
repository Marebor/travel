using System;

namespace Trips.Common
{
    public interface IEvent
    {
        Guid CorrelationId { get; }
        int SequenceNumber { get; }
    }
}
