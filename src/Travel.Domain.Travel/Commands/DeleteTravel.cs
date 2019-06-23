using System;
using Travel.Common.Cqrs;

namespace Travel.Domain.Travel.Commands
{
    public class DeleteTravel : ICommand
    {
        public Guid CommandId { get; set; }
        public Guid AggregateId { get; set; }
        public int AggregateVersion { get; set; }
    }
}
