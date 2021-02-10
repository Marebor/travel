using System;

namespace Trips.Domain.Commands
{
    public sealed class CreateTrip : CommandBase
    {
        public CreateTrip(string destination, Guid correlationId) : base(correlationId)
        {
            Destination = destination;
        }

        public string Destination { get; }
    }
}
