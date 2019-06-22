using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Travel.Common.Storage
{
    public interface IQueryableStore<T>
    {
        Task<IEnumerable<TResult>> Query<TResult>(Expression<Func<T, bool>> filter, Expression<Func<T, TResult>> selection);
    }
}
