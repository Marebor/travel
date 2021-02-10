using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Trips.Domain.Commands;
using Trips.Infrastructure;
using Trips.Models;
using Trips.ReadModel;
using Trips.ReadModel.Repositories;

namespace Trips.Controllers
{
    [ApiController]
    [Route("api/trips")]
    public class TripsController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly ICustomerRepository _customerRepository;
        private readonly ITripRepository _tripRepository;
        private readonly ICreatedEntityIdResolver _createdEntityIdResolver;

        public TripsController(
            ICommandDispatcher commandDispatcher,
            ICustomerRepository customerRepository,
            ITripRepository tripRepository,
            ICreatedEntityIdResolver createdEntityIdResolver)
        {
            _commandDispatcher = commandDispatcher;
            _customerRepository = customerRepository;
            _tripRepository = tripRepository;
            _createdEntityIdResolver = createdEntityIdResolver;
        }

        [HttpGet]
        public Task<IReadOnlyCollection<ITrip>> GetTripsAsync(
            CancellationToken cancellationToken)
        {
            return _tripRepository.GetTripsAsync(cancellationToken);
        }

        [HttpGet("{id}", Name = nameof(GetTripByIdAsync))]
        public async Task<ActionResult<ITrip>> GetTripByIdAsync(
            int id, 
            CancellationToken cancellationToken)
        {
            var trip = await _tripRepository.GetTripByUserFriendlyIdAsync(id, cancellationToken);

            if (trip is null)
            {
                return NotFound();
            }

            return Ok(trip);
        }

        [HttpPost]
        //[Authorize]
        public async Task<IActionResult> CreateTripAsync(
            Models.CreateTrip trip, 
            CancellationToken cancellationToken)
        {
            var correlationId = Guid.NewGuid();

            await _commandDispatcher.DispatchAsync(new Domain.Commands.CreateTrip(
                    trip.Destination,
                    correlationId),
                cancellationToken);

            var uniqueId = await _createdEntityIdResolver
                .ResolveIdByCorrelationIdAsync(correlationId, cancellationToken);
            var createdTrip = await _tripRepository
                .GetTripByUniqueIdAsync(uniqueId, cancellationToken);

            return CreatedAtRoute(nameof(GetTripByIdAsync), new { createdTrip.Id }, createdTrip);
        }

        [HttpPut("{id}")]
        //[Authorize]
        public async Task EditTripAsync(
            int id,
            UpdateTrip updatedTrip,
            CancellationToken cancellationToken)
        {
            var correlationId = Guid.NewGuid();
            var trip = await _tripRepository.GetTripByUserFriendlyIdAsync(id, cancellationToken);

            await _commandDispatcher.DispatchAsync(new EditTrip(
                    trip.UniqueId,
                    updatedTrip.Destination,
                    updatedTrip.IsCancelled,
                    updatedTrip.Owner,
                    correlationId), 
                cancellationToken);
        }

        [HttpPost("{tripId}/participants")]
        //[Authorize]
        public async Task AddTripParticipantAsync(
            int tripId,
            Models.AssignCustomerToTrip command,
            CancellationToken cancellationToken)
        {
            var correlationId = Guid.NewGuid();
            var trip = await _tripRepository
                .GetTripByUserFriendlyIdAsync(tripId, cancellationToken);
            var customer = await _customerRepository
                .GetCustomerByUserFriendlyIdAsync(command.CustomerId, cancellationToken);

            await _commandDispatcher.DispatchAsync(new Domain.Commands.AssignCustomerToTrip(
                    customer.UniqueId,
                    trip.UniqueId,
                    correlationId), 
                cancellationToken);
        }
    }
}
