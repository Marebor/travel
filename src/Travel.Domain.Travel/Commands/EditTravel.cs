using System;
using Travel.Common.Cqrs;

namespace Travel.Domain.Travel.Commands
{
    public class EditTravel : ICommand
    {
        public Guid Id { get; set; }
        public string Destination { get; set; }
        public DateTime? Date { get; set; }
        public string Requester { get; set; }
        public string RequesterRole { get; set; }
    }
}
