using System.Threading;
using System.Threading.Tasks;
using Trips.Common;
using Trips.Domain.Events;
using Trips.ReadModel.Repositories;

namespace Trips.ReadModel.EventHandlers
{
    internal sealed class CustomerCreatedHandler : IEventHandler<CustomerCreated>
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerCreatedHandler(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        Task IEventHandler<CustomerCreated>.HandleAsync(CustomerCreated @event, CancellationToken cancellationToken)
        {
            return _customerRepository.CreateCustomerAsync(
                @event.CustomerId,
                @event.Name, 
                @event.Owner, 
                cancellationToken);
        }
    }
}
