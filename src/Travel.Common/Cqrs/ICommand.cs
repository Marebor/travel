using System;

namespace Travel.Common.Cqrs
{
    public interface ICommand
    {
        Guid CommandId { get; }
        Guid AggregateId { get; }
        int AggregateVersion { get; }
    }
}
