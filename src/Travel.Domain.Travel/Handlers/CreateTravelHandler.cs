using System;
using System.Threading.Tasks;
using Travel.Common.Cqrs;
using Travel.Common.ErrorHandling;
using Travel.Common.ErrorHandling.Exceptions;
using Travel.Common.Storage;
using Travel.Domain.Travel.Commands;
using Travel.Domain.Travel.Events;

namespace Travel.Domain.Travel.Handlers
{
    public class CreateTravelHandler : ICommandHandler<CreateTravel>
    {
        private readonly IAggregateStore<Models.Travel> store;
        private readonly IEventPublisher eventPublisher;

        public CreateTravelHandler(IAggregateStore<Models.Travel> store, IEventPublisher eventPublisher)
        {
            this.store = store;
            this.eventPublisher = eventPublisher;
        }

        public async Task Handle(CreateTravel command)
        {
            if (command.Id == null || command.Id == Guid.Empty)
            {
                throw new IncorrectRequestException(ErrorCodes.ParameterCannotBeEmpty, nameof(command.Id));
            }

            if (string.IsNullOrWhiteSpace(command.Owner))
            {
                throw new IncorrectRequestException(ErrorCodes.ParameterCannotBeEmpty, nameof(command.Owner));
            }

            if (string.IsNullOrWhiteSpace(command.Destination))
            {
                throw new IncorrectRequestException(ErrorCodes.ParameterCannotBeEmpty, nameof(command.Destination));
            }

            Models.Travel travel = await store.Get(command.Id);

            if (travel != null)
            {
                throw new IncorrectRequestException(ErrorCodes.IdAlreadyExists, nameof(command.Id));
            }

            await eventPublisher.Publish(new TravelCreated
            {
                Id = command.Id,
                Owner = command.Owner,
                Destination = command.Destination,
                Date = command.Date,
            });
        }
    }
}
