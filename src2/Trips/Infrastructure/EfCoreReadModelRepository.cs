using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Trips.ReadModel;
using Trips.ReadModel.Repositories;

namespace Trips.Infrastructure
{
    internal sealed class EfCoreReadModelRepository : 
        ICustomerRepository, 
        ITripRepository
    {
        private readonly TripsReadModelContext _context;

        public EfCoreReadModelRepository(TripsReadModelContext context)
        {
            _context = context;
        }

        async Task ITripRepository.AssignCustomerToTripAsync(
            Guid tripId,
            Guid customerId, 
            CancellationToken cancellationToken)
        {
            var trip = await _context.Trips
                .SingleAsync(t => t.UniqueId == tripId, cancellationToken);
            var customer = await _context.Customers
                .SingleAsync(c => c.UniqueId == customerId, cancellationToken);

            trip.ParticipantsList.Add(customer);

            await _context.SaveChangesAsync(cancellationToken);
        }

        async Task<ICustomer> ICustomerRepository.CreateCustomerAsync(
            Guid id, 
            string name, 
            string owner, 
            CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.AddAsync(new TripsReadModelContext.Customer 
            {
                UniqueId = id,
                Name = name, 
                Owner = owner 
            }, 
            cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return customer.Entity;
        }

        async Task<ITrip> ITripRepository.CreateTripAsync(
            Guid id, 
            string destination, 
            bool isCancelled, 
            string owner, 
            CancellationToken cancellationToken)
        {
            var trip = await _context.Trips.AddAsync(new TripsReadModelContext.Trip
            {
                UniqueId = id,
                Destination = destination,
                IsCancelled = isCancelled,
                Owner = owner,
            },
            cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return trip.Entity;
        }

        async Task<IReadOnlyCollection<ICustomer>> ICustomerRepository.GetCustomersAsync(
            CancellationToken cancellationToken)
        {
            var customers = await _context.Customers.ToArrayAsync(cancellationToken);

            return customers;
        }

        async Task<ICustomer> ICustomerRepository.GetCustomerByUserFriendlyIdAsync(
            int id, 
            CancellationToken cancellationToken)
        {
            var customer = await _context.Customers
                .SingleOrDefaultAsync(c => c.Id == id, cancellationToken);

            return customer;
        }

        async Task<ICustomer> ICustomerRepository.GetCustomerByUniqueIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var customer = await _context.Customers
                .SingleOrDefaultAsync(c => c.UniqueId == id, cancellationToken);

            return customer;
        }

        async Task<IReadOnlyCollection<ITrip>> ITripRepository.GetTripsAsync(
            CancellationToken cancellationToken)
        {
            var trips = await _context.Trips
                .Include(t => t.ParticipantsList)
                .ToArrayAsync(cancellationToken);

            return trips;
        }

        async Task<ITrip> ITripRepository.GetTripByUserFriendlyIdAsync(
            int id, 
            CancellationToken cancellationToken)
        {
            var trip = await _context.Trips
                .Include(t => t.ParticipantsList)
                .SingleOrDefaultAsync(t => t.Id == id, cancellationToken);

            return trip;
        }

        async Task<ITrip> ITripRepository.GetTripByUniqueIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var trip = await _context.Trips
                .SingleOrDefaultAsync(t => t.UniqueId == id, cancellationToken);

            return trip;
        }

        async Task<ICustomer> ICustomerRepository.UpdateCustomerAsync(
            Guid customerId,
            string? name,
            string? owner, 
            CancellationToken cancellationToken)
        {
            var customer = await _context.Customers.SingleAsync(c => c.UniqueId == customerId, cancellationToken);

            if (name != null)
            {
                customer.Name = name;
            }

            if (owner != null)
            {
                customer.Owner = owner;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return customer;
        }

        async Task<ITrip> ITripRepository.UpdateTripAsync(
            Guid tripId,
            string? destination,
            bool? isCancelled,
            string? owner, 
            CancellationToken cancellationToken)
        {
            var trip = await _context.Trips.SingleAsync(t => t.UniqueId == tripId, cancellationToken);

            if (destination != null)
            {
                trip.Destination = destination;
            }

            if (isCancelled.HasValue)
            {
                trip.IsCancelled = isCancelled.Value;
            }

            if (owner != null)
            {
                trip.Owner = owner;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return trip;
        }
    }
}
