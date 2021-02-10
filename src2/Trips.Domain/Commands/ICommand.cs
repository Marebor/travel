using System;

namespace Trips.Domain.Commands
{
    public interface ICommand
    {
        Guid CorrelationId { get; }
    }
}
