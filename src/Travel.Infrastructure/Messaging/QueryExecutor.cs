using System;
using System.Threading.Tasks;
using Travel.Common.Cqrs;

namespace Travel.Infrastructure.Messaging
{
    public class QueryExecutor : IQueryExecutor
    {
        private readonly Func<Type, Type, IQueryHandler> handlersFactory;

        public QueryExecutor(Func<Type, Type, IQueryHandler> handlersFactory)
        {
            this.handlersFactory = handlersFactory;
        }

        public Task<TResult> Execute<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>
        {
            IQueryHandler<TQuery, TResult> handler = handlersFactory(typeof(TQuery), typeof(TResult)) as IQueryHandler<TQuery, TResult>;

            if (handler == null)
            {
                throw new InvalidOperationException($"Handler for query: {query.GetType().Name} is not registred.");
            }

            return handler.Handle(query);
        }
    }
}
