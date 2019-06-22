using System;

namespace Travel.Common.ErrorHandling.Exceptions
{
    public class TravelException : Exception
    {
        public TravelException(string message) : base(message)
        {
        }

        public TravelException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
