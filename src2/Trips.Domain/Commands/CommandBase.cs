using System;

namespace Trips.Domain.Commands
{
    public abstract class CommandBase : ICommand
    {
        protected CommandBase(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public Guid CorrelationId { get; }
    }
}
