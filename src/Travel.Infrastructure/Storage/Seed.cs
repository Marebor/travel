using MongoDB.Driver;
using System;
using Travel.Common.Storage;

namespace Travel.Infrastructure.Storage
{
    public static class Seed
    {

        public static void SeedDatabase(this IMongoDatabase db)
        {
            var travelEvents = db.GetCollection<EventWrapper>("Events-Travel");
        }
    }
}
