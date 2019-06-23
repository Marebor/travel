using System;

namespace Travel.Common.ErrorHandling.Exceptions
{
    public class ResourceStateChangedException : TravelException
    {
        public ResourceStateChangedException(string resourceType, Guid resourceId, int currentVersion)
            : base($"{resourceType} with ID = {resourceId} has changed. Current version: {currentVersion}")
        {
        }
    }
}
