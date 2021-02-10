using Trips.Domain.Authorization;

namespace Trips.Infrastructure
{
    internal sealed class FakeUserDataProvider : IUserDataProvider
    {
        IUser IUserDataProvider.GetUserData()
        {
            return new HttpContextUserDataProvider.User
            {
                Name = "Trips Admin",
                Role = UserRole.Admin
            };
        }
    }
}
