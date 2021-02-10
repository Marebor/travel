using System;

namespace Trips.Domain.Repositories
{
    internal interface IEntity
    {
        Guid Id { get; }
        int SequenceNumber { get; }
    }
}
