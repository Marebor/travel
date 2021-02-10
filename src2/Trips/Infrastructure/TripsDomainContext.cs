using Microsoft.EntityFrameworkCore;
using System;

namespace Trips.Infrastructure
{
    internal sealed class TripsDomainContext : DbContext
    {
        public DbSet<Event> Events { get; set; }

        public TripsDomainContext(DbContextOptions<TripsDomainContext> options) : base(options) { }

        internal sealed class Event
        {
            public int Id { get; set; }
            public Guid RelatedEntityId { get; set; }
            public string EventType { get; set; }
            public string Content { get; set; }
        }
    }
}
