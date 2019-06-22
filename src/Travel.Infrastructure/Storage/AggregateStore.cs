using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Travel.Common.Cqrs;
using Travel.Common.Model;
using Travel.Common.Storage;

namespace Travel.Infrastructure.Storage
{
    public class AggregateStore<T> : IAggregateStore<T> where T : Aggregate, new()
    {
        private readonly IMongoDatabase database;
        private IMongoCollection<EventWrapper> Events => database.GetCollection<EventWrapper>($"{nameof(Events)}-{typeof(T).Name}");

        public AggregateStore(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<T> Get(Guid id)
        {
            IList<IEvent> events = await Events.AsQueryable()
                .Where(x => x.AggregateId == id.ToString())
                .OrderBy(x => x.AggregateVersion)
                .Select(x => x.Event)
                .ToListAsync();

            return Aggregate.Builder.Build<T>(events);
        }
    }
}
