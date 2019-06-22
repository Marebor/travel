using System.Threading.Tasks;

namespace Travel.Common.Cqrs
{
    public interface IQueryExecutor
    {
        Task<TResult> Execute<TQuery, TResult>(TQuery query) where TQuery : IQuery<TResult>;
    }
}
