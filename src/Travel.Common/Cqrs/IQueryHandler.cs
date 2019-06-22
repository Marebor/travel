using System.Threading.Tasks;

namespace Travel.Common.Cqrs
{
    public interface IQueryHandler
    {
    }

    public interface IQueryHandler<TQuery, TResult> : IQueryHandler where TQuery : IQuery
    {
        Task<TResult> Handle(TQuery query);
    }
}
