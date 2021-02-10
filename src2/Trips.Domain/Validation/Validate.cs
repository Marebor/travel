using Trips.Domain.Authorization;

namespace Trips.Domain.Validation
{
    internal static class Validate
    {
        public static void NotNull<T>(T obj, string message) where T : class
        {
            if (obj is null)
            {
                throw new ValidationException(message);
            }
        }

        public static void UserPermission(string owner, IUser requester, string message)
        {
            if (requester.Role == UserRole.Admin || owner == requester.Name)
            {
                return;
            }

            throw new ValidationException(message);
        }
    }
}
