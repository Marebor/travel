using System;
using System.Threading;
using System.Threading.Tasks;
using Trips.Domain.Authorization;
using Trips.Domain.Events;
using Trips.Domain.Repositories;
using Trips.Domain.Validation;

namespace Trips.Domain.Commands.Handlers
{
    internal sealed class CreateTripHandler : ICommandHandler<CreateTrip>
    {
        private readonly IDestinationRepository _destinationRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IEntityRepository _tripRepository;
        private readonly IUserDataProvider _userDataProvider;

        public CreateTripHandler(
            IDestinationRepository destinationRepository, 
            IEventPublisher eventPublisher, 
            IEntityRepository tripRepository, 
            IUserDataProvider userDataProvider)
        {
            _destinationRepository = destinationRepository;
            _eventPublisher = eventPublisher;
            _tripRepository = tripRepository;
            _userDataProvider = userDataProvider;
        }

        async Task ICommandHandler<CreateTrip>.HandleAsync(CreateTrip command, CancellationToken cancellationToken)
        {
            var destinationExists = await _destinationRepository
                .DestinationExistsAsync(command.Destination, cancellationToken);

            if (!destinationExists)
            {
                throw new ValidationException($"Destination '{command.Destination}' does not exist.");
            }

            var user = _userDataProvider.GetUserData();

            if (user?.Name is null)
            {
                throw new ValidationException($"Only known user can create a trip.");
            }

            var id = Guid.NewGuid();

            await _eventPublisher.PublishAsync(
                new TripCreated(
                    id,
                    command.Destination,
                    false,
                    user.Name, 
                    command.CorrelationId,
                    1), 
                id,
                cancellationToken);
        }
    }
}
