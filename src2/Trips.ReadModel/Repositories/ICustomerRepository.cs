using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Trips.ReadModel.Repositories
{
    public interface ICustomerRepository
    {
        Task<IReadOnlyCollection<ICustomer>> GetCustomersAsync(
            CancellationToken cancellationToken);
        Task<ICustomer> GetCustomerByUserFriendlyIdAsync(
            int id, 
            CancellationToken cancellationToken);
        Task<ICustomer> GetCustomerByUniqueIdAsync(
            Guid id,
            CancellationToken cancellationToken);
        Task<ICustomer> CreateCustomerAsync(
            Guid id, 
            string name, 
            string owner, 
            CancellationToken cancellationToken);
        Task<ICustomer> UpdateCustomerAsync(
            Guid uniqueId, 
            string? name, 
            string? owner, 
            CancellationToken cancellationToken);
    }
}
