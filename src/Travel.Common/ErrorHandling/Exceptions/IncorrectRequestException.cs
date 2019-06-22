using System.Linq;

namespace Travel.Common.ErrorHandling.Exceptions
{
    public class IncorrectRequestException : TravelException
    {
        public IncorrectRequestException(string reason, string parameter) 
            : base($"Incorrect parameters provided: {parameter}. Failure reason: {reason}")
        {
        }
    }
}
