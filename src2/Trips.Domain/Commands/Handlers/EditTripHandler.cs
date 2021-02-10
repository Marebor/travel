using System.Threading;
using System.Threading.Tasks;
using Trips.Domain.Authorization;
using Trips.Domain.Events;
using Trips.Domain.Repositories;
using Trips.Domain.Validation;

namespace Trips.Domain.Commands.Handlers
{
    internal sealed class EditTripHandler : ICommandHandler<EditTrip>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IEntityRepository _entityRepository;
        private readonly IUserDataProvider _userDataProvider;
        private readonly IDestinationRepository _destinationRepository;

        public EditTripHandler(
            IEventPublisher eventPublisher, 
            IEntityRepository entityRepository, 
            IUserDataProvider userDataProvider,
            IDestinationRepository destinationRepository)
        {
            _eventPublisher = eventPublisher;
            _entityRepository = entityRepository;
            _userDataProvider = userDataProvider;
            _destinationRepository = destinationRepository;
        }

        async Task ICommandHandler<EditTrip>.HandleAsync(EditTrip command, CancellationToken cancellationToken)
        {
            var trip = await _entityRepository.GetEntityByIdAsync<Trip>(command.TripId, cancellationToken);

            Validate.NotNull(
                trip,
                $"Trip with ID = {command.TripId} does not exist.");

            var user = _userDataProvider.GetUserData();

            Validate.UserPermission(
                trip.Owner,
                user,
                $"User {user.Name} does not have permission to edit trip with ID = {trip.Id}");

            if (command.Destination != null && command.Destination != trip.Destination)
            {
                var destinationExists = await _destinationRepository
                    .DestinationExistsAsync(command.Destination, cancellationToken);

                if (!destinationExists)
                {
                    throw new ValidationException($"Destination '{command.Destination}' does not exist.");
                }
            }

            await _eventPublisher.PublishAsync(
                new TripUpdated(
                    command.TripId,
                    command.Destination,
                    command.IsCancelled,
                    command.Owner,
                    command.CorrelationId,
                    trip.SequenceNumber + 1),
                trip.Id,
                cancellationToken);
        }
    }
}
