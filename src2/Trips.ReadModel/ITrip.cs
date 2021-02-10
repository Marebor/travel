using System.Collections.Generic;

namespace Trips.ReadModel
{
    public interface ITrip : IUniqueId, IUserFriendlyId, IOwnedBy
    {
        string Destination { get; }
        bool IsCancelled { get; }
        IReadOnlyCollection<ICustomer> Participants { get; }
    }
}
