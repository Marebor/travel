using Trips.Common;

namespace Trips.Domain.Validation
{
    public class ValidationException : TripsException
    {
        public ValidationException(string message) : base(message)
        {
        }
    }
}
