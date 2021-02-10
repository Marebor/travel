namespace Trips.Domain.Repositories
{
    internal interface IOwnedBy
    {
        string Owner { get; }
    }
}
