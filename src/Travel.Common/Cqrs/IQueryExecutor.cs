using System.Collections.Generic;
using System.Threading.Tasks;

namespace Travel.Common.Cqrs
{
    public interface IQueryExecutor
    {
        Task<IEnumerable<TResult>> Execute<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
    }
}
