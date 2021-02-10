namespace Trips.Domain.Authorization
{
    public interface IUser
    {
        string Name { get; }
        UserRole Role { get; }
    }
}
