using System;
using Travel.Common.Cqrs;

namespace Travel.Domain.Travel.Commands
{
    public class CreateTravel : ICommand
    {
        public Guid CommandId { get; set; }
        public Guid AggregateId { get; set; }
        public int AggregateVersion => 0;
        public string Destination { get; set; }
        public DateTime Date { get; set; }
    }
}
