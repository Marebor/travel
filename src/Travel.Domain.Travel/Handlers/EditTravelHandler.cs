using System;
using System.Threading.Tasks;
using Travel.Common.Auth;
using Travel.Common.Cqrs;
using Travel.Common.ErrorHandling;
using Travel.Common.ErrorHandling.Exceptions;
using Travel.Common.Storage;
using Travel.Domain.Travel.Commands;
using Travel.Domain.Travel.Events;

namespace Travel.Domain.Travel.Handlers
{
    public class EditTravelHandler : ICommandHandler<EditTravel>
    {
        private readonly IAggregateStore<Models.Travel> store;
        private readonly IEventPublisher eventPublisher;

        public EditTravelHandler(IAggregateStore<Models.Travel> store, IEventPublisher eventPublisher)
        {
            this.store = store;
            this.eventPublisher = eventPublisher;
        }

        public async Task Handle(EditTravel command)
        {
            if (command.Id == null || command.Id == Guid.Empty)
            {
                throw new IncorrectRequestException(ErrorCodes.ParameterCannotBeEmpty, nameof(command.Id));
            }

            if (string.IsNullOrWhiteSpace(command.Destination) && !command.Date.HasValue)
            {
                throw new IncorrectRequestException(ErrorCodes.ParameterCannotBeEmpty, $"{nameof(command.Destination)}, {nameof(command.Date)}");
            }

            Models.Travel travel = await store.Get(command.Id);

            if (travel == null)
            {
                throw new IncorrectRequestException(ErrorCodes.ResourceDoesNotExist, nameof(travel));
            }

            if (travel.Owner != command.Requester && command.RequesterRole != Roles.Admin)
            {
                throw new UnauthorizedUserException();
            }

            await eventPublisher.Publish(new TravelUpdated
            {
                Id = command.Id,
                Destination = string.IsNullOrWhiteSpace(command.Destination) ? travel.Destination : command.Destination,
                Date = command.Date.HasValue ? command.Date.Value : travel.Date,
            });
        }
    }
}
