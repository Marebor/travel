using System;

namespace Trips.Domain.Commands
{
    public sealed class CreateCustomer : CommandBase
    {
        public string Name { get; }

        public CreateCustomer(string name, Guid correlationId) : base(correlationId)
        {
            Name = name;
        }
    }
}
