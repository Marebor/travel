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
    public class DeleteTravelHandler : ICommandHandler<DeleteTravel>
    {
        private readonly IAggregateStore<Models.Travel> store;
        private readonly IEventPublisher eventPublisher;

        public DeleteTravelHandler(IAggregateStore<Models.Travel> store, IEventPublisher eventPublisher)
        {
            this.store = store;
            this.eventPublisher = eventPublisher;
        }

        public async Task Handle(DeleteTravel command)
        {
            if (command.AggregateId == null || command.AggregateId == Guid.Empty)
            {
                throw new IncorrectRequestException(ErrorCodes.ParameterCannotBeEmpty, nameof(command.AggregateId));
            }
            
            Models.Travel travel = await store.Get(command.AggregateId);

            if (travel == null)
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

            var @event = new TravelDeleted
            {
                RelatedCommandId = command.CommandId,
                Id = command.AggregateId,
            };
            travel.Apply(@event);
            @event.AggregateVersion = travel.Version;

            await eventPublisher.Publish(@event);
        }
    }
}
