using System;

namespace Travel.Common.Cqrs
{
    public interface IEvent
    {
        Guid RelatedCommandId { get; }
        int AggregateVersion { get; }
    }
}
