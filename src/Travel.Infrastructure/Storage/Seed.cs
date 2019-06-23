using MongoDB.Driver;
using System;
using System.Collections.Generic;
using Travel.Common.Storage;
using Travel.Domain.Travel.Events;

namespace Travel.Infrastructure.Storage
{
    public static class Seed
    {
        public static void SeedDatabase(this IMongoDatabase db)
        {
            var travelEvents = db.GetCollection<EventWrapper>("Events-Travel");
            var travelReadModels = db.GetCollection<ReadModel.Models.Travel>("ReadModels-Travel");

            Events.Collection.ForEach(x => travelEvents.ReplaceOne(y => false, x, new UpdateOptions { IsUpsert = true }));
            ReadModels.Collection.ForEach(x => travelReadModels.ReplaceOne(y => false, x, new UpdateOptions { IsUpsert = true }));
        }

        private static class Events
        {
            public static List<EventWrapper> Collection => new List<EventWrapper>
            {
                new EventWrapper
                {
                    AggregateId = "42CA3738-1608-45A7-8638-B5076042BB3D",
                    AggregateVersion = 1,
                    Event = new TravelCreated
                    {
                        Id = Guid.Parse("42CA3738-1608-45A7-8638-B5076042BB3D"),
                        AggregateVersion = 1,
                        RelatedCommandId = Guid.Parse("FB6AF782-2302-4A3E-B74D-8B6B40DBFBB3"),
                        Owner = "User1",
                        Destination = "Athens",
                        Date = DateTime.Parse("2019-10-01"),
                    }
                },
                new EventWrapper
                {
                    AggregateId = "3F9515FF-8722-439E-87CA-867F6D6A6329",
                    AggregateVersion = 1,
                    Event = new TravelCreated
                    {
                        Id = Guid.Parse("3F9515FF-8722-439E-87CA-867F6D6A6329"),
                        AggregateVersion = 1,
                        RelatedCommandId = Guid.Parse("3E7B90CD-6725-4DB1-B1EE-1E8FDB3CEF84"),
                        Owner = "User2",
                        Destination = "Rome",
                        Date = DateTime.Parse("2019-10-11"),
                    }
                },
                new EventWrapper
                {
                    AggregateId = "ECB842F6-176B-46C5-A64E-6EE9AD12BC8D",
                    AggregateVersion = 1,
                    Event = new TravelCreated
                    {
                        Id = Guid.Parse("ECB842F6-176B-46C5-A64E-6EE9AD12BC8D"),
                        AggregateVersion = 1,
                        RelatedCommandId = Guid.Parse("81079153-0F66-4125-BC1A-71CEBFA7BFAA"),
                        Owner = "User3",
                        Destination = "Barcelona",
                        Date = DateTime.Parse("2019-09-21"),
                    }
                },
                new EventWrapper
                {
                    AggregateId = "02B7AE02-4E6D-4BDA-A356-1244C50924BF",
                    AggregateVersion = 1,
                    Event = new TravelCreated
                    {
                        Id = Guid.Parse("02B7AE02-4E6D-4BDA-A356-1244C50924BF"),
                        AggregateVersion = 1,
                        RelatedCommandId = Guid.Parse("5A343C30-ED8F-42C2-88A6-8B0132DC9009"),
                        Owner = "User4",
                        Destination = "Berlin",
                        Date = DateTime.Parse("2019-10-01"),
                    }
                },
                new EventWrapper
                {
                    AggregateId = "02B7AE02-4E6D-4BDA-A356-1244C50924BF",
                    AggregateVersion = 2,
                    Event = new TravelDeleted
                    {
                        Id = Guid.Parse("02B7AE02-4E6D-4BDA-A356-1244C50924BF"),
                        AggregateVersion = 1,
                        RelatedCommandId = Guid.Parse("AD7079B4-C4CE-4F6A-A404-0FA01EC61F6E"),
                    }
                },
                new EventWrapper
                {
                    AggregateId = "ECB842F6-176B-46C5-A64E-6EE9AD12BC8D",
                    AggregateVersion = 2,
                    Event = new TravelUpdated
                    {
                        Id = Guid.Parse("ECB842F6-176B-46C5-A64E-6EE9AD12BC8D"),
                        AggregateVersion = 2,
                        RelatedCommandId = Guid.Parse("595EC305-1188-49F0-B123-4D17AA7323B5"),
                        Destination = "Athens",
                        Date = DateTime.Parse("2019-10-01"),
                    }
                },
            };
        }

        private static class ReadModels
        {
            public static List<ReadModel.Models.Travel> Collection => new List<ReadModel.Models.Travel>
            {
                new ReadModel.Models.Travel
                {
                    Id = "42CA3738-1608-45A7-8638-B5076042BB3D",
                    Version = 1,
                    Owner = "User1",
                    Destination = "Athens",
                    Date = DateTime.Parse("2019-10-01"),
                    Deleted = false,
                },
                new ReadModel.Models.Travel
                {
                    Id = "3F9515FF-8722-439E-87CA-867F6D6A6329",
                    Version = 1,
                    Owner = "User2",
                    Destination = "Rome",
                    Date = DateTime.Parse("2019-10-11"),
                    Deleted = false,
                },
                new ReadModel.Models.Travel
                {
                    Id = "ECB842F6-176B-46C5-A64E-6EE9AD12BC8D",
                    Version = 2,
                    Owner = "User3",
                    Destination = "Barcelona",
                    Date = DateTime.Parse("2019-09-21"),
                    Deleted = false,
                },
                new ReadModel.Models.Travel
                {
                    Id = "02B7AE02-4E6D-4BDA-A356-1244C50924BF",
                    Version = 2,
                    Owner = "User4",
                    Destination = "Berlin",
                    Date = DateTime.Parse("2019-10-01"),
                    Deleted = true,
                },
            };
        }
    }
}
