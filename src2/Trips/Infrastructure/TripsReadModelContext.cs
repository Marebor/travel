using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Trips.ReadModel;

namespace Trips.Infrastructure
{
    internal sealed class TripsReadModelContext : DbContext
    {
        public DbSet<Trip> Trips { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public TripsReadModelContext(DbContextOptions<TripsReadModelContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trip>()
                .Property(trip => trip.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Trip>()
                .HasMany(trip => trip.ParticipantsList);

            modelBuilder.Entity<Customer>()
                .Property(customer => customer.Id)
                .ValueGeneratedOnAdd();
            modelBuilder.Entity<Customer>()
                .HasMany(customer => customer.TripsList);
        }

        internal class Trip : ITrip
        {
            public int Id { get; set; }
            public Guid UniqueId { get; set; }
            public string Destination { get; set; }
            public bool IsCancelled { get; set; }
            public string Owner { get; set; }
            public IReadOnlyCollection<ICustomer> Participants => ParticipantsList.ToArray();
            internal IList<Customer> ParticipantsList { get; set; }

            public Trip()
            {
                ParticipantsList = new List<Customer>();
            }
        }

        internal class Customer : ICustomer
        {
            public int Id { get; set; }
            public Guid UniqueId { get; set; }
            public string Name { get; set; }
            public string Owner { get; set; }
            public IReadOnlyCollection<ITrip> Trips => TripsList.ToArray();
            internal IList<Trip> TripsList { get; set; }

            public Customer()
            {
                TripsList = new List<Trip>();
            }
        }
    }
}
