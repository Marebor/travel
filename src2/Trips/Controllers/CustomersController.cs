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
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICreatedEntityIdResolver _createdEntityIdResolver;

        public CustomersController(
            ICommandDispatcher commandDispatcher,
            ICustomerRepository customerRepository,
            ICreatedEntityIdResolver createdEntityIdResolver)
        {
            _commandDispatcher = commandDispatcher;
            _customerRepository = customerRepository;
            _createdEntityIdResolver = createdEntityIdResolver;
        }

        [HttpGet]
        public Task<IReadOnlyCollection<ICustomer>> GetCustomersAsync(
            CancellationToken cancellationToken)
        {
            return _customerRepository.GetCustomersAsync(cancellationToken);
        }

        [HttpGet("{id}", Name = nameof(GetCustomerByIdAsync))]
        public async Task<ActionResult<ITrip>> GetCustomerByIdAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var trip = await _customerRepository.GetCustomerByUserFriendlyIdAsync(id, cancellationToken);

            if (trip is null)
            {
                return NotFound();
            }

            return Ok(trip);
        }

        [HttpPost]
        //[Authorize]
        public async Task<ActionResult> CreateCustomerAsync(
            Models.CreateCustomer customer,
            CancellationToken cancellationToken)
        {
            var correlationId = Guid.NewGuid();

            await _commandDispatcher.DispatchAsync(
                new Domain.Commands.CreateCustomer(
                    customer.Name,
                    correlationId),
                cancellationToken);

            var uniqueId = await _createdEntityIdResolver
                .ResolveIdByCorrelationIdAsync(correlationId, cancellationToken);
            var createdCustomer = await _customerRepository
                .GetCustomerByUniqueIdAsync(uniqueId, cancellationToken);

            return CreatedAtRoute(nameof(GetCustomerByIdAsync), new { createdCustomer.Id }, createdCustomer);
        }

        [HttpPut("{id}")]
        //[Authorize]
        public async Task EditCustomerAsync(
            int id,
            UpdateCustomer updatedCustomer,
            CancellationToken cancellationToken)
        {
            var correlationId = Guid.NewGuid();
            var customer = await _customerRepository
                .GetCustomerByUserFriendlyIdAsync(id, cancellationToken);

            await _commandDispatcher.DispatchAsync(
                new EditCustomer(
                    customer.UniqueId,
                    updatedCustomer.Name,
                    updatedCustomer.Owner,
                    correlationId), 
                cancellationToken);
        }
    }
}
