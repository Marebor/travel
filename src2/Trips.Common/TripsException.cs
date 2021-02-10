using System;

namespace Trips.Common
{
    public class TripsException : Exception
    {
        public string MessageForEndUser { get; }
        public TripsException(string messageForEndUser) : this(messageForEndUser, messageForEndUser) { }

        public TripsException(string messageForEndUser, string technicalMessage) : base(technicalMessage) 
        {
            MessageForEndUser = messageForEndUser;
        }
    }
}
