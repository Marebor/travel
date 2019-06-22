using System;
using System.Threading.Tasks;
using Travel.Common.Model;

namespace Travel.Common.Storage
{
    public interface IAggregateStore<T> where T : Aggregate, new()
    {
        Task<T> Get(Guid id);
    }
}
