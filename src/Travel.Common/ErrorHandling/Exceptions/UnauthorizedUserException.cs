using System.Linq;

namespace Travel.Common.ErrorHandling.Exceptions
{
    public class UnauthorizedUserException : TravelException
    {
        public UnauthorizedUserException() : base("User not authorized to perform operation.")
        {
        }
    }
}
