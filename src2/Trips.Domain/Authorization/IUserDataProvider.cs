namespace Trips.Domain.Authorization
{
    public interface IUserDataProvider
    {
        IUser GetUserData();
    }
}
