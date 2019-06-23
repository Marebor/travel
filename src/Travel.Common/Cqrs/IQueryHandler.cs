using System.Collections.Generic;
using System.Threading.Tasks;

namespace Travel.Common.Cqrs
{
    public interface IQueryHandler
    {
    }

    public interface IQueryHandler<TQuery, TResult> : IQueryHandler where TQuery : IQuery<TResult>
    {
        Task<IEnumerable<TResult>> Handle(TQuery query);
    }
}
