﻿using System;
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
    public class CreateTravelHandler : ICommandHandler<CreateTravel>
    {
        private readonly IAggregateStore<Models.Travel> store;
        private readonly IEventPublisher eventPublisher;
        private readonly IIdentityProvider identityProvider;

        public async Task Handle(CreateTravel command)
        {
            if (command.AggregateId == null || command.AggregateId == Guid.Empty)
            {
                throw new IncorrectRequestException(ErrorCodes.ParameterCannotBeEmpty, nameof(command.AggregateId));
            }

            if (string.IsNullOrWhiteSpace(command.Destination))
            {
                throw new IncorrectRequestException(ErrorCodes.ParameterCannotBeEmpty, nameof(command.Destination));
            }

            Models.Travel travel = await store.Get(command.AggregateId);

            if (travel != null)
            {
                throw new IncorrectRequestException(ErrorCodes.IdAlreadyExists, nameof(command.AggregateId));
            }

            await eventPublisher.Publish(new TravelCreated
            {
                RelatedCommandId = command.CommandId,
                AggregateVersion = 1,
                Id = Guid.NewGuid(),
                Owner = identityProvider.GetIdentity().Username,
                Destination = command.Destination,
                Date = command.Date,
            });
        }
    }
}
