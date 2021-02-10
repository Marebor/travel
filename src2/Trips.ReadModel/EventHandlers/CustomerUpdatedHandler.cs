using System.Threading;
using System.Threading.Tasks;
using Trips.Common;
using Trips.Domain.Events;
using Trips.ReadModel.Repositories;

namespace Trips.ReadModel.EventHandlers
{
    internal sealed class CustomerUpdatedHandler : IEventHandler<CustomerUpdated>
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerUpdatedHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        Task IEventHandler<CustomerUpdated>.HandleAsync(CustomerUpdated @event, CancellationToken cancellationToken)
        {
            return _customerRepository.UpdateCustomerAsync(
                @event.CustomerId,
                @event.Name,
                @event.Owner,
                cancellationToken);
        }
    }
}
