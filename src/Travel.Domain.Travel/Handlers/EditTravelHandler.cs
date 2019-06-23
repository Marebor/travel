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
            if (command.AggregateId == null || command.AggregateId == Guid.Empty)
            {
                throw new IncorrectRequestException(ErrorCodes.ParameterCannotBeEmpty, nameof(command.AggregateId));
            }

            if (string.IsNullOrWhiteSpace(command.Destination) && !command.Date.HasValue)
            {
                throw new IncorrectRequestException(ErrorCodes.ParameterCannotBeEmpty, $"{nameof(command.Destination)}, {nameof(command.Date)}");
            }

            Models.Travel travel = await store.Get(command.AggregateId);

            if (travel == null || travel.Deleted)
            {
                throw new IncorrectRequestException(ErrorCodes.ResourceDoesNotExist, nameof(travel));
            }

            if (travel.Version != command.AggregateVersion)
            {
                throw new ResourceStateChangedException(nameof(Travel), travel.Id, travel.Version);
            }

            if (travel.Owner != command.Requester && command.RequesterRole != Roles.Admin)
            {
                throw new UnauthorizedUserException();
            }

            var @event = new TravelUpdated
            {
                RelatedCommandId = command.CommandId,
                Id = travel.Id,
                Destination = string.IsNullOrWhiteSpace(command.Destination) ? travel.Destination : command.Destination,
                Date = command.Date.HasValue ? command.Date.Value : travel.Date,
            };
            travel.ApplyEvent(@event);
            @event.AggregateVersion = travel.Version;

            await eventPublisher.Publish(@event);
        }
    }
}
