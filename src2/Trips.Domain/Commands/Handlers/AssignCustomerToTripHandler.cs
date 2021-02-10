using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Trips.Domain.Authorization;
using Trips.Domain.Events;
using Trips.Domain.Repositories;
using Trips.Domain.Validation;

namespace Trips.Domain.Commands.Handlers
{
    internal sealed class AssignCustomerToTripHandler : ICommandHandler<AssignCustomerToTrip>
    {
        private readonly IEntityRepository _entityRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IUserDataProvider _userDataProvider;

        public AssignCustomerToTripHandler(
            IEntityRepository entityRepository, 
            IEventPublisher eventPublisher,
            IUserDataProvider userDataProvider)
        {
            _entityRepository = entityRepository;
            _eventPublisher = eventPublisher;
            _userDataProvider = userDataProvider;
        }

        async Task ICommandHandler<AssignCustomerToTrip>.HandleAsync(AssignCustomerToTrip command, CancellationToken cancellationToken)
        {
            var trip = await _entityRepository.GetEntityByIdAsync<Trip>(command.TripId, cancellationToken);

            Validate.NotNull(
                trip, 
                $"Trip with ID = {command.TripId} does not exist.");

            var user = _userDataProvider.GetUserData();

            Validate.UserPermission(
                trip.Owner,
                user, 
                $"User {user.Name} is not permitted to edit trip with ID = {command.TripId}.");

            var customer = await _entityRepository.GetEntityByIdAsync<Customer>(command.CustomerId, cancellationToken);

            Validate.NotNull(
                customer,
                $"Customer with ID = {command.CustomerId} does not exist.");
            Validate.UserPermission(
                customer.Owner,
                user,
                $"User {user.Name} is not permitted to edit customer with ID = {command.CustomerId}.");

            if (trip.AssignedCustomersIds.Contains(command.CustomerId))
            {
                throw new ValidationException($"Customer with ID = {customer.Id} is already assigned to trip with ID = {trip.Id}.");
            }

            await _eventPublisher.PublishAsync(
                new CustomerAssignedToTrip(
                    customer.Id, 
                    trip.Id, 
                    command.CorrelationId,
                    trip.SequenceNumber), 
                trip.Id,
                cancellationToken);
        }
    }
}
