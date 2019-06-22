using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Travel.Common.Storage;

namespace Travel.Infrastructure.Storage
{
    public class QueryableStore<T> : IQueryableStore<T>
    {
        private readonly IMongoDatabase database;
        private IMongoQueryable<T> ReadModels => database
            .GetCollection<T>($"{nameof(ReadModels)}-{typeof(T).Name}")
            .AsQueryable();

        public QueryableStore(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<TResult>> Query<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selection)
            => await ReadModels.Where(filter).Select(selection).ToListAsync();
    }
}
