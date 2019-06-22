using System;
using Travel.Common.Cqrs;

namespace Travel.Domain.Travel.Commands
{
    public class CreateTravel : ICommand
    {
        public Guid Id { get; set; }
        public string Owner { get; set; }
        public string Destination { get; set; }
        public DateTime Date { get; set; }
    }
}
