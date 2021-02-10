using System;
using System.Threading;
using System.Threading.Tasks;
using Trips.Domain.Authorization;
using Trips.Domain.Events;

namespace Trips.Domain.Commands.Handlers
{
    internal sealed class CreateCustomerHandler : ICommandHandler<CreateCustomer>
    {
        private readonly IEventPublisher _eventPublisher;
        private readonly IUserDataProvider _userDataProvider;

        public CreateCustomerHandler(
            IEventPublisher eventPublisher,
            IUserDataProvider userDataProvider)
        {
            _eventPublisher = eventPublisher;
            _userDataProvider = userDataProvider;
        }

        async Task ICommandHandler<CreateCustomer>.HandleAsync(CreateCustomer command, CancellationToken cancellationToken)
        {
            var user = _userDataProvider.GetUserData();
            var id = Guid.NewGuid();

            await _eventPublisher.PublishAsync(
                new CustomerCreated(
                    id,
                    command.Name,
                    user.Name, 
                    command.CorrelationId,
                    1), 
                id,
                cancellationToken);
        }
    }
}
