using System.Collections.Generic;

namespace Trips.ReadModel
{
    public interface ICustomer : IUniqueId, IUserFriendlyId, IOwnedBy
    {
        string Name { get; }
        IReadOnlyCollection<ITrip> Trips { get; }
    }
}
