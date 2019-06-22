using System;

namespace Travel.ReadModel.Models
{
    public class Travel
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public bool Deleted { get; set; }
        public string Owner { get; set; }
        public string Destination { get; set; }
        public DateTime Date { get; set; }
    }
}
