using System;
using System.Threading.Tasks;
using Travel.Common.Auth;
using Travel.Common.Cqrs;
using Travel.Common.ErrorHandling;
using Travel.Common.ErrorHandling.Exceptions;
using Travel.Common.Storage;
using Travel.Domain.Travel.Commands;
using Travel.Domain.Travel.Events;
using Travel.Domain.Travel.Models;

namespace Travel.Domain.Travel.Handlers
{
    public class DeleteTravelHandler : ICommandHandler<DeleteTravel>
    {
        private readonly IAggregateStore<Models.Travel> store;
        private readonly IEventPublisher eventPublisher;
        private readonly IIdentityProvider identityProvider;

        public DeleteTravelHandler(IAggregateStore<Models.Travel> store, IEventPublisher eventPublisher, IIdentityProvider identityProvider)
        {
            this.store = store;
            this.eventPublisher = eventPublisher;
            this.identityProvider = identityProvider;
        }

        public async Task Handle(DeleteTravel command)
        {
            if (command.AggregateId == null || command.AggregateId == Guid.Empty)
            {
                throw new IncorrectRequestException(ErrorCodes.ParameterCannotBeEmpty, nameof(command.AggregateId));
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

            Identity identity = identityProvider.GetIdentity();

            if (travel.Owner != identity.Username && identity.Role != Roles.Admin)
            {
                throw new UnauthorizedUserException();
            }

            var @event = new TravelDeleted
            {
                RelatedCommandId = command.CommandId,
                Id = command.AggregateId,
            };
            travel.ApplyEvent(@event);
            @event.AggregateVersion = travel.Version;

            await eventPublisher.Publish(@event);
        }
    }
}
