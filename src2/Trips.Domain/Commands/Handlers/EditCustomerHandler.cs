using System.Threading;
using System.Threading.Tasks;
using Trips.Domain.Authorization;
using Trips.Domain.Events;
using Trips.Domain.Repositories;
using Trips.Domain.Validation;

namespace Trips.Domain.Commands.Handlers
{
    internal sealed class EditCustomerHandler : ICommandHandler<EditCustomer>
    {
        private readonly IEntityRepository _entityRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IUserDataProvider _userDataProvider;

        public EditCustomerHandler(
            IEntityRepository entityRepository, 
            IEventPublisher eventPublisher,
            IUserDataProvider userDataProvider)
        {
            _entityRepository = entityRepository;
            _eventPublisher = eventPublisher;
            _userDataProvider = userDataProvider;
        }

        async Task ICommandHandler<EditCustomer>.HandleAsync(EditCustomer command, CancellationToken cancellationToken)
        {
            var customer = await _entityRepository.GetEntityByIdAsync<Customer>(command.CustomerId, cancellationToken);

            Validate.NotNull(
                customer,
                $"Customer with ID = {command.CustomerId} does not exist.");

            var user = _userDataProvider.GetUserData();

            Validate.UserPermission(
                customer.Owner,
                user,
                $"User {user.Name} is not permitted to edit customer with ID = {customer.Id}.");

            await _eventPublisher.PublishAsync(
                new CustomerUpdated(
                    command.CustomerId,
                    command.Name,
                    command.Owner,
                    command.CorrelationId,
                    customer.SequenceNumber + 1), 
                customer.Id,
                cancellationToken);
        }
    }
}
